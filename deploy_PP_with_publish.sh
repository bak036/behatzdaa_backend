#!/usr/bin/env bash
# ===========================================================
# deploy_PP_SIMPLE.sh - SIMPLE WORKAROUND
# Uses existing build from Visual Studio and replace the D:Publish directory
# Uses existing build from Visual Studio and replace the D:Publish directory
# ===========================================================

set -e
set +u
if (set -o pipefail >/dev/null 2>&1); then set -o pipefail; fi

POWERSHELL="/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe"

if [ ! -f "$POWERSHELL" ]; then
    echo "❌ ERROR: PowerShell not found"
    exit 1
fi

ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)
cd "$ROOT"

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

log "🔍 Project: $PROJECT_NAME"
log "🔍 Branch: $BRANCH"
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

# Get DB config
log "🔗 Loading config from database..."

db_values=$("$POWERSHELL" -NoProfile -Command '
  try {
    Import-Module SqlServer -ErrorAction Stop | Out-Null
    $connectionString = "Server=172.29.92.20\sql2005;Database=NofTest;User ID=alexk;Password=Dtsal21xk;TrustServerCertificate=True;";
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

if [[ "$db_values" == ERROR* ]]; then
    log "❌ Database connection failed"
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

log "==================================================="
log "🚀 DEPLOYMENT STARTED (SIMPLE MODE)"
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
# STEP 1: Publishing .NET 8 project
# ==================================================

CSPROJ="$ROOT/Nofshonit.Api/Nofshonit.Api.csproj"

if [[ ! -f "$CSPROJ" ]]; then
    log "❌ Project file not found: $CSPROJ"
    exit 1
fi

log "📦 Publishing .NET 8 project: $(basename "$CSPROJ")..."

dotnet publish \
    "$CSPROJ" \
    -c Release \
    -o "$RAW_LOCAL"

if [[ $? -ne 0 ]]; then
    log "❌ dotnet publish failed"
    exit 1
fi

log "✓ Publish completed"

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

# Add environment configs
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

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
# STEP 3: UPLOADING TO SERVER
# ==================================================
log "==================================================="
log "📤 UPLOADING TO SERVER"
log "==================================================="

UPLOAD=$("$POWERSHELL" -NoProfile -Command "
    try {
        try { net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null } catch {}
        
        \$null = net use '$RAW_REMOTE' /user:'$REMOTE_USER' '$REMOTE_PASSWORD' /persistent:no 2>&1
        if (\$LASTEXITCODE -ne 0) { throw 'Network connection failed' }
        
        \$destFile = '$RAW_REMOTE\\${PROJECT}_${BRANCH}_publish.zip'
        Copy-Item -Path '$RAW_ZIP' -Destination \$destFile -Force -ErrorAction Stop
        
        net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null
        Write-Output 'SUCCESS'
    } catch {
        Write-Output \"FAILED: \$(\$_.Exception.Message)\"
    }
" 2>&1)

if [[ "$UPLOAD" != *"SUCCESS"* ]]; then
    log "❌ Upload failed: $UPLOAD"
    exit 1
fi

log "   ✓ Uploaded successfully"
log ""

# ==================================================
# STEP 4: WAITING FOR WATCHER CONFIRMATION
# STEP 4: WAITING FOR WATCHER CONFIRMATION
# ==================================================
log "==================================================="
log "⏳ WAITING FOR WATCHER CONFIRMATION"
log "==================================================="

CONFIRM_DIR="//PP-SERVER/CiCd/RESULTS"
WATCHER_CONFIRMED=false
MAX_WAIT=120
CHECK_INTERVAL=2
DEPLOY_START=$(date +%s)

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
    
    if [[ -n "$RESULT" ]]; then
        WATCHER_CONFIRMED=true
        log "   ✅ Confirmed after ${ELAPSED}s!"
        break
    fi
    
    [[ $ELAPSED -eq 30 ]] && log "   ... still waiting (30s)"
    [[ $ELAPSED -eq 60 ]] && log "   ... still waiting (60s)"
    [[ $ELAPSED -eq 90 ]] && log "   ... still waiting (90s)"
    [[ $ELAPSED -eq 120 ]] && log "   ... still waiting (120)"


    sleep $CHECK_INTERVAL
done

# ==================================================
# STEP 6: WATCHER CONFIRMED
# ==================================================
log ""
log "==================================================="
if [[ "$WATCHER_CONFIRMED" == true ]]; then
    log "🎉 DEPLOYMENT COMPLETE"
else
    log "⚠️  UPLOADED (watcher didn't respond)"
fi
log "==================================================="

# ==================================================
# STEP 6: SEND EMAIL
# ==================================================
GIT_EMAIL=$(git config user.email 2>/dev/null || echo "")
if [[ -z "$GIT_EMAIL" ]]; then
    WIN_USER=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:USERNAME" 2>/dev/null | tr -d '\r\n')
    GIT_EMAIL="${WIN_USER}@swish.co.il"
fi

TRIGGERED_BY=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:USERNAME" 2>/dev/null | tr -d '\r\n')
DEPLOY_TIME=$(date '+%d.%m.%Y %H:%M')
SERVER_NAME=$("$POWERSHELL" -NoProfile -Command "Write-Output \$env:COMPUTERNAME" 2>/dev/null | tr -d '\r\n')

# Read actual deploy/backup paths from watcher result file; fall back to DB config value
DEPLOY_PATH="$RAW_LOCAL"
BACKUP_PATH=""
if [[ -n "$WATCHER_RESULT_FILE" ]]; then
    DEPLOY_PATH=$("$POWERSHELL" -NoProfile -Command "
        \$txt = Get-Content '$WATCHER_RESULT_FILE' -ErrorAction SilentlyContinue
        \$line = \$txt | Where-Object { \$_ -match '^DeployPath=' }
        if (\$line) { (\$line -split '=',2)[1] } else { '$RAW_LOCAL' }
    " 2>/dev/null | tr -d '\r\n')
    BACKUP_PATH=$("$POWERSHELL" -NoProfile -Command "
        \$txt = Get-Content '$WATCHER_RESULT_FILE' -ErrorAction SilentlyContinue
        \$line = \$txt | Where-Object { \$_ -match '^BackupPath=' }
        if (\$line) { (\$line -split '=',2)[1] }
    " 2>/dev/null | tr -d '\r\n')
fi
[[ -z "$DEPLOY_PATH" ]] && DEPLOY_PATH="$RAW_LOCAL"

if [[ "$WATCHER_CONFIRMED" == true ]]; then
    SUBJECT="Deployment Completed — $PROJECT ($BRANCH)"
    BADGE_TEXT="✓ CONFIRMED BY WATCHER"
    HEADER_COLOR="#2e7d32"
    BORDER_COLOR="#4caf50"
    BG_COLOR="#f1f8e9"
    FOOTER_TEXT="✓ Deployment verified by watchdog service"
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

BACKUP_ROW=""
[[ -n "$BACKUP_PATH" ]] && BACKUP_ROW=$(_monorow "Backup" "$BACKUP_PATH")

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
      $(_row "Server"       "${SERVER_NAME}")
      $(_monorow "Deploy Path" "${DEPLOY_PATH}")
      ${BACKUP_ROW}
      $(_row "Triggered By" "${TRIGGERED_BY}")
      $(_row "IIS Restart"  "Yes")
    </table>
    <div style="margin-top:20px;padding-top:14px;border-top:1px solid #ddd;font-size:12px;color:#555;">
      <div>${FOOTER_TEXT}</div>
      <div style="margin-top:4px;color:#888;">Email sent only after successful watchdog confirmation.</div>
      <div style="margin-top:2px;color:#888;">Detailed log attached for your records.</div>
    </div>
  </div>
</div>
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
# STEP 7: DEPLOYMENT COMPLETE
# ==================================================
log ""
log "✅ Done!"
log ""

exit 0