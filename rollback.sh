#!/usr/bin/env bash
# ===============================================================
# ♻️ UNIVERSAL AUTO-ROLLBACK SCRIPT — WITH AUTO-SETUP
# ===============================================================

set -e
if (set -o pipefail >/dev/null 2>&1); then set -o pipefail; fi

# Dynamic PowerShell path detection
if [[ "$SHELL" == *bash* ]]; then
  POWERSHELL="powershell.exe"
else
  POWERSHELL="/c/Windows/System32/WindowsPowerShell/v1.0/powershell.exe"
fi

# Check if PowerShell is available
# Check if PowerShell is available

if [ ! -f "$POWERSHELL" ] && ! command -v "$POWERSHELL" >/dev/null 2>&1; then
    echo "❌ ERROR: PowerShell not found"
    echo "   This script requires Windows PowerShell to run."
    echo "   Please ensure you're running on Windows with PowerShell installed."
    exit 1
fi

ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)
cd "$ROOT"

# ===== AUTO-DETECT PROJECT AND BRANCH =====
PROJECT_NAME=$(basename "$ROOT")
BRANCH=$(git rev-parse --abbrev-ref HEAD)

LOG_FILE="$ROOT/automation_master_log.txt"
VARS_FILE="$ROOT/config_variables.json"

# Logging helper
log() {
    local msg="$*"
    local timestamp="[ $(date '+%Y-%m-%d %H:%M:%S') ]"
    echo "$timestamp $msg" | tee -a "$LOG_FILE"
}

# Variable replacement function
replace_variables() {
    local input="$1"
    local result="$input"
    
    CURRENT_PROJECT=$(basename "$ROOT")
    CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
    
    result="${result//\{PROJECT_NAME\}/$CURRENT_PROJECT}"
    result="${result//\{BRANCH_NAME\}/$CURRENT_BRANCH}"
    result="${result//\{REPO_ROOT\}/$ROOT}"
    
    if [ -f "$VARS_FILE" ]; then
        while IFS= read -r line; do
            if [[ "$line" =~ \"(\{[^}]+\})\"[[:space:]]*:[[:space:]]*\"([^\"]+)\" ]]; then
                var_name="${BASH_REMATCH[1]}"
                var_value="${BASH_REMATCH[2]}"
                result="${result//$var_name/$var_value}"
            fi
        done < <(grep -o '"{[^}]*}"[[:space:]]*:[[:space:]]*"[^"]*"' "$VARS_FILE")
    fi
    
    echo "$result"
}

# Ensure log file exists
LOG_DIR=$(dirname "$LOG_FILE")
mkdir -p "$LOG_DIR"
if [ ! -f "$LOG_FILE" ]; then
    echo "===================================================" > "$LOG_FILE"
    echo "[ $(date '+%Y-%m-%d %H:%M:%S') ] Rollback Log initialized" >> "$LOG_FILE"
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

JQ=$(command -v jq || echo "jq")

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

log "==================================================="
log "✓ All dependencies ready"
log "==================================================="
log ""

# ===== LOAD CONFIG FROM DB - FILTERED BY PROJECT AND BRANCH =====
log "📊 Loading configuration from database..."
log "   Server: 172.29.92.20\sql2005"
log "   Database: NofTest"
log "   Looking for: Project='$PROJECT_NAME', Branch='$BRANCH'"

db_values=$("$POWERSHELL" -NoProfile -Command '
  $ErrorActionPreference = "Stop"
  try {
    # Ensure SqlServer module is imported
    Import-Module SqlServer -ErrorAction Stop | Out-Null
    
    $connectionString = "Server=172.29.92.20\sql2005;Database=NofTest;User ID=alexk;Password=Dtsal21xk;TrustServerCertificate=True;";
    $query = "SELECT TOP 1 pr.ProjectName, pr.Branch, pr.FlagName, pr.ZipName, pr.AppPool, pr.SiteName, pdc.RemoteServer, pdc.RemoteUser, pdc.RemotePassword, pdc.LocalPublishDir FROM NofTest..ProjectRules pr LEFT JOIN NofTest..ProjectDepJoyConfig pdc ON pr.ProjectName = pdc.Project WHERE pr.ProjectName='"'$PROJECT_NAME'"' AND pr.Branch='"'$BRANCH'"'";
    
    $result = Invoke-Sqlcmd -ConnectionString $connectionString -Query $query -ErrorAction Stop;
    
    if ($null -eq $result) {
      Write-Output "NO_RESULTS"
    } else {
      foreach ($row in $result) {
        $line = ($row.ProjectName + "|||DELIM|||" + $row.Branch + "|||DELIM|||" + $row.FlagName + "|||DELIM|||" + $row.ZipName + "|||DELIM|||" + $row.AppPool + "|||DELIM|||" + $row.SiteName + "|||DELIM|||" + $row.RemoteServer + "|||DELIM|||" + $row.RemoteUser + "|||DELIM|||" + $row.RemotePassword + "|||DELIM|||" + $row.LocalPublishDir)
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
    log "❌ ERROR: No configuration found in database"
    log "   Project: $PROJECT_NAME"
    log "   Branch: $BRANCH"
    log ""
    log "   Please ensure the configuration exists in ProjectRules table"
    exit 1
fi

log "   ✓ Configuration loaded from database"

# Parse DB values
PROJECT=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $1}')
BRANCH=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $2}')
FLAG_NAME=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $3}')
ZIP_NAME=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $4}')
APP_POOL=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $5}')
SITE_NAME=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $6}')
REMOTE_SERVER=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $7}')
REMOTE_USER=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $8}')
REMOTE_PASSWORD=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $9}')
DEPLOY_ROOT=$(echo "$db_values" | awk -F'\\|\\|\\|DELIM\\|\\|\\|' '{print $10}')

# Replace variables in all config values
log "🔄 Replacing variables in configuration..."
PROJECT=$(replace_variables "$PROJECT")
BRANCH=$(replace_variables "$BRANCH")
FLAG_NAME=$(replace_variables "$FLAG_NAME")
REMOTE_SERVER=$(replace_variables "$REMOTE_SERVER")
REMOTE_USER=$(replace_variables "$REMOTE_USER")
REMOTE_PASSWORD=$(replace_variables "$REMOTE_PASSWORD")
DEPLOY_ROOT=$(replace_variables "$DEPLOY_ROOT")

# Store RAW values for Windows paths
RAW_REMOTE="$REMOTE_SERVER"
RAW_FLAG="$FLAG_NAME"

# Normalize paths for Bash
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

CONFIRM_DIR="//PP-SERVER/CiCd/RESULTS"

log "==============================================================="
log "== ♻️  ROLLBACK STARTED =="
log "==============================================================="
log "Project:      $PROJECT"
log "Branch:       $BRANCH"
log "Flag:         $FLAG_NAME"
log "Remote:       $REMOTE_SERVER"
log "User:         $REMOTE_USER"
log "Deploy Root:  $DEPLOY_ROOT"
log "==============================================================="

# STEP 1: CREATE AND UPLOAD ROLLBACK FLAG
log "🪶 [1/2] Creating rollback flag..."

FLAG_PATH="./${RAW_FLAG}"
echo "rollback triggered $(date)" > "$FLAG_PATH"
log "   ✓ Flag created: $RAW_FLAG"

ROLLBACK_START=$(date +%s)

log "📤 Uploading rollback flag to server..."

UPLOAD_RESULT=$("$POWERSHELL" -NoProfile -Command "
try {
    try { net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null } catch {}
    
    \$netUseResult = net use '$RAW_REMOTE' /user:'$REMOTE_USER' '$REMOTE_PASSWORD' /persistent:no 2>&1
    if (\$LASTEXITCODE -ne 0) { throw \"Connection failed\" }
    
    \$destFile = \"$RAW_REMOTE\\$RAW_FLAG\"
    Copy-Item -Path '$FLAG_PATH' -Destination \$destFile -Force -ErrorAction Stop
    
    net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null
    Write-Output 'SUCCESS'
} catch {
    Write-Output \"FAILED: \$(\$_.Exception.Message)\"
    try { net use '$RAW_REMOTE' /delete /y 2>&1 | Out-Null } catch {}
    exit 1
}
" 2>&1)

if [[ "$UPLOAD_RESULT" != *"SUCCESS"* ]]; then
    log "❌ ERROR: Failed to upload rollback flag"
    exit 1
fi

log "   ✓ Rollback flag uploaded successfully"

# STEP 2: WAIT FOR WATCHER
log "⏳ [2/2] Waiting for watcher confirmation..."

WATCHER_CONFIRMED=false
MAX_WAIT=120
CHECK_INTERVAL=2
RESULT_FILE=""

for ((i=1; i<=MAX_WAIT/CHECK_INTERVAL; i++)); do
    ELAPSED=$((i * CHECK_INTERVAL))
    
    RESULT=$("$POWERSHELL" -NoProfile -Command "
        \$rollbackStart = (Get-Date '1970-01-01 00:00:00Z').AddSeconds($ROLLBACK_START)
        if (Test-Path '$CONFIRM_DIR') {
            \$file = Get-ChildItem '$CONFIRM_DIR' -Filter '*${PROJECT}*${BRANCH}*Rollback*Success*.txt' -ErrorAction SilentlyContinue |
                Where-Object { \$_.LastWriteTime -gt \$rollbackStart } |
                Select-Object -First 1
            if (\$file) { Write-Output \$file.FullName }
        }
    " 2>/dev/null | tr -d '\r\n' | sed 's/^[[:space:]]*//;s/[[:space:]]*$//')
    
    if [[ -n "$RESULT" ]] && [[ "$RESULT" != "null" ]]; then
        WATCHER_CONFIRMED=true
        RESULT_FILE="$RESULT"
        log "   ✓ Watcher confirmed after ${ELAPSED}s!"
        log "   📄 Result file: $(basename "$RESULT")"
        break
    fi
    
    [[ $ELAPSED -eq 30 ]] && log "   ... waiting (30s)"
    [[ $ELAPSED -eq 60 ]] && log "   ... waiting (60s)"
    
    sleep $CHECK_INTERVAL
done

# Parse watcher result file to get rollback details
if [[ "$WATCHER_CONFIRMED" == true ]] && [[ -n "$RESULT_FILE" ]]; then
    log "📄 Reading rollback details from watcher..."
    
    # Convert the path back to Windows format for PowerShell
    RESULT_FILE_WIN=$(echo "$RESULT_FILE" | sed 's#/#\\#g')
    
    WATCHER_DATA=$("$POWERSHELL" -NoProfile -Command "
        try {
            if (Test-Path '$RESULT_FILE_WIN') {
                Get-Content '$RESULT_FILE_WIN' -Raw -ErrorAction Stop
            } else {
                Write-Output 'FILE_NOT_FOUND'
            }
        } catch {
            Write-Output \"ERROR: \$(\$_.Exception.Message)\"
        }
    " 2>&1)
    
    if [[ "$WATCHER_DATA" == *"FILE_NOT_FOUND"* ]]; then
        log "   ⚠️  Warning: Confirmation file not accessible"
        WATCHER_DEPLOY_ROOT="Confirmed (file not accessible)"
        WATCHER_BACKUP="Confirmed (file not accessible)"
        WATCHER_IIS="Unknown"
        WATCHER_SERVER="PP-SERVER"
    elif [[ "$WATCHER_DATA" == *"ERROR:"* ]]; then
        log "   ⚠️  Warning: Error reading file: $WATCHER_DATA"
        WATCHER_DEPLOY_ROOT="Confirmed (read error)"
        WATCHER_BACKUP="Confirmed (read error)"
        WATCHER_IIS="Unknown"
        WATCHER_SERVER="PP-SERVER"
    elif [[ -n "$WATCHER_DATA" ]] && [[ "$WATCHER_DATA" != "null" ]]; then
        # Parse Key=Value format from the confirmation file
        NORMALIZED_DATA=$(echo "$WATCHER_DATA" | sed 's/\r$//')
        
        WATCHER_DEPLOY_ROOT=$(echo "$NORMALIZED_DATA" | grep -E '^DeployRoot=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        if [[ -z "$WATCHER_DEPLOY_ROOT" ]]; then
            WATCHER_DEPLOY_ROOT=$(echo "$NORMALIZED_DATA" | grep -E '^Site=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        fi
        
        WATCHER_BACKUP=$(echo "$NORMALIZED_DATA" | grep -E '^BackupRemoved=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        
        WATCHER_IIS=$(echo "$NORMALIZED_DATA" | grep -E '^IISRestart=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        
        WATCHER_SERVER=$(echo "$NORMALIZED_DATA" | grep -E '^WatcherNode=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        if [[ -z "$WATCHER_SERVER" ]]; then
            WATCHER_SERVER=$(echo "$NORMALIZED_DATA" | grep -E '^Pool=' | cut -d'=' -f2- | sed 's/^[[:space:]]*//;s/[[:space:]]*$//' || true)
        fi
        
        [[ -z "$WATCHER_DEPLOY_ROOT" ]] && WATCHER_DEPLOY_ROOT="Path not in confirmation"
        [[ -z "$WATCHER_BACKUP" ]] && WATCHER_BACKUP="Backup info not in confirmation"
        [[ -z "$WATCHER_IIS" ]] && WATCHER_IIS="Not specified"
        [[ -z "$WATCHER_SERVER" ]] && WATCHER_SERVER="DTSLC-PP"
        
        log "   ✓ Rollback details retrieved:"
        log "      📁 Deploy Root: $WATCHER_DEPLOY_ROOT"
        log "      🗑️  Backup Removed: $WATCHER_BACKUP"
        log "      🔄 IIS Restart: $WATCHER_IIS"
        log "      🖥️  Server: $WATCHER_SERVER"
    else
        log "   ⚠️  Warning: Confirmation file is empty"
        WATCHER_DEPLOY_ROOT="Confirmed (details not available)"
        WATCHER_BACKUP="Confirmed (details not available)"
        WATCHER_IIS="Unknown"
        WATCHER_SERVER="PP-SERVER"
    fi
else
    WATCHER_DEPLOY_ROOT=""
    WATCHER_BACKUP=""
    WATCHER_IIS=""
    WATCHER_SERVER=""
fi

# Cleanup
rm -f "$FLAG_PATH"

# ----------------------------------------------------
# SEND EMAIL - WITH IMPROVED USER DETECTION
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

log ""
log "   👤 Detected user: $TRIGGERED_BY"
log "   📧 Email address: $EMAIL"

if [[ "$WATCHER_CONFIRMED" == true ]]; then
    EMAIL_SUBJECT="♻️ Rollback Completed — $PROJECT ($BRANCH)"
    EMAIL_BADGE="✓ CONFIRMED BY WATCHER"
    EMAIL_BGCOLOR="#e3f2fd"
    EMAIL_COLOR="#1976d2"
    EMAIL_FOOTER="<strong>✓ Rollback verified by watchdog service</strong><br>Previous deployment has been restored successfully."
else
    EMAIL_SUBJECT="🟨 Rollback Requested — $PROJECT ($BRANCH)"
    EMAIL_BADGE="⚠ WATCHDOG DID NOT RESPOND"
    EMAIL_BGCOLOR="#fff3e0"
    EMAIL_COLOR="#ff9800"
    EMAIL_FOOTER="<strong>⚠ Note: Watchdog service did not confirm rollback</strong><br>Rollback flag uploaded successfully, but watchdog confirmation not received within 2 minutes.<br>Please check watchdog service logs and verify rollback manually."
fi

# Escape special characters for PowerShell
WATCHER_DEPLOY_ROOT_ESC=$(echo "$WATCHER_DEPLOY_ROOT" | sed "s/'/''/g")
WATCHER_BACKUP_ESC=$(echo "$WATCHER_BACKUP" | sed "s/'/''/g")
WATCHER_IIS_ESC=$(echo "$WATCHER_IIS" | sed "s/'/''/g")
WATCHER_SERVER_ESC=$(echo "$WATCHER_SERVER" | sed "s/'/''/g")
DEPLOY_ROOT_ESC=$(echo "$DEPLOY_ROOT" | sed "s/'/''/g")

log "📝 DEBUG: Email variables before sending:"
log "   PROJECT='$PROJECT'"
log "   BRANCH='$BRANCH'"
log "   DEPLOY_ROOT='$WATCHER_DEPLOY_ROOT'"
log "   BACKUP_REMOVED='$WATCHER_BACKUP'"
log "   IIS_RESTART='$WATCHER_IIS'"
log "   SERVER='$WATCHER_SERVER'"

EMAIL_RESULT=$("$POWERSHELL" -NoProfile -Command "
try {
    \$outlook = New-Object -ComObject Outlook.Application
    \$mail = \$outlook.CreateItem(0)
    
    \$mail.To = '$EMAIL'
    \$mail.Subject = '$EMAIL_SUBJECT'
    
    \$timestamp = Get-Date -Format 'dd.MM.yyyy HH:mm'
    \$triggeredBy = '$TRIGGERED_BY'
    \$project = '$PROJECT'
    \$branch = '$BRANCH'
    
    # Use watcher data if available, otherwise use defaults
    \$deployRoot = if ('$WATCHER_DEPLOY_ROOT_ESC' -ne '') { '$WATCHER_DEPLOY_ROOT_ESC' } else { '$DEPLOY_ROOT_ESC' }
    \$backupRemoved = if ('$WATCHER_BACKUP_ESC' -ne '') { '$WATCHER_BACKUP_ESC' } else { 'Not confirmed by watcher' }
    \$iisRestart = if ('$WATCHER_IIS_ESC' -ne '') { '$WATCHER_IIS_ESC' } else { 'Unknown' }
    \$serverNode = if ('$WATCHER_SERVER_ESC' -ne '') { '$WATCHER_SERVER_ESC' } else { 'PP-SERVER' }
    
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
        <tr><td>Project</td><td><strong>\$project</strong></td></tr>
        <tr><td>Branch</td><td><strong>\$branch</strong></td></tr>
        <tr><td>Rollback Time</td><td>\$timestamp</td></tr>
        <tr><td>Server</td><td>\$serverNode</td></tr>
        <tr><td>Deployment Root</td><td>\$deployRoot</td></tr>
        <tr><td>Backup Removed</td><td>\$backupRemoved</td></tr>
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
    log "🎉 ROLLBACK COMPLETE - WATCHER CONFIRMED"
    log "   📁 Deployment Root: $WATCHER_DEPLOY_ROOT"
    log "   🗑️  Backup Removed: $WATCHER_BACKUP"
    log "   🔄 IIS Restart: $WATCHER_IIS"
else
    log "⚠️  ROLLBACK FLAG UPLOADED - WATCHER DID NOT RESPOND"
    log "   Flag uploaded to: $RAW_REMOTE"
    log "   Please verify watchdog service is processing the rollback"
fi
log "==================================================="

exit 0