#!/usr/bin/env bash
# ===========================================================
# deploy_PP_with_publish.sh - beyahad_backend
# Builds & publishes  locally, packages it,
# and uploads it to the PP remote server.
# ===========================================================

set -e
set +u
if (set -o pipefail >/dev/null 2>&1); then set -o pipefail; fi

# ============================================================
# Robust PowerShell detection — works regardless of which shell
# actually executes this script. Typing "bash deploy_PP_with_publish.sh"
# inside a plain PowerShell prompt can resolve to WSL's bash.exe instead
# of Git Bash's, and WSL uses "/mnt/c/..." for Windows drives while
# Git Bash uses "/c/...". A single hardcoded path breaks under
# whichever convention it doesn't match. Try every known way
# PowerShell might be reachable, in order, so this "just works" on
# any machine/terminal combination without manual intervention.
# ============================================================
find_powershell() {
    local candidates=(
        "powershell.exe"
        "/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe"
        "/mnt/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe"
    )
    local c
    for c in "${candidates[@]}"; do
        if command -v "$c" >/dev/null 2>&1 || [[ -f "$c" ]]; then
            echo "$c"
            return 0
        fi
    done
    return 1
}

POWERSHELL=$(find_powershell) || {
    echo "❌ ERROR: PowerShell not found"
    echo "   Checked: powershell.exe (PATH), Git-Bash path, WSL path."
    exit 1
}

find_winscp() {
    local candidates=(
        "winscp.com"
        "/c/Program Files (x86)/WinSCP/WinSCP.com"
        "/c/Program Files/WinSCP/WinSCP.com"
        "/mnt/c/Program Files (x86)/WinSCP/WinSCP.com"
        "/mnt/c/Program Files/WinSCP/WinSCP.com"
    )
    local c
    for c in "${candidates[@]}"; do
        if command -v "$c" >/dev/null 2>&1 || [[ -f "$c" ]]; then
            echo "$c"; return 0
        fi
    done
    return 1
}

WINSCP=$(find_winscp) || {
    echo "❌ ERROR: WinSCP.com not found. Install WinSCP (winscp.net)."
    exit 1
}

# ============================================================
# Defensive step: shut down WSL if it's running, before it can
# interfere. An active WSL VM brings up a Hyper-V virtual network
# adapter (vEthernet (WSL)) that can disrupt UDP-broadcast-based
# protocols on the host — including SQL Server's named-instance
# resolution via SQL Browser (UDP 1434), which this script depends
# on for "\sql2005". This runs automatically on every machine and
# every invocation, regardless of which shell launched this script,
# so a stray WSL session can't silently break the DB connection.
# "wsl --shutdown" is a no-op (harmless, near-instant) if WSL isn't
# installed or isn't running.
# ============================================================
if command -v wsl.exe >/dev/null 2>&1 || command -v wsl >/dev/null 2>&1; then
    (wsl.exe --shutdown >/dev/null 2>&1 || wsl --shutdown >/dev/null 2>&1 || true)
fi

ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)
cd "$ROOT"

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

PROJECT_NAME=$(basename "$ROOT")
BRANCH=$(git rev-parse --abbrev-ref HEAD)
LOG_FILE="$ROOT/automation_master_log.txt"

log() {
    local msg="$*"
    local timestamp="[ $(date '+%Y-%m-%d %H:%M:%S') ]"
    echo "$timestamp $msg" | tee -a "$LOG_FILE"
}

mkdir -p "$(dirname "$LOG_FILE")"
if [ ! -f "$LOG_FILE" ]; then
    echo "===================================================" > "$LOG_FILE"
    echo "[ $(date '+%Y-%m-%d %H:%M:%S') ] Log initialized" >> "$LOG_FILE"
    echo "===================================================" >> "$LOG_FILE"
fi

# ============================================================
# Load secrets from .env — never hardcode credentials in this script.
# Expected keys in $SCRIPT_DIR/.env:
#   DB_SERVER=172.29.92.20\sql2005
#   DB_NAME=NofTest
#   DB_USER=sqladmin
#   DB_PASSWORD=********
#
# NOTE: this block is placed AFTER the log() definition above (the
# original version of this script called log() from inside the
# DB_SERVER backslash-repair check below, before log() had been
# defined yet — that would have failed with "log: command not found"
# had that repair branch ever actually triggered).
# ============================================================
ENV_FILE="$SCRIPT_DIR/.env"
if [[ ! -f "$ENV_FILE" ]]; then
    echo "❌ ERROR: .env file not found at $ENV_FILE"
    echo "   Create it with DB_SERVER, DB_NAME, DB_USER, DB_PASSWORD (see .env.example)"
    exit 1
fi
set -a
# shellcheck disable=SC1090
source "$ENV_FILE"
set +a

: "${DB_SERVER:?Missing DB_SERVER in .env}"
: "${DB_NAME:?Missing DB_NAME in .env}"
: "${DB_USER:?Missing DB_USER in .env}"
: "${DB_PASSWORD:?Missing DB_PASSWORD in .env}"
: "${SFTP_HOST:?Missing SFTP_HOST in .env}"
: "${SFTP_PORT:?Missing SFTP_PORT in .env}"
: "${SFTP_USER:?Missing SFTP_USER in .env}"
: "${SFTP_PASSWORD:?Missing SFTP_PASSWORD in .env}"
SFTP_UPLOAD_DIR="${SFTP_UPLOAD_DIR:-/pp}"
SFTP_CONFIRM_DIR="${SFTP_CONFIRM_DIR:-/pp}"

# Defensive check: if DB_SERVER was left unquoted in .env (e.g.
# "DB_SERVER=172.29.92.20\sql2005" instead of
# "DB_SERVER=\"172.29.92.20\sql2005\""), bash's own `source` silently drops
# the backslash while parsing the file, turning a valid SQL Server instance
# address like "172.29.92.20\sql2005" into the invalid "172.29.92.20sql2005"
# before this script ever sees it. Detect that shape (digits/dots directly
# followed by letters, no separator) and re-insert the backslash before the
# instance name, so a missing quote in .env on any machine can't silently
# break the DB connection.
if [[ "$DB_SERVER" =~ ^([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})([A-Za-z].*)$ ]]; then
    log "⚠️  DB_SERVER appears to be missing its backslash (unquoted value in .env?) — repairing automatically"
    DB_SERVER="${BASH_REMATCH[1]}\\${BASH_REMATCH[2]}"
    log "   → Using repaired DB_SERVER: $DB_SERVER"
fi

log "🔍 Project: $PROJECT_NAME"
log "🔍 Branch: $BRANCH"
log "   ✓ DB credentials loaded from .env (server: ${DB_SERVER}, db: ${DB_NAME}, user: ${DB_USER})"
log ""

log "==================================================="
log "🔧 CHECKING & INSTALLING DEPENDENCIES"
log "==================================================="

# ============================================================
# STEP 1: Check and install PowerShell prerequisites
# ============================================================
log ""
log "📦 [STEP 1/4] Checking PowerShell prerequisites..."

PREREQ_RESULT=$("$POWERSHELL" -NoProfile -Command '
$ErrorActionPreference = "Stop"
$results = @()

try {
    # Check NuGet Package Provider
    Write-Output "   Checking NuGet Package Provider..."
    $nuget = Get-PackageProvider -Name NuGet -ErrorAction SilentlyContinue -ListAvailable | 
             Where-Object { $_.Version -ge "2.8.5.201" }
    
    if ($null -eq $nuget) {
        Write-Output "   Installing NuGet Package Provider..."
        Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force -Scope CurrentUser | Out-Null
        Write-Output "   [OK] NuGet Package Provider installed"
        $results += "NUGET_INSTALLED"
    } else {
        Write-Output "   [OK] NuGet Package Provider already installed (v$($nuget.Version))"
        $results += "NUGET_EXISTS"
    }
    
    # Check PSGallery Trust Policy
    Write-Output "   Checking PSGallery installation policy..."
    $gallery = Get-PSRepository -Name PSGallery -ErrorAction SilentlyContinue
    
    if ($null -eq $gallery) {
        Write-Output "   PSGallery repository not found, registering..."
        Register-PSRepository -Default -ErrorAction SilentlyContinue | Out-Null
        $gallery = Get-PSRepository -Name PSGallery -ErrorAction SilentlyContinue
    }
    
    if ($gallery.InstallationPolicy -ne "Trusted") {
        Write-Output "   Setting PSGallery as trusted repository..."
        Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
        Write-Output "   [OK] PSGallery set as trusted"
        $results += "PSGALLERY_CONFIGURED"
    } else {
        Write-Output "   [OK] PSGallery already trusted"
        $results += "PSGALLERY_EXISTS"
    }
    
    $results += "SUCCESS"
    Write-Output ($results -join "|")
    
} catch {
    Write-Output "ERROR: $($_.Exception.Message)"
    exit 1
}
' 2>&1)

if [[ "$PREREQ_RESULT" == ERROR* ]]; then
    log "❌ ERROR: Failed to set up PowerShell prerequisites"
    log "   $PREREQ_RESULT"
    exit 1
fi

if [[ "$PREREQ_RESULT" == *"NUGET_INSTALLED"* ]] || [[ "$PREREQ_RESULT" == *"PSGALLERY_CONFIGURED"* ]]; then
    log "   ✓ PowerShell prerequisites configured"
else
    log "   ✓ PowerShell prerequisites already present"
fi

# ============================================================
# STEP 2: Check and install jq
# ============================================================
log ""
log "📦 [STEP 2/4] Checking jq..."
if ! command -v jq &> /dev/null; then
    log "   ⚙  jq not found. Installing..."
    if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
        # Windows with Git Bash - download jq
        JQ_URL="https://github.com/jqlang/jq/releases/download/jq-1.7.1/jq-win64.exe"
        JQ_PATH="/usr/bin/jq.exe"
        if curl -L "$JQ_URL" -o "$JQ_PATH" 2>/dev/null; then
            chmod +x "$JQ_PATH"
            log "   ✓ jq installed successfully"
        else
            log "   ⚠️  jq installation failed, but continuing..."
        fi
    fi
else
    log "   ✓ jq is ready"
fi

# ============================================================
# STEP 3: Check and install SqlServer module
# ============================================================
log ""
log "📦 [STEP 3/4] Checking SQL Server PowerShell module..."

SQL_MODULE_RESULT=$("$POWERSHELL" -NoProfile -Command '
$ErrorActionPreference = "Stop"

try {
    # Check if SqlServer module is available
    $module = Get-Module -ListAvailable -Name SqlServer | Select-Object -First 1
    
    if ($null -eq $module) {
        Write-Output "   SqlServer module not found. Installing for current user..."
        Write-Output "   This may take 1-2 minutes on first run..."
        
        # Install the module
        Install-Module -Name SqlServer -Scope CurrentUser -Force -AllowClobber -SkipPublisherCheck -ErrorAction Stop | Out-Null
        
        Write-Output "   [OK] SqlServer module installed successfully"
        $result = "INSTALLED"
    } else {
        Write-Output "   [OK] SqlServer module already installed (v$($module.Version))"
        $result = "EXISTS"
    }
    
    # Try to import the module
    Write-Output "   Importing SqlServer module..."
    Import-Module SqlServer -ErrorAction Stop | Out-Null
    Write-Output "   [OK] SqlServer module imported successfully"
    
    # Test Invoke-Sqlcmd availability
    if (Get-Command Invoke-Sqlcmd -ErrorAction SilentlyContinue) {
        Write-Output "   [OK] Invoke-Sqlcmd command is available"
        Write-Output "SUCCESS|$result"
    } else {
        throw "Invoke-Sqlcmd command not available after module import"
    }
    
} catch {
    Write-Output "ERROR: $($_.Exception.Message)"
    exit 1
}
' 2>&1)

if [[ "$SQL_MODULE_RESULT" == ERROR* ]]; then
    log "❌ ERROR: Failed to set up SqlServer module"
    log "   $SQL_MODULE_RESULT"
    exit 1
fi

if [[ "$SQL_MODULE_RESULT" == *"INSTALLED"* ]]; then
    log "   ✓ SqlServer module newly installed"
elif [[ "$SQL_MODULE_RESULT" == *"EXISTS"* ]]; then
    log "   ✓ SqlServer module ready"
fi

# ============================================================
# STEP 4: Check Robocopy
# ============================================================
log ""
log "📦 [STEP 4/4] Checking Robocopy..."
ROBOCOPY_CHECK=$("$POWERSHELL" -NoProfile -Command "
    if (Get-Command robocopy -ErrorAction SilentlyContinue) {
        Write-Output 'FOUND'
    } else {
        Write-Output 'NOT_FOUND'
    }
" 2>&1)

if [[ "$ROBOCOPY_CHECK" == *"FOUND"* ]]; then
    log "   ✓ Robocopy is ready"
else
    log "   ❌ ERROR: Robocopy not found"
    log "   Robocopy should be available on all Windows systems"
    exit 1
fi

log ""
log "==================================================="
log "✓ All dependencies ready"
log "==================================================="
log ""

# ============================================================
# Network preflight check — verify the SQL Server host is reachable
# before attempting a DB connection. This runs automatically on every
# machine; if it fails, it gives an actionable message (e.g. connect
# to VPN) instead of surfacing a raw, confusing ADO.NET error.
# ============================================================
DB_HOST_ONLY="${DB_SERVER%%\\*}"
log "🌐 Checking network connectivity to $DB_HOST_ONLY:1433..."
NET_CHECK=$("$POWERSHELL" -NoProfile -Command "
    try {
        \$result = Test-NetConnection -ComputerName '$DB_HOST_ONLY' -Port 1433 -WarningAction SilentlyContinue
        if (\$result.TcpTestSucceeded) { Write-Output 'REACHABLE' } else { Write-Output 'UNREACHABLE' }
    } catch {
        Write-Output 'UNREACHABLE'
    }
" 2>&1 | tr -d '\r\n')

if [[ "$NET_CHECK" != "REACHABLE" ]]; then
    log "❌ ERROR: Cannot reach $DB_HOST_ONLY on port 1433"
    log "   This usually means:"
    log "   • You are not connected to the company VPN"
    log "   • You are on a different network than the DB server expects"
    log "   • A firewall is blocking the connection"
    log "   Please connect to VPN / verify network access, then re-run this script."
    exit 1
fi
log "   ✓ Network reachable"
log ""

# Get DB config
log "🔗 Loading config from database..."

DB_MAX_RETRIES=3
DB_RETRY_DELAY=5
db_attempt=1
db_values=""

while (( db_attempt <= DB_MAX_RETRIES )); do
    if (( db_attempt > 1 )); then
        log "   ⟳ Retry $db_attempt/$DB_MAX_RETRIES after transient failure (waiting ${DB_RETRY_DELAY}s)..."
        sleep "$DB_RETRY_DELAY"
    fi

    db_values=$("$POWERSHELL" -NoProfile -Command '
      try {
        Import-Module SqlServer -ErrorAction Stop | Out-Null
        # DB_SERVER / DB_NAME / DB_USER / DB_PASSWORD are inherited from the
        # parent bash process (loaded from .env via "set -a; source .env")
        # — never hardcoded here.
        $connectionString = "Server=$($env:DB_SERVER);Database=$($env:DB_NAME);User ID=$($env:DB_USER);Password=$($env:DB_PASSWORD);TrustServerCertificate=True;";
        $query = "SELECT TOP 1 * FROM ProjectDepJoyConfig WHERE Project='"'$PROJECT_NAME'"' AND Branch='"'$BRANCH'"' ORDER BY ID DESC";
        $result = Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop;
        if ($null -eq $result) {
          Write-Output "NO_RESULTS"
        } else {
          foreach ($row in $result) {
            $line = ($row.Project + "|||" + $row.Branch + "|||" + $row.RemoteServer + "|||" + $row.RemoteUser + "|||" + $row.RemotePassword + "|||" + $row.LocalPublishDir + "|||" + $row.ZipPath)
            Write-Output $line
          }
        }
      } catch {
        Write-Output ("ERROR: " + $_.Exception.Message)
      }
    ')

    # A named-instance resolution timeout ("error: 40") is the classic
    # transient failure mode here (SQL Browser/UDP 1434 flakiness over VPN)
    # — retry on that specifically, plus any other ERROR, up to the cap.
    if [[ "$db_values" != ERROR* ]]; then
        break
    fi

    log "   ⚠️  Attempt $db_attempt failed: $db_values"
    (( db_attempt++ ))
done

if [[ "$db_values" == ERROR* ]]; then
    log "❌ Database connection failed"
    log "   $db_values"
    exit 1
elif [[ "$db_values" == "NO_RESULTS" ]]; then
    log "❌ No config found for $PROJECT_NAME / $BRANCH"
    exit 1
fi

PROJECT=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $1}')
BRANCH=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $2}')
REMOTE_SERVER=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $3}')
REMOTE_USER=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $4}')
REMOTE_PASSWORD=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $5}')
LOCAL_PUBLISH_DIR=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $6}')
ZIP_PATH=$(echo "$db_values" | awk -F'\\|\\|\\|' '{print $7}')

RAW_REMOTE="$REMOTE_SERVER"
RAW_LOCAL="$LOCAL_PUBLISH_DIR"
RAW_ZIP="$ZIP_PATH"

log "   ✓ Config loaded"
log ""

# Normalize paths
normalize_path() {
    local winpath="$1"
    local path="${winpath//\\//}"
    # Use $OSTYPE rather than $MSYSTEM — MSYSTEM isn't reliably exported
    # when this script runs non-interactively (e.g. Task Scheduler), which
    # was causing this branch to be skipped and producing a doubled slash
    # like "C://Users/..." previously.
    if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" || "$OSTYPE" == "win32" ]]; then
        # Git-Bash/MSYS style: C:/Users/... -> /c/Users/...
        path=$(echo "$path" | sed -E 's#^([A-Za-z]):#/\L\1#')
    elif grep -qi microsoft /proc/version 2>/dev/null; then
        # WSL: $OSTYPE reports linux-gnu here (not msys), but the filesystem
        # only exposes Windows drives under /mnt/<drive>, so a path like
        # "C:/Users/..." can never resolve via test -d / cd — it needs to be
        # "/mnt/c/Users/...".
        path=$(echo "$path" | sed -E 's#^([A-Za-z]):#/mnt/\L\1#')
    else
        # Only insert the ":/" if it isn't already there, so this is safe
        # to call on a path that's already been through the conversion above.
        path=$(echo "$path" | sed -E 's#^([A-Za-z]):([^/])#\1:/\2#')
    fi
    echo "$path"
}

REMOTE_SERVER=$(normalize_path "$REMOTE_SERVER")
LOCAL_PUBLISH_DIR=$(normalize_path "$LOCAL_PUBLISH_DIR")
ZIP_PATH=$(normalize_path "$ZIP_PATH")

log "==================================================="
log "🚀 DEPLOYMENT STARTED"
log "==================================================="
log ""

# ==================================================
# STEP 0: PREPARE PUBLISH DIRECTORY
# ==================================================
log "📂 Preparing publish directory..."

# Check if project folder exists in D:\Publish and clean it
PUBLISH_RESULT=$("$POWERSHELL" -NoProfile -Command "
    try {
        \$publishBase = Split-Path '$RAW_LOCAL'
        \$projectFolder = Split-Path '$RAW_LOCAL' -Leaf
        \$fullPath = '$RAW_LOCAL'
        
        Write-Output \"   → Publish base: \$publishBase\"
        Write-Output \"   → Project folder: \$projectFolder\"
        Write-Output \"   → Full path: \$fullPath\"
        
        # Check if the project folder exists
        if (Test-Path \$fullPath) {
            Write-Output \"   → Found existing folder, deleting...\"
            Remove-Item \$fullPath -Recurse -Force -ErrorAction Stop
            Write-Output \"   ✓ Existing folder deleted\"
        } else {
            Write-Output \"   → No existing folder found\"
        }
        
        # Create fresh directory
        Write-Output \"   → Creating fresh directory...\"
        New-Item -ItemType Directory -Path \$fullPath -Force -ErrorAction Stop | Out-Null
        Write-Output \"   ✓ Fresh directory created\"
        
        Write-Output 'SUCCESS'
    } catch {
        Write-Output \"ERROR: \$(\$_.Exception.Message)\"
    }
" 2>&1)

# Show the output
echo "$PUBLISH_RESULT" | while IFS= read -r line; do
    if [[ "$line" == "   "* ]] || [[ "$line" == "SUCCESS" ]]; then
        log "$line"
    fi
done

if [[ "$PUBLISH_RESULT" != *"SUCCESS"* ]]; then
    log "❌ Failed to prepare publish directory"
    log "   $PUBLISH_RESULT"
    exit 1
fi

log ""

# ==================================================
# STEP 1: BUILD & PUBLISH Beyahad_Backend
# ==================================================
# NOTE: Beyahad_Backend.sln contains multiple projects (Nofshonit.Infrastructure,
# Nofshonit.Common, Nofshonit.Services, Nofshonit.BL, Nofshonit.Repositories,
# TicketsHubRepository, OrderDll, Nofshonit.Logs, and a broken external reference
# ManagementStockClientCP under ..\dts.ecommerce\ which isn't checked out here).
# Publishing the whole .sln fails because of that broken reference, and would
# also try to publish non-web library projects unnecessarily. The actual web
# app is Nofshonit.Api — dotnet publish on this single .csproj still pulls in
# every project it depends on, so this is equivalent to publishing the site
# without touching the projects that don't belong in the publish output.
CSPROJ="$ROOT/Nofshonit.Api/Nofshonit.Api.csproj"

if [[ ! -f "$CSPROJ" ]]; then
    log "❌ Project file not found: $CSPROJ"
    log "   Update the CSPROJ path in this script if Nofshonit.Api.csproj lives elsewhere."
    exit 1
fi

log "📦 Publishing Beyahad_Backend: $(basename "$CSPROJ")..."

dotnet publish \
    "$CSPROJ" \
    -c Release \
    -o "$RAW_LOCAL"

if [[ $? -ne 0 ]]; then
    log "❌ dotnet publish failed"
    exit 1
fi

log "✓ Publish completed"
log ""

# ==================================================
# STEP 1B: DEVSEC — VULNERABILITY SCAN (NuGet + front-end libraries)
# ==================================================
# Matches the DCODE reference script's DevSec approach exactly: runs
# synchronously, before packaging, with direct calls using "|| true" —
# no timeout/heartbeat machinery, scanning the published output
# ($RAW_LOCAL) rather than source wwwroot. Informational only — findings
# are logged and included in the email but never block the deploy.
#
# One deliberate deviation from DCODE: NuGet is restricted to nuget.org,
# because (unlike DCODE) this project also has a private Telerik feed
# configured that isn't reachable for this check — confirmed by the
# "NU1900 ... nuget.telerik.com" warning on every build. Without this
# restriction, the scan hangs trying to reach that feed.
log "==================================================="
log "🔒 DEVSEC VULNERABILITY SCAN"
log "==================================================="

log "   📦 Scanning NuGet packages (dotnet list package --vulnerable)..."
NUGET_SCAN=$(dotnet list "$CSPROJ" package --vulnerable --include-transitive --source https://api.nuget.org/v3/index.json 2>&1) || true
NUGET_VULN_COUNT=0
NUGET_SCAN_OUTPUT=""
if echo "$NUGET_SCAN" | grep -qi "has the following vulnerable packages"; then
    log "   ⚠️  Vulnerable NuGet packages found:"
    while IFS= read -r scanline; do
        log "      | $scanline"
    done <<< "$NUGET_SCAN"
    NUGET_VULN_COUNT=$(echo "$NUGET_SCAN" | grep -c '^\s*>' || true)
    NUGET_SCAN_OUTPUT="$NUGET_SCAN"
else
    log "   ✓ No known-vulnerable NuGet packages found"
fi

log "   🌐 Scanning front-end libraries (retire.js) in: $RAW_LOCAL"
RETIRE_OUTPUT=""
if command -v npx &> /dev/null; then
    RETIRE_SCAN=$(npx --yes retire --path "$RAW_LOCAL" --outputformat text --severity low 2>&1) || true
    if [[ -n "$RETIRE_SCAN" ]] && echo "$RETIRE_SCAN" | grep -qi "vulnerabilit"; then
        log "   ⚠️  Vulnerable front-end libraries found:"
        while IFS= read -r scanline; do
            log "      | $scanline"
        done <<< "$RETIRE_SCAN"
        RETIRE_OUTPUT="$RETIRE_SCAN"
    else
        log "   ✓ No known-vulnerable front-end libraries found"
    fi
else
    log "   ⚠️  npx/Node.js not found — skipping front-end library scan"
fi

# --- Combined summary for the single "DevSec Scan" email row ---
if [[ "$NUGET_VULN_COUNT" -gt 0 ]] && [[ -n "$RETIRE_OUTPUT" ]]; then
    DEVSEC_SUMMARY="⚠️ $NUGET_VULN_COUNT vulnerable NuGet package(s) + JS library issues found — see details below"
    DEVSEC_HAS_ISSUES=true
elif [[ "$NUGET_VULN_COUNT" -gt 0 ]]; then
    DEVSEC_SUMMARY="⚠️ $NUGET_VULN_COUNT vulnerable NuGet package(s) found — see details below"
    DEVSEC_HAS_ISSUES=true
elif [[ -n "$RETIRE_OUTPUT" ]]; then
    DEVSEC_SUMMARY="⚠️ Vulnerable JS library(ies) found — see details below"
    DEVSEC_HAS_ISSUES=true
else
    DEVSEC_SUMMARY="✅ No known vulnerabilities found"
    DEVSEC_HAS_ISSUES=false
fi

log "==================================================="
log "✓ DevSec scan complete"
log "==================================================="
log ""

# ==================================================
# STEP 2: CREATING DEPLOYMENT PACKAGE
# ==================================================
log "==================================================="
log "📦 CREATING DEPLOYMENT PACKAGE"
log "==================================================="

TEMP="$ROOT/.deploy_temp"
if [ -d "$TEMP" ]; then
    for item in "$TEMP"/*; do
        case "$item" in
            */dev) ;;
            *) rm -rf "$item" 2>/dev/null || true ;;
        esac
    done
fi
mkdir -p "$TEMP"

log "   Copying files to temp directory..."

"$POWERSHELL" -NoProfile -Command "
    \$robocopyArgs = @(
        '$RAW_LOCAL',
        '$TEMP',
        '/MIR', '/R:0', '/W:0',
        '/XD', 'dev', '/XD', '.git',
        '/NFL', '/NDL', '/NJH', '/NJS'
    )
    \$null = & robocopy @robocopyArgs
    if (\$LASTEXITCODE -lt 8) { Write-Output 'OK' }
" 2>&1

log "   ✓ Files copied to temp"

# Add environment configs (SCRIPT_DIR was set near the top of this script)
if [ -f "$SCRIPT_DIR/web_PP.config" ]; then
    cp "$SCRIPT_DIR/web_PP.config" "$TEMP/web_PP.config"
    cp "$SCRIPT_DIR/web_PP.config" "$TEMP/web.config"
    log "   ✓ web_PP.config added"
fi

if [ -f "$SCRIPT_DIR/appsettings_PP.json" ]; then
    cp "$SCRIPT_DIR/appsettings_PP.json" "$TEMP/appsettings_PP.json"
    cp "$SCRIPT_DIR/appsettings_PP.json" "$TEMP/appsettings.json"
    log "   ✓ appsettings_PP.json added"
fi

log "   Creating ZIP archive..."

"$POWERSHELL" -NoProfile -Command "
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    if (Test-Path '$RAW_ZIP') { Remove-Item '$RAW_ZIP' -Force }
    [System.IO.Compression.ZipFile]::CreateFromDirectory('$TEMP', '$RAW_ZIP', 'Optimal', \$false)
" 2>&1

if [ ! -f "$RAW_ZIP" ]; then
    log "❌ ZIP creation failed"
    exit 1
fi

log "   ✓ ZIP created: $(basename "$RAW_ZIP")"
log ""

# ==================================================
# STEP 3: UPLOADING TO SFTP VAULT
# ==================================================
log "==================================================="
log "📤 UPLOADING TO SFTP VAULT ($SFTP_HOST:$SFTP_PORT$SFTP_UPLOAD_DIR)"
log "==================================================="

SFTP_ZIP_NAME="${PROJECT}_${BRANCH}_publish.zip"
WINSCP_LOG="$ROOT/.winscp_upload.log"
rm -f "$WINSCP_LOG"

UPLOAD_SCRIPT=$(mktemp)
cat > "$UPLOAD_SCRIPT" <<EOF
option batch abort
option confirm off
option transfer binary
open sftp://${SFTP_USER}:${SFTP_PASSWORD}@${SFTP_HOST}:${SFTP_PORT}/ -hostkey=*
lcd $(cygpath -m "$(dirname "$ZIP_PATH")")
put $(basename "$ZIP_PATH") ${SFTP_UPLOAD_DIR}/${SFTP_ZIP_NAME}
exit
EOF

MSYS_NO_PATHCONV=1 timeout 120 "$WINSCP" /log="$(cygpath -w "$WINSCP_LOG" 2>/dev/null || echo "$WINSCP_LOG")" /ini=nul \
    /script="$(cygpath -w "$UPLOAD_SCRIPT" | sed 's/^\\//')" 2>&1
UPLOAD_EXIT=$?
rm -f "$UPLOAD_SCRIPT"

if [[ $UPLOAD_EXIT -eq 124 ]]; then
    log "❌ SFTP upload timed out after 120s"
    exit 1
fi
if [[ $UPLOAD_EXIT -ne 0 ]]; then
    log "❌ SFTP upload failed (exit $UPLOAD_EXIT)"
    [[ -f "$WINSCP_LOG" ]] && tail -20 "$WINSCP_LOG" | while IFS= read -r l; do log "   $l"; done
    exit 1
fi

log "   ✓ Uploaded to ${SFTP_UPLOAD_DIR}/${SFTP_ZIP_NAME}"
log ""

# ==================================================
# STEP 4: WAITING FOR WATCHER CONFIRMATION
# ==================================================
log "==================================================="
log "⏳ WAITING FOR WATCHER CONFIRMATION"
log "==================================================="

LOCAL_CONFIRM_DIR="$ROOT/.deploy_confirmations"
mkdir -p "$LOCAL_CONFIRM_DIR"
LOCAL_CONFIRM_WIN=$(cygpath -w "$LOCAL_CONFIRM_DIR" | sed 's/^\\//')

connStr="Server=${DB_SERVER};Database=${DB_NAME};User ID=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;"
SERVER_COUNT=$("$POWERSHELL" -NoProfile -Command "
    try {
        Import-Module SqlServer -ErrorAction Stop | Out-Null
        \$r = Invoke-Sqlcmd -ConnectionString '$connStr' -Query \"SELECT COUNT(*) AS cnt FROM ProjectRules WHERE ProjectName='${PROJECT_NAME}' AND Branch='${BRANCH}' AND ServerName IS NOT NULL AND ServerName != ''\"
        Write-Output \$r.cnt
    } catch { Write-Output 1 }
" 2>/dev/null | tr -d '\r\n') || SERVER_COUNT=1
[[ "$SERVER_COUNT" =~ ^[0-9]+$ ]] || SERVER_COUNT=1
[[ "$SERVER_COUNT" -eq 0 ]] && SERVER_COUNT=1

log "   Expecting responses from $SERVER_COUNT server(s)..."

WATCHER_CONFIRMED=false
WATCHER_RESULT_FILES=()
declare -A SEEN_DEPLOY_FILES
CHECK_INTERVAL=4
MAX_WAIT=600

set +e
for ((i=1; i<=MAX_WAIT/CHECK_INTERVAL; i++)); do
    ELAPSED=$((i * CHECK_INTERVAL))

    TMP_LIST=$(mktemp)
    TMP_SCRIPT=$(mktemp)
    cat > "$TMP_SCRIPT" <<EOF
option batch continue
option confirm off
open sftp://${SFTP_USER}:${SFTP_PASSWORD}@${SFTP_HOST}:${SFTP_PORT}/ -hostkey=*
cd ${SFTP_CONFIRM_DIR}
ls
exit
EOF
    MSYS_NO_PATHCONV=1 "$WINSCP" /log=NUL /ini=nul /script="$(cygpath -w "$TMP_SCRIPT" | sed 's/^\\//')" > "$TMP_LIST" 2>&1 || true
    rm -f "$TMP_SCRIPT"

    while IFS= read -r line; do
        FNAME=$(echo "$line" | grep -oE "${PROJECT_NAME}_${BRANCH}_Deploy_Success_[^ ]+" | head -1) || true
        [[ -z "$FNAME" ]] && continue
        [[ -n "${SEEN_DEPLOY_FILES[$FNAME]:-}" ]] && continue

        TMP_GET=$(mktemp)
        cat > "$TMP_GET" <<EOF
option batch continue
option confirm off
open sftp://${SFTP_USER}:${SFTP_PASSWORD}@${SFTP_HOST}:${SFTP_PORT}/ -hostkey=*
cd ${SFTP_CONFIRM_DIR}
lcd ${LOCAL_CONFIRM_WIN}
get "${FNAME}"
rm "${FNAME}"
exit
EOF
        MSYS_NO_PATHCONV=1 "$WINSCP" /log=NUL /ini=nul /script="$(cygpath -w "$TMP_GET" | sed 's/^\\//')" >/dev/null 2>&1 || true
        rm -f "$TMP_GET"

        LOCAL_TXT="$LOCAL_CONFIRM_DIR/$FNAME"
        if [[ -f "$LOCAL_TXT" ]]; then
            SEEN_DEPLOY_FILES[$FNAME]=1
            WATCHER_RESULT_FILES+=("$LOCAL_TXT")
            R_SERVER=$(grep -E 'WatcherNode=' "$LOCAL_TXT" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_SERVER="?"
            R_STATUS=$(grep -E 'Status=' "$LOCAL_TXT" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_STATUS="?"
            log ""
            log "   📥 Response from server: $R_SERVER → Status: $R_STATUS"
            cat "$LOCAL_TXT" | while IFS= read -r rline; do log "      | $rline"; done
        fi
    done < "$TMP_LIST"
    rm -f "$TMP_LIST"

    RECEIVED=${#WATCHER_RESULT_FILES[@]}
    if [[ $RECEIVED -ge $SERVER_COUNT ]]; then
        WATCHER_CONFIRMED=true
        break
    fi

    [[ $ELAPSED -eq 30  ]] && log "   ... waiting (${RECEIVED}/${SERVER_COUNT} responded, 30s)"
    [[ $ELAPSED -eq 60  ]] && log "   ... waiting (${RECEIVED}/${SERVER_COUNT} responded, 60s)"
    [[ $ELAPSED -eq 120 ]] && log "   ... waiting (${RECEIVED}/${SERVER_COUNT} responded, 120s)"
    [[ $ELAPSED -eq 300 ]] && log "   ... waiting (${RECEIVED}/${SERVER_COUNT} responded, 300s)"

    sleep $CHECK_INTERVAL
done
set -e

RECEIVED=${#WATCHER_RESULT_FILES[@]}

# ==================================================
# STEP 5: WATCHER CONFIRMED
# ==================================================
log ""
log "==================================================="
if [[ "$WATCHER_CONFIRMED" == true ]]; then
    log "🎉 DEPLOYMENT COMPLETE — $RECEIVED/$SERVER_COUNT servers responded"
else
    log "⚠️  UPLOADED ($RECEIVED/$SERVER_COUNT servers responded within ${MAX_WAIT}s)"
fi
log "==================================================="

DEPLOY_PATH="$RAW_LOCAL"
BACKUP_PATH=""
SMOKE_STATUS=""
SMOKE_DETAIL=""
if [[ ${#WATCHER_RESULT_FILES[@]} -gt 0 ]]; then
    FIRST="${WATCHER_RESULT_FILES[0]}"
    DEPLOY_PATH=$(grep -E 'DeployPath=' "$FIRST" | head -1 | cut -d'=' -f2- | tr -d '\r') || DEPLOY_PATH="$RAW_LOCAL"
    BACKUP_PATH=$(grep -E '(BackupPath|BackupRemoved)=' "$FIRST" | head -1 | cut -d'=' -f2- | tr -d '\r') || BACKUP_PATH=""
    SMOKE_STATUS=$(grep -E 'SmokeTestStatus=' "$FIRST" | head -1 | cut -d'=' -f2- | tr -d '\r') || SMOKE_STATUS=""
    SMOKE_DETAIL=$(grep -E 'SmokeTestDetail=' "$FIRST" | head -1 | cut -d'=' -f2- | tr -d '\r') || SMOKE_DETAIL=""
    [[ -z "$DEPLOY_PATH" ]] && DEPLOY_PATH="$RAW_LOCAL"
fi

# ==================================================
# STEP 6: SMOKE TEST RESULT (read from watcher — tests run server-side)
# ==================================================
# No smoke suite is configured for Beyahad_Backend yet, so this will
# currently always come back "Skipped" — but it now reads that status
# from the watcher (like DCODE does) rather than hardcoding the message,
# so nothing needs to change here once a smoke suite IS added later; the
# watcher config (SmokeTestDir override) is the only thing that'll need
# updating at that point.
log ""
log "==================================================="
log "🧪 SMOKE TEST RESULT (from watcher)"
log "==================================================="

SITE_STATUS="⏭ Not tested — deployment not confirmed by watcher"

if [[ "$WATCHER_CONFIRMED" == true ]]; then
    case "$SMOKE_STATUS" in
        Passed)
            SITE_STATUS="✅ Site is up — smoke tests passed"
            ;;
        Failed)
            SITE_STATUS="❌ Deployment completed but smoke tests FAILED — ${SMOKE_DETAIL:-see watcher log}"
            ;;
        Error)
            SITE_STATUS="⚠ Smoke test execution error — ${SMOKE_DETAIL:-see watcher log}"
            ;;
        Skipped)
            SITE_STATUS="⏭ Not tested — ${SMOKE_DETAIL:-no automated smoke/QA suite configured for this project yet}"
            ;;
        *)
            SITE_STATUS="⏭ Not tested — smoke test status not found in watcher result (older watcher version?)"
            ;;
    esac
    log "   $SITE_STATUS"
else
    log "   ⏭  Skipped — watcher did not confirm the deployment"
fi

# ==================================================
# STEP 7: SEND EMAIL
# ==================================================
GIT_EMAIL=$(git config user.email 2>/dev/null || echo "")
if [[ -z "$GIT_EMAIL" ]]; then
    WIN_USER=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:USERNAME" 2>/dev/null | tr -d '\r\n') || WIN_USER=""
    GIT_EMAIL="${WIN_USER}@swish.co.il"
fi

# These are cosmetic (used only for the email body) — with pipefail enabled,
# a nonzero exit from powershell.exe here (even a transient one) would
# otherwise be treated as a simple-command failure and silently kill the
# whole script under set -e, even though the deploy already succeeded.
# The "|| VAR=fallback" ensures a hiccup here can't do that.
TRIGGERED_BY=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:USERNAME" 2>/dev/null | tr -d '\r\n') || TRIGGERED_BY="unknown"
DEPLOY_TIME=$(date '+%d.%m.%Y %H:%M')
SERVER_NAME=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:COMPUTERNAME" 2>/dev/null | tr -d '\r\n') || SERVER_NAME="unknown"
[[ -z "$TRIGGERED_BY" ]] && TRIGGERED_BY="unknown"
[[ -z "$SERVER_NAME" ]] && SERVER_NAME="unknown"

# If the site verification itself failed (deploy succeeded, smoke tests
# didn't), flip to a warning presentation even though the watcher confirmed
# the file deploy. With no smoke suite configured yet this branch simply
# never triggers today — SITE_STATUS will be "Skipped", not "❌" — but it's
# wired up now so nothing else needs touching once a suite is added.
SITE_VERIFY_FAILED=false
if [[ "$SITE_STATUS" == "❌"* ]]; then
    SITE_VERIFY_FAILED=true
fi

if [[ "$WATCHER_CONFIRMED" == true ]] && [[ "$SITE_VERIFY_FAILED" == false ]]; then
    SUBJECT="✅ DEPLOY PROCESS COMPLETED — $PROJECT ($BRANCH)"
    BADGE_TEXT="♻️ DEPLOY STATUS:SUCCESS"
    HEADER_COLOR="#2e7d32"
    BORDER_COLOR="#4caf50"
    BG_COLOR="#f1f8e9"
    FOOTER_TEXT="✓ Deployment verified by watchdog service"
elif [[ "$WATCHER_CONFIRMED" == true ]] && [[ "$SITE_VERIFY_FAILED" == true ]]; then
    SUBJECT="[WARNING] Deployed but site DOWN — $PROJECT ($BRANCH)"
    BADGE_TEXT="⚠ DEPLOYED — SITE VERIFICATION FAILED"
    HEADER_COLOR="#c62828"
    BORDER_COLOR="#e57373"
    BG_COLOR="#fdecea"
    FOOTER_TEXT="⚠ File deploy confirmed by watchdog, but the site did not pass smoke tests — please check manually"
else
    SUBJECT="[WARNING] Deployment Uploaded — $PROJECT ($BRANCH)"
    BADGE_TEXT="⚠ NO WATCHER RESPONSE"
    HEADER_COLOR="#e65100"
    BORDER_COLOR="#ff9800"
    BG_COLOR="#fff8e1"
    FOOTER_TEXT="⚠ Watcher did not confirm — please verify manually"
fi

# Build HTML in bash — avoids PowerShell here-string/bash quoting conflicts
HTML_FILE="$ROOT/.deploy_email.html"

_row()     { echo "<tr style='border-bottom:1px solid #e0e0e0;'><td style='padding:10px 16px;color:#555;font-size:13px;width:160px;'>$1</td><td style='padding:10px 16px;font-weight:600;font-size:13px;color:#111;'>$2</td></tr>"; }
_monorow() { echo "<tr style='border-bottom:1px solid #e0e0e0;'><td style='padding:10px 16px;color:#555;font-size:13px;width:160px;'>$1</td><td style='padding:10px 16px;font-weight:600;font-size:12px;color:#111;font-family:monospace;'>$2</td></tr>"; }
_html_escape() { echo "$1" | sed 's/&/\&amp;/g; s/</\&lt;/g; s/>/\&gt;/g'; }

BACKUP_ROW=""
[[ -n "$BACKUP_PATH" ]] && BACKUP_ROW=$(_monorow "Backup" "$BACKUP_PATH")

# Only build the "DevSec Vulnerability Details" card when something was
# actually flagged — a clean scan gets just the one-line summary row above,
# matching how a successful run shouldn't need an expanded details block.
DEVSEC_DETAILS_HTML=""
if [[ "$DEVSEC_HAS_ISSUES" == true ]]; then
    NUGET_DETAILS_ESC=""
    [[ -n "$NUGET_SCAN_OUTPUT" ]] && NUGET_DETAILS_ESC=$(_html_escape "$NUGET_SCAN_OUTPUT")
    JS_DETAILS_ESC=""
    [[ -n "$RETIRE_OUTPUT" ]] && JS_DETAILS_ESC=$(_html_escape "$RETIRE_OUTPUT")

    NUGET_BLOCK=""
    if [[ -n "$NUGET_DETAILS_ESC" ]]; then
        NUGET_BLOCK="<div style='margin-bottom:16px;'><div style='font-weight:600;font-size:13px;color:#424242;margin-bottom:6px;'>NuGet packages:</div><pre style='background:#f5f5f5;border:1px solid #ddd;border-radius:6px;padding:14px;font-size:12px;line-height:1.5;overflow-x:auto;white-space:pre-wrap;color:#333;'>${NUGET_DETAILS_ESC}</pre></div>"
    fi

    JS_BLOCK=""
    if [[ -n "$JS_DETAILS_ESC" ]]; then
        JS_BLOCK="<div><div style='font-weight:600;font-size:13px;color:#424242;margin-bottom:6px;'>JS libraries (retire.js):</div><pre style='background:#f5f5f5;border:1px solid #ddd;border-radius:6px;padding:14px;font-size:12px;line-height:1.5;overflow-x:auto;white-space:pre-wrap;color:#333;'>${JS_DETAILS_ESC}</pre></div>"
    fi

    DEVSEC_DETAILS_HTML="
<div style='font-family:Arial,sans-serif;max-width:680px;margin:16px auto 0 auto;background:#fff;border:1.5px solid #e0e0e0;border-radius:8px;padding:20px 24px;'>
  <h3 style='margin:0 0 14px 0;font-size:15px;color:#1a1a1a;'>🔒 DevSec Vulnerability Details</h3>
  ${NUGET_BLOCK}
  ${JS_BLOCK}
</div>"
fi

cat > "$HTML_FILE" << HTMLEOF
<div style="font-family:Arial,sans-serif;max-width:680px;margin:0 auto;">
  <div style="background:${HEADER_COLOR};padding:4px 18px;border-radius:6px 6px 0 0;"></div>
  <div style="background:${BG_COLOR};border:1.5px solid ${BORDER_COLOR};border-top:none;border-radius:0 0 8px 8px;padding:24px 28px;">
    <h2 style="margin:0 0 18px 0;font-size:18px;color:#1a1a1a;">&#9632;&nbsp; ${SUBJECT}</h2>
    <hr style="border:none;border-top:1.5px solid ${BORDER_COLOR};margin-bottom:20px;">
    <div style="margin-bottom:20px;">
      <span style="display:inline-block;background:${HEADER_COLOR};color:#fff;font-weight:700;font-size:13px;padding:8px 18px;border-radius:5px;">${BADGE_TEXT}</span>
    </div>
    <table style="width:100%;border-collapse:collapse;background:#fff;border-radius:8px;overflow:hidden;">
      $(_row "Project"      "${PROJECT}")
      $(_row "Branch"       "${BRANCH}")
      $(_row "Upload Time"  "${DEPLOY_TIME}")
      $(_row "Servers"      "${RECEIVED}/${SERVER_COUNT} responded")
      $(_monorow "Deploy Path" "${DEPLOY_PATH}")
      ${BACKUP_ROW}
      $(_row "Triggered By" "${TRIGGERED_BY}")
      $(_row "Site Status"  "${SITE_STATUS}")
      $(_row "DevSec Scan"  "${DEVSEC_SUMMARY}")
    </table>
    $(
      if [[ ${#WATCHER_RESULT_FILES[@]} -gt 0 ]]; then
        echo "<div style='margin-top:16px;'><table style='width:100%;border-collapse:collapse;background:#fff;border-radius:8px;font-size:12px;'>"
        echo "<tr style='background:#f5f5f5;'><th style='padding:8px;text-align:left;'>Server</th><th style='padding:8px;text-align:left;'>Status</th><th style='padding:8px;text-align:left;'>IIS</th><th style='padding:8px;text-align:left;'>Pool</th></tr>"
        for TXT_FILE in "${WATCHER_RESULT_FILES[@]}"; do
          R_SERVER=$(grep -E 'WatcherNode=' "$TXT_FILE" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_SERVER="?"
          R_STATUS=$(grep -E 'Status=' "$TXT_FILE" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_STATUS="?"
          R_IIS=$(grep -E 'IISRestart=' "$TXT_FILE" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_IIS="?"
          R_POOL=$(grep -E 'Pool=' "$TXT_FILE" | head -1 | cut -d'=' -f2- | tr -d '\r') || R_POOL="?"
          R_COLOR="#2e7d32"; [[ "$R_STATUS" != "Success" ]] && R_COLOR="#c62828"
          echo "<tr><td style='padding:8px;'>$R_SERVER</td><td style='padding:8px;color:$R_COLOR;font-weight:bold;'>$R_STATUS</td><td style='padding:8px;'>$R_IIS</td><td style='padding:8px;'>$R_POOL</td></tr>"
        done
        echo "</table></div>"
      fi
    )
    <div style="margin-top:20px;padding-top:14px;border-top:1px solid #ddd;font-size:12px;color:#555;">
      <div>${FOOTER_TEXT}</div>
      <div style="margin-top:2px;color:#888;">Detailed log attached for your records.</div>
    </div>
  </div>
</div>
${DEVSEC_DETAILS_HTML}
HTMLEOF

# Convert bash path to Windows path for PowerShell
RAW_HTML_FILE=$(cygpath -w "$HTML_FILE" 2>/dev/null || echo "$HTML_FILE" | sed -E 's|^/([a-zA-Z])/|\1:/|')

"$POWERSHELL" -NoProfile -Command "
    try {
        \$html = Get-Content '$RAW_HTML_FILE' -Raw -Encoding UTF8
        \$outlook = New-Object -ComObject Outlook.Application
        \$mail = \$outlook.CreateItem(0)
        \$mail.To      = '$GIT_EMAIL'
        \$mail.Subject = '$SUBJECT'
        \$mail.HTMLBody = \$html
        if (Test-Path '$LOG_FILE') {
            \$null = \$mail.Attachments.Add('$LOG_FILE')
        }
        \$mail.Send()
        Write-Output 'SENT'
    } catch {
        Write-Output \"FAILED: \$(\$_.Exception.Message)\"
    }
" 2>&1

rm -f "$HTML_FILE"

# ==================================================
# STEP 8: DEPLOYMENT COMPLETE
# ==================================================
log ""
log "✅ Done!"
log ""

log ""
log "==================================================="
log "🏁 PIPELINE FINISHED"
log "==================================================="

exit 0