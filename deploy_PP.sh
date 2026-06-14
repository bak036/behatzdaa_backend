#!/usr/bin/env bash
# ===========================================================
# deploy_PP.sh - DEPLOYMENT SCRIPT - business editor
# With automatic prerequisite installation
# ===========================================================

set -e
set +u
if (set -o pipefail >/dev/null 2>&1); then set -o pipefail; fi

POWERSHELL="/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe"

# Check if PowerShell is available
if [ ! -f "$POWERSHELL" ]; then
    echo "❌ ERROR: PowerShell not found at $POWERSHELL"
    echo "   This script requires Windows PowerShell to run."
    echo "   Please ensure you're running on Windows with PowerShell installed."
    exit 1
fi

ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)
cd "$ROOT"

PROJECT_NAME=$(basename "$ROOT")
BRANCH=$(git rev-parse --abbrev-ref HEAD)
LOG_FILE="$ROOT/automation_master_log.txt"

# Logging helper
log() {
    local msg="$*"
    local timestamp="[ $(date '+%Y-%m-%d %H:%M:%S') ]"
    echo "$timestamp $msg" | tee -a "$LOG_FILE"
}

# Ensure log file exists
LOG_DIR=$(dirname "$LOG_FILE")
mkdir -p "$LOG_DIR"
if [ ! -f "$LOG_FILE" ]; then
    echo "===================================================" > "$LOG_FILE"
    echo "[ $(date '+%Y-%m-%d %H:%M:%S') ] Log initialized" >> "$LOG_FILE"
    echo "===================================================" >> "$LOG_FILE"
fi

log "🔍 Detected project: $PROJECT_NAME"
log "🔍 Detected branch: $BRANCH"
log ""
log "==================================================="
log "🔧 CHECKING & INSTALLING DEPENDENCIES"
log "==================================================="

# ============================================================
# STEP 1: Check and install PowerShell prerequisites
# ============================================================
log "📦 Checking PowerShell prerequisites..."

PREREQ_RESULT=$("$POWERSHELL" -NoProfile -Command '
$ErrorActionPreference = "Stop"
$results = @()

try {
    # Check NuGet Package Provider
    Write-Output "   🔍 Checking NuGet Package Provider..."
    $nuget = Get-PackageProvider -Name NuGet -ErrorAction SilentlyContinue -ListAvailable | 
             Where-Object { $_.Version -ge "2.8.5.201" }
    
    if ($null -eq $nuget) {
        Write-Output "   ⚙  Installing NuGet Package Provider..."
        Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force -Scope CurrentUser | Out-Null
        Write-Output "   ✓ NuGet Package Provider installed"
        $results += "NUGET_INSTALLED"
    } else {
        Write-Output "   ✓ NuGet Package Provider already installed (v$($nuget.Version))"
        $results += "NUGET_EXISTS"
    }
    
    # Check PSGallery Trust Policy
    Write-Output "   🔍 Checking PSGallery installation policy..."
    $gallery = Get-PSRepository -Name PSGallery -ErrorAction SilentlyContinue
    
    if ($null -eq $gallery) {
        Write-Output "   ⚠️  PSGallery repository not found, registering..."
        Register-PSRepository -Default -ErrorAction SilentlyContinue | Out-Null
        $gallery = Get-PSRepository -Name PSGallery -ErrorAction SilentlyContinue
    }
    
    if ($gallery.InstallationPolicy -ne "Trusted") {
        Write-Output "   ⚙  Setting PSGallery as trusted repository..."
        Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
        Write-Output "   ✓ PSGallery set as trusted"
        $results += "PSGALLERY_CONFIGURED"
    } else {
        Write-Output "   ✓ PSGallery already trusted"
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
log "📦 Checking jq..."
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
log "📦 Checking SQL Server PowerShell module..."

SQL_MODULE_RESULT=$("$POWERSHELL" -NoProfile -Command '
$ErrorActionPreference = "Stop"

try {
    # Check if SqlServer module is available
    $module = Get-Module -ListAvailable -Name SqlServer | Select-Object -First 1
    
    if ($null -eq $module) {
        Write-Output "   ⚙  SqlServer module not found. Installing for current user..."
        Write-Output "   ⏳ This may take 1-2 minutes on first run..."
        
        # Install the module
        Install-Module -Name SqlServer -Scope CurrentUser -Force -AllowClobber -SkipPublisherCheck -ErrorAction Stop | Out-Null
        
        Write-Output "   ✓ SqlServer module installed successfully"
        $result = "INSTALLED"
    } else {
        Write-Output "   ✓ SqlServer module already installed (v$($module.Version))"
        $result = "EXISTS"
    }
    
    # Try to import the module
    Write-Output "   🔄 Importing SqlServer module..."
    Import-Module SqlServer -ErrorAction Stop | Out-Null
    Write-Output "   ✓ SqlServer module imported successfully"
    
    # Test Invoke-Sqlcmd availability
    if (Get-Command Invoke-Sqlcmd -ErrorAction SilentlyContinue) {
        Write-Output "   ✓ Invoke-Sqlcmd command is available"
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
log "📦 Checking Robocopy..."
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

log "==================================================="
log "✓ All dependencies ready"
log "==================================================="
log ""

# ============================================================
# STEP 5: Load config from DB
# ============================================================
log "🔗 Connecting to database..."
log "   Server: 172.29.92.20\sql2005"
log "   Database: NofTest"

db_values=$("$POWERSHELL" -NoProfile -Command '
  $ErrorActionPreference = "Stop"
  try {
    # Ensure SqlServer module is imported
    Import-Module SqlServer -ErrorAction Stop | Out-Null
    
    $connectionString = "Server=172.29.92.20\sql2005;Database=NofTest;User ID=alexk;Password=Dtsal21xk;TrustServerCertificate=True;";
    $query = "SELECT TOP 1 * FROM ProjectDepJoyConfig WHERE Project='"'$PROJECT_NAME'"' AND Branch='"'$BRANCH'"' ORDER BY ID DESC";
    
    $result = Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop;
    
    if ($null -eq $result) {
      Write-Output "NO_RESULTS"
    } else {
      foreach ($row in $result) {
        $remote = $row.RemoteServer
        $local  = $row.LocalPublishDir
        $zip    = $row.ZipPath
        $line = ($row.Project + "|||DELIM|||" + $row.Branch + "|||DELIM|||" + $remote + "|||DELIM|||" + $row.RemoteUser + "|||DELIM|||" + $row.RemotePassword + "|||DELIM|||" + $local + "|||DELIM|||" + $zip + "|||DELIM|||" + $row.CreateAt)
        Write-Output $line
      }
    }
  } catch {
    Write-Output ("ERROR: " + $_.Exception.Message)
  }
')

if [[ "$db_values" == ERROR* ]]; then
    log "❌ ERROR: Failed to connect to database"
    log "   $db_values"
    log ""
    log "   Possible causes:"
    log "   • Network connectivity issues"
    log "   • SQL Server not accessible"
    log "   • SqlServer module not properly installed"
    log "   • Invalid credentials"
    exit 1
elif [[ "$db_values" == "NO_RESULTS" || -z "$db_values" ]]; then
    log "❌ ERROR: No deployment configuration found in database"
    log "   Project: $PROJECT_NAME"
    log "   Branch: $BRANCH"
    log ""
    log "   Please ensure the configuration exists in ProjectDepJoyConfig table"
    exit 1
else
    log "   ✓ Deployment config loaded from database"
    PROJECT=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $1}')
    BRANCH=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $2}')
    REMOTE_SERVER=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $3}')
    REMOTE_USER=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $4}')
    REMOTE_PASSWORD=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $5}')
    LOCAL_PUBLISH_DIR=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $6}')
    ZIP_PATH=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $7}')
    CREATE_AT=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $8}')
fi

RAW_REMOTE="$REMOTE_SERVER"
RAW_LOCAL="$LOCAL_PUBLISH_DIR"
RAW_ZIP="$ZIP_PATH"

# Normalize paths for Bash/Git Bash
normalize_path() {
    local winpath="$1"
    local path="${winpath//\\//}"
    if [[ "$MSYSTEM" == "MINGW64" || "$MSYSTEM" == "MINGW32" ]]; then
        path=$(echo "$path" | sed -E 's#^([A-Za-z]):#/\L\1/#')
    else
        path=$(echo "$path" | sed -E 's#^([A-Za-z]):#\1:/#')
    fi
    echo "$path"
}

REMOTE_SERVER=$(normalize_path "$REMOTE_SERVER")
LOCAL_PUBLISH_DIR=$(normalize_path "$LOCAL_PUBLISH_DIR")
ZIP_PATH=$(normalize_path "$ZIP_PATH")

log "DEBUG: Raw Remote='$RAW_REMOTE' → Normalized='$REMOTE_SERVER'"
log "DEBUG: Raw Local='$RAW_LOCAL' → Normalized='$LOCAL_PUBLISH_DIR'"
log "DEBUG: Raw Zip='$RAW_ZIP' → Normalized='$ZIP_PATH'"

if ! ls "$LOCAL_PUBLISH_DIR" >/dev/null 2>&1; then
    log "❌ ERROR: Cannot access publish directory → $LOCAL_PUBLISH_DIR"
    exit 1
fi
log "   ✓ Publish directory accessible"

CONFIRM_DIR="//PP-SERVER/CiCd/RESULTS"

log "==================================================="
log "🚀 DEPLOYMENT STARTED"
log "==================================================="
log "Project: $PROJECT"
log "Branch:  $BRANCH"
log "==================================================="

# STEP 1: CREATE ZIP
log "📦 [1/3] Creating deployment package..."

TEMP="$ROOT/.deploy_temp"
log "   ↳ Cleaning temp directory $TEMP"

# Remove everything EXCEPT /dev folder
if [ -d "$TEMP" ]; then
    for item in "$TEMP"/*; do
        case "$item" in
            */dev) log "   ⚠️ Skipping system mount $item" ;;
            *) rm -rf "$item" 2>/dev/null || true ;;
        esac
    done
fi
mkdir -p "$TEMP"

log "   ↳ Copying files from $LOCAL_PUBLISH_DIR to $TEMP"

# USE ROBOCOPY TO AVOID /dev ISSUES
COPY_RESULT=$("$POWERSHELL" -NoProfile -Command "
try {
    # Use robocopy to copy files, excluding dev folder
    \$source = '$RAW_LOCAL'
    \$dest = '$TEMP'
    
    # Robocopy with exclusions
    \$robocopyArgs = @(
        \$source,
        \$dest,
        '/MIR',
        '/R:0',
        '/W:0',
        '/XD', 'dev',  # Exclude dev folder
        '/XD', '.git', # Exclude git folder
        '/NFL',        # No file list
        '/NDL',        # No directory list
        '/NJH',        # No job header
        '/NJS'         # No job summary
    )
    
    \$result = & robocopy @robocopyArgs
    
    # Robocopy exit codes: 0-7 are success, 8+ are errors
    if (\$LASTEXITCODE -lt 8) {
        Write-Output 'SUCCESS'
    } else {
        throw \"Robocopy failed with exit code \$LASTEXITCODE\"
    }
} catch {
    Write-Output \"FAILED: \$(\$_.Exception.Message)\"
    exit 1
}
" 2>&1)

if [[ "$COPY_RESULT" != *"SUCCESS"* ]]; then
    log "❌ ERROR: Failed to copy files"
    log "   $COPY_RESULT"
    exit 1
fi

log "   ✓ Files copied successfully"

# STEP 1a: Add PP config files to package
# Look in the directory where the script is located (repository root)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
CONFIG_DIR="$SCRIPT_DIR"

log "📋 Adding environment-specific configs to package..."
log "   Script directory: $SCRIPT_DIR"
log "   Looking for configs in: $CONFIG_DIR"

WEB_PP_CONFIG="$CONFIG_DIR/web_PP.config"
log "   Checking for: $WEB_PP_CONFIG"
log "   File exists: $([ -f "$WEB_PP_CONFIG" ] && echo 'YES' || echo 'NO')"

if [ -f "$WEB_PP_CONFIG" ]; then
    # Copy as web_PP.config AND overwrite web.config with PP version
    cp "$WEB_PP_CONFIG" "$TEMP/web_PP.config"
    cp "$WEB_PP_CONFIG" "$TEMP/web.config"
    log "   ✓ web_PP.config added to package"
    log "   ✓ web.config overwritten with PP version"
    log "   ↳ Size: $(wc -c < "$TEMP/web_PP.config") bytes"
else
    log "   ⚠️  web_PP.config not found at $WEB_PP_CONFIG"
fi

APPSETTINGS_PP="$CONFIG_DIR/appsettings_PP.json"
log "   Checking for: $APPSETTINGS_PP"
log "   File exists: $([ -f "$APPSETTINGS_PP" ] && echo 'YES' || echo 'NO')"

if [ -f "$APPSETTINGS_PP" ]; then
    # Copy as appsettings_PP.json AND overwrite appsettings.json with PP version
    cp "$APPSETTINGS_PP" "$TEMP/appsettings_PP.json"
    cp "$APPSETTINGS_PP" "$TEMP/appsettings.json"
    log "   ✓ appsettings_PP.json added to package"
    log "   ✓ appsettings.json overwritten with PP version"
    log "   ↳ Size: $(wc -c < "$TEMP/appsettings_PP.json") bytes"
else
    log "   ⚠️  appsettings_PP.json not found at $APPSETTINGS_PP"
fi

log ""
log "📦 Package contains:"
[ -f "$TEMP/web.config" ] && log "   ✓ web.config (PP version)"
[ -f "$TEMP/web_PP.config" ] && log "   ✓ web_PP.config (backup)"
[ -f "$TEMP/appsettings.json" ] && log "   ✓ appsettings.json (PP version)"
[ -f "$TEMP/appsettings_PP.json" ] && log "   ✓ appsettings_PP.json (backup)"
log ""

# STEP 2: Create ZIP archive
log "   ↳ Creating ZIP archive at $RAW_ZIP"
"$POWERSHELL" -NoProfile -Command "
Add-Type -AssemblyName System.IO.Compression.FileSystem
if (Test-Path '$RAW_ZIP') { Remove-Item '$RAW_ZIP' -Force }
[System.IO.Compression.ZipFile]::CreateFromDirectory('$TEMP', '$RAW_ZIP', 'Optimal', \$false)
" || { log "❌ ERROR: ZIP creation failed"; exit 1; }

if [ ! -f "$RAW_ZIP" ]; then
    log "❌ ERROR: Failed to create ZIP → $RAW_ZIP"
    exit 1
fi

log "   ✓ ZIP created successfully"

# STEP 2: COPY TO SERVER
log "📤 [2/3] Uploading to network share..."

COPY_RESULT=$("$POWERSHELL" -NoProfile -Command "
try {
    try { net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null } catch {}
    
    \$netUseResult = net use '$RAW_REMOTE' /user:'$REMOTE_USER' '$REMOTE_PASSWORD' /persistent:no 2>&1
    if (\$LASTEXITCODE -ne 0) { throw \"Network connection failed\" }
    
    if (-not (Test-Path '$RAW_ZIP')) { throw 'Source ZIP not found' }
    
    \$destFile = \"$RAW_REMOTE\\${PROJECT}_${BRANCH}_publish.zip\"
    Copy-Item -Path '$RAW_ZIP' -Destination \$destFile -Force -ErrorAction Stop
    
    net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null
    Write-Output 'SUCCESS'
} catch {
    Write-Output \"FAILED: \$(\$_.Exception.Message)\"
    try { net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null } catch {}
    exit 1
}
" 2>&1)

if [[ "$COPY_RESULT" != *"SUCCESS"* ]]; then
    log "❌ ERROR: Upload failed"
    exit 1
fi

log "   ✓ ZIP uploaded successfully"

# STEP 3: WAIT FOR WATCHER
log "⏳ [3/3] Waiting for watcher confirmation..."

WATCHER_CONFIRMED=false
MAX_WAIT=120
CHECK_INTERVAL=2
DEPLOY_START=$(date +%s)

# Initialize variables for deployment details
DEPLOY_ROOT=""
BACKUP_TAKEN=""
IIS_RESTART=""
SERVER_NODE=""
CONFIRMATION_FILE=""

for ((i=1; i<=MAX_WAIT/CHECK_INTERVAL; i++)); do
    ELAPSED=$((i * CHECK_INTERVAL))
    
    RESULT=$("$POWERSHELL" -NoProfile -Command "
        \$deployStart = (Get-Date '1970-01-01 00:00:00Z').AddSeconds($DEPLOY_START)
        if (Test-Path '$CONFIRM_DIR') {
            \$file = Get-ChildItem '$CONFIRM_DIR' -Filter '*${PROJECT}*${BRANCH}*Deploy*Success*.txt' -ErrorAction SilentlyContinue |
                Where-Object { \$_.LastWriteTime -gt \$deployStart } |
                Select-Object -First 1
            if (\$file) { Write-Output \$file.FullName }
        }
    " 2>/dev/null | tr -d '\r\n' | sed 's/^[[:space:]]*//;s/[[:space:]]*$//')
    
    if [[ -n "$RESULT" ]] && [[ "$RESULT" != "null" ]]; then
        WATCHER_CONFIRMED=true
        CONFIRMATION_FILE="$RESULT"
        log "   ✓ Watcher confirmed after ${ELAPSED}s!"
        log "   📄 Confirmation file: $CONFIRMATION_FILE"
        
        # ⭐ READ THE CONFIRMATION FILE TO EXTRACT DEPLOYMENT DETAILS
        log "   📋 Reading deployment details from watcher..."
        
        # Convert the path back to Windows format for PowerShell
        CONFIRMATION_FILE_WIN=$(echo "$CONFIRMATION_FILE" | sed 's#/#\\#g')
        
        WATCHER_DATA=$("$POWERSHELL" -NoProfile -Command "
            try {
                if (Test-Path '$CONFIRMATION_FILE_WIN') {
                    Get-Content '$CONFIRMATION_FILE_WIN' -Raw -ErrorAction Stop
                } else {
                    Write-Output 'FILE_NOT_FOUND'
                }
            } catch {
                Write-Output \"ERROR: \$(\$_.Exception.Message)\"
            }
        " 2>&1)
        
        if [[ "$WATCHER_DATA" == *"FILE_NOT_FOUND"* ]]; then
            log "   ⚠️  Warning: Confirmation file not accessible"
            DEPLOY_ROOT="Confirmed (file not accessible)"
            BACKUP_TAKEN="Confirmed (file not accessible)"
            IIS_RESTART="Unknown"
            SERVER_NODE="PP-SERVER"
        elif [[ "$WATCHER_DATA" == *"ERROR:"* ]]; then
            log "   ⚠️  Warning: Error reading file: $WATCHER_DATA"
            DEPLOY_ROOT="Confirmed (read error)"
            BACKUP_TAKEN="Confirmed (read error)"
            IIS_RESTART="Unknown"
            SERVER_NODE="PP-SERVER"
        elif [[ -n "$WATCHER_DATA" ]] && [[ "$WATCHER_DATA" != "null" ]]; then
            # Parse Key=Value format from the confirmation file
            # Handle Windows line endings by using sed to normalize first
            NORMALIZED_DATA=$(echo "$WATCHER_DATA" | sed 's/\r$//')
            
            # Use || true to prevent set -e from exiting on no match
            # Priority: DeployRoot first, then Site as fallback
            DEPLOY_ROOT=$(echo "$NORMALIZED_DATA" | grep -E '^DeployRoot=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            if [[ -z "$DEPLOY_ROOT" ]]; then
                DEPLOY_ROOT=$(echo "$NORMALIZED_DATA" | grep -E '^Site=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            fi
            
            BACKUP_TAKEN=$(echo "$NORMALIZED_DATA" | grep -E '^BackupTaken=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            
            IIS_RESTART=$(echo "$NORMALIZED_DATA" | grep -E '^IISRestart=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            
            # Priority: WatcherNode first, then Pool as fallback
            SERVER_NODE=$(echo "$NORMALIZED_DATA" | grep -E '^WatcherNode=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            if [[ -z "$SERVER_NODE" ]]; then
                SERVER_NODE=$(echo "$NORMALIZED_DATA" | grep -E '^Pool=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            fi
            if [[ -z "$SERVER_NODE" ]]; then
                SERVER_NODE=$(echo "$NORMALIZED_DATA" | grep -E '^Server=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
            fi
            
            # Set defaults if not found
            [[ -z "$DEPLOY_ROOT" ]] && DEPLOY_ROOT="Path not in confirmation"
            [[ -z "$BACKUP_TAKEN" ]] && BACKUP_TAKEN="Backup path not in confirmation"
            [[ -z "$IIS_RESTART" ]] && IIS_RESTART="Not specified"
            [[ -z "$SERVER_NODE" ]] && SERVER_NODE="PP-SERVER"
            
            log "   ✓ Deployment details retrieved:"
            log "      📁 Deploy Path: $DEPLOY_ROOT"
            log "      💾 Backup: $BACKUP_TAKEN"
            log "      🔄 IIS Restart: $IIS_RESTART"
            log "      🖥️  Server: $SERVER_NODE"
        else
            log "   ⚠️  Warning: Confirmation file is empty"
            DEPLOY_ROOT="Confirmed (details not available)"
            BACKUP_TAKEN="Confirmed (details not available)"
            IIS_RESTART="Unknown"
            SERVER_NODE="PP-SERVER"
        fi
        
        break
    fi
    
    [[ $ELAPSED -eq 30 ]] && log "   ... still waiting (30s)"
    [[ $ELAPSED -eq 60 ]] && log "   ... still waiting (60s)"
    
    sleep $CHECK_INTERVAL
done

log ""
log "==================================================="
if [[ "$WATCHER_CONFIRMED" == true ]]; then
    log "🎉 DEPLOYMENT COMPLETE - WATCHER CONFIRMED"
else
    log "⚠️  WATCHER DID NOT RESPOND"
fi
log "==================================================="

# ----------------------------------------------------
# SEND EMAIL
# SEND EMAIL
# ----------------------------------------------------
# Try to get username from Git config first (who cloned the repo)
GIT_USERNAME=$(git config user.name 2>/dev/null || echo "")
GIT_EMAIL=$(git config user.email 2>/dev/null || echo "")

# If Git username not found, try to extract from path
if [[ -z "$GIT_USERNAME" ]]; then
    GIT_USERNAME=$(echo "$ROOT" | sed -E 's#.*Users/([^/]+).*#\1#')
fi

# If Git email not found, try Windows username
if [[ -z "$GIT_EMAIL" ]]; then
    WIN_USERNAME=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:USERNAME" 2>/dev/null | tr -d '\r\n')
    if [[ -n "$WIN_USERNAME" ]]; then
        GIT_USERNAME="$WIN_USERNAME"
        GIT_EMAIL="${WIN_USERNAME}@swish.co.il"
    else
        GIT_EMAIL="${GIT_USERNAME}@swish.co.il"
    fi
fi

EMAIL="$GIT_EMAIL"
TRIGGERED_BY="$GIT_USERNAME"

log "   👤 Detected user: $TRIGGERED_BY"
log "   📧 Email address: $EMAIL"

log ""
log "📧 Sending notification email to: $EMAIL"

# Set email properties based on watcher confirmation
if [[ "$WATCHER_CONFIRMED" == true ]]; then
    EMAIL_SUBJECT="🟩 Deployment Completed — $PROJECT ($BRANCH)"
    EMAIL_BADGE="✓ CONFIRMED BY WATCHER"
    EMAIL_BGCOLOR="#e8f5e9"
    EMAIL_COLOR="#4caf50"
    EMAIL_FOOTER="<strong>✓ Deployment verified by watchdog service</strong><br>Email sent only after successful watchdog confirmation.<br>Detailed log attached for your records."
else
    EMAIL_SUBJECT="🟨 Deployment Uploaded — $PROJECT ($BRANCH)"
    EMAIL_BADGE="⚠ WATCHDOG DID NOT RESPOND"
    EMAIL_BGCOLOR="#fff3e0"
    EMAIL_COLOR="#ff9800"
    EMAIL_FOOTER="<strong>⚠ Note: Watchdog service did not confirm deployment</strong><br>Package was uploaded successfully, but watchdog confirmation not received within 2 minutes.<br>Please check watchdog service logs and verify deployment manually."
    # Set defaults for email when watcher didn't respond
    DEPLOY_ROOT="Not confirmed by watchdog"
    BACKUP_TAKEN="Not confirmed by watchdog"
    IIS_RESTART="Unknown"
    SERVER_NODE="PP-SERVER"
fi

# Escape special characters for PowerShell
DEPLOY_ROOT_ESC=$(echo "$DEPLOY_ROOT" | sed "s/'/''/g")
BACKUP_TAKEN_ESC=$(echo "$BACKUP_TAKEN" | sed "s/'/''/g")
IIS_RESTART_ESC=$(echo "$IIS_RESTART" | sed "s/'/''/g")
SERVER_NODE_ESC=$(echo "$SERVER_NODE" | sed "s/'/''/g")

log "📝 DEBUG: Email variables before sending:"
log "   DEPLOY_ROOT='$DEPLOY_ROOT'"
log "   BACKUP_TAKEN='$BACKUP_TAKEN'"
log "   IIS_RESTART='$IIS_RESTART'"
log "   SERVER_NODE='$SERVER_NODE'"

# Create email using PowerShell
EMAIL_RESULT=$("$POWERSHELL" -NoProfile -Command "
try {
    \$outlook = New-Object -ComObject Outlook.Application
    \$mail = \$outlook.CreateItem(0)
    
    \$mail.To = '$EMAIL'
    \$mail.Subject = '$EMAIL_SUBJECT'
    
    \$timestamp = Get-Date -Format 'dd.MM.yyyy HH:mm'
    
    # Use the extracted values from watcher confirmation
    \$deployRoot   = '$DEPLOY_ROOT_ESC'
    \$backupTaken  = '$BACKUP_TAKEN_ESC'
    \$iisRestart   = '$IIS_RESTART_ESC'
    \$serverNode   = '$SERVER_NODE_ESC'
    \$triggeredBy  = '$TRIGGERED_BY'
    
    \$htmlBody = @\"
<html>
<head>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; }
        .container { 
            background: $EMAIL_BGCOLOR; 
            padding: 30px; 
            border-radius: 12px; 
            max-width: 650px;
            border: 3px solid $EMAIL_COLOR;
            direction:ltr; text-align:left;
        }
        .header { 
            color: #424242; 
            margin-top: 0; 
            border-bottom: 3px solid $EMAIL_COLOR; 
            padding-bottom: 12px; 
        }
        .badge { 
            background: $EMAIL_COLOR; 
            color: white; 
            padding: 10px 20px; 
            border-radius: 8px; 
            display: inline-block; 
            font-weight: bold;
            margin: 15px 0;
            font-size: 16px;
        }
        table { 
            width: 100%; 
            border-collapse: collapse; 
            margin-top: 25px; 
            background: white; 
            border-radius: 8px; 
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        td { 
            padding: 14px 18px; 
            border-bottom: 1px solid #e0e0e0; 
        }
        td:first-child { 
            font-weight: 600; 
            color: #424242; 
            width: 38%; 
            background: #f5f5f5; 
        }
        tr:last-child td { 
            border-bottom: none; 
        }
        .footer { 
            margin-top: 25px; 
            padding-top: 20px; 
            border-top: 2px solid $EMAIL_COLOR; 
            color: #555; 
            font-size: 13px; 
        }
    </style>
</head>
<body>
<div class='container'>
    <h2 class='header'>$EMAIL_SUBJECT</h2>
    <div class='badge'>$EMAIL_BADGE</div>
    
    <table>
        <tr><td>Project</td><td><strong>$PROJECT</strong></td></tr>
        <tr><td>Branch</td><td><strong>$BRANCH</strong></td></tr>
        <tr><td>Upload Time</td><td>\$timestamp</td></tr>
        <tr><td>Server</td><td>\$serverNode</td></tr>
        <tr><td>Deploy Path</td><td>\$deployRoot</td></tr>
        <tr><td>Backup</td><td>\$backupTaken</td></tr>
        <tr><td>Triggered By</td><td>\$triggeredBy</td></tr>
        <tr><td>IIS Restart</td><td>\$iisRestart</td></tr>
    </table>
    
    <div class='footer'>
        $EMAIL_FOOTER
    </div>
</div>
</body>
</html>
\"@
    
    \$mail.HTMLBody = \$htmlBody
    
    if (Test-Path '$LOG_FILE') {
        \$mail.Attachments.Add('$LOG_FILE') | Out-Null
    }
    
    \$mail.Send()
    Write-Output 'EMAIL_SENT'
    
} catch {
    Write-Output \"EMAIL_FAILED: \$(\$_.Exception.Message)\"
}
" 2>&1)

if [[ "$EMAIL_RESULT" == *"EMAIL_SENT"* ]]; then
    log "   ✓ Email sent successfully!"
else
    log "   ⚠️  Email issue: $EMAIL_RESULT"
fi

log ""
log "==================================================="
if [[ "$WATCHER_CONFIRMED" == true ]]; then
    log "🎉 DEPLOYMENT COMPLETE - WATCHDOG CONFIRMED"
    log "   📁 Deployed to: $DEPLOY_ROOT"
    log "   💾 Backup: $BACKUP_TAKEN"
    log "   🔄 IIS Restart: $IIS_RESTART"
else
    log "⚠️  DEPLOYMENT UPLOADED - WATCHDOG DID NOT RESPOND"
    log "   Package uploaded to: $RAW_REMOTE"
    log "   Please verify watchdog service is processing files"
fi
log "==================================================="
log ""

exit 0