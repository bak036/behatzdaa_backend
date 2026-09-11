#!/usr/bin/env bash
# ===========================================================
# fix_postbuild_conditions.sh
# Scans every .csproj under the repo root and adds the missing
# Condition="'$(SolutionDir)' != '' AND '$(SolutionDir)' != '*Undefined*'"
# to any <Target Name="PostBuild"> that doesn't already have a
# Condition attribute. Without it, the PostBuild xcopy step fails
# with MSB3073 ("*Undefined*..." path) whenever the project is built
# outside Visual Studio (e.g. via "dotnet build/publish"), because
# $(SolutionDir) is only ever set by the VS/MSBuild solution build,
# never by a per-project dotnet CLI build.
#
# Uses a targeted text/regex patch on just the <Target Name="PostBuild">
# opening tag rather than loading the file as XML and re-saving the
# whole document — XmlDocument.Save() reformats the entire file
# (strips blank lines, changes indentation), which would pollute the
# diff with unrelated changes. This only touches the one line/tag.
# ===========================================================

set -e
set +u

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
    exit 1
}

# ============================================================
# powershell.exe is a native Windows process — it cannot resolve
# WSL-style paths like "/mnt/c/Users/...". Convert to a Windows
# path whichever shell we're running under (WSL -> wslpath,
# Git-Bash/MSYS -> cygpath, otherwise assume it's already a
# Windows path).
# ============================================================
to_windows_path() {
    local p="$1"
    if command -v wslpath >/dev/null 2>&1; then
        wslpath -w "$p"
    elif command -v cygpath >/dev/null 2>&1; then
        cygpath -w "$p"
    else
        echo "$p"
    fi
}

ROOT=$(git rev-parse --show-toplevel 2>/dev/null || pwd)
cd "$ROOT"

echo "🔍 Scanning .csproj files under: $ROOT"

TMP_PS1="$ROOT/.fix_postbuild_conditions.ps1"

cat > "$TMP_PS1" << 'PSEOF'
param(
    [Parameter(Mandatory = $true)]
    [string]$RootPath
)

$ErrorActionPreference = 'Stop'

# Literal text to insert — built with backticks so PowerShell doesn't
# try to expand $(SolutionDir) as its own variable syntax.
$conditionValue = "'`$(SolutionDir)' != '' AND '`$(SolutionDir)' != '*Undefined*'"

# Matches the <Target Name="PostBuild" ...> opening tag, capturing any
# attributes after Name so they're preserved, but only when the tag
# does NOT already contain a Condition attribute (negative lookahead
# per character, so it also works across multi-line tags).
$pattern = '<Target\s+Name="PostBuild"((?:(?!Condition=)[^>])*)>'

$evaluator = {
    param($m)
    '<Target Name="PostBuild"' + $m.Groups[1].Value + ' Condition="' + $conditionValue + '">'
}

$csprojFiles = Get-ChildItem -Path $RootPath -Recurse -Filter *.csproj -File |
    Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }

$changed = @()
$skipped = @()
$notFound = @()

foreach ($file in $csprojFiles) {
    # StreamReader with BOM detection preserves the file's original
    # encoding (UTF-8 w/ or w/o BOM) so we don't silently rewrite it.
    $reader = New-Object System.IO.StreamReader($file.FullName, [System.Text.Encoding]::UTF8, $true)
    $content = $reader.ReadToEnd()
    $encodingUsed = $reader.CurrentEncoding
    $reader.Close()

    if ($content -notmatch 'Name="PostBuild"') {
        $notFound += $file.FullName
        continue
    }

    $newContent = [regex]::Replace($content, $pattern, [System.Text.RegularExpressions.MatchEvaluator]$evaluator)

    if ($newContent -ne $content) {
        [System.IO.File]::WriteAllText($file.FullName, $newContent, $encodingUsed)
        $changed += $file.FullName
    } else {
        $skipped += $file.FullName
    }
}

Write-Output '---CHANGED---'
$changed | ForEach-Object { Write-Output $_ }
Write-Output '---SKIPPED---'
$skipped | ForEach-Object { Write-Output $_ }
Write-Output '---NOPOSTBUILD---'
$notFound | ForEach-Object { Write-Output $_ }
PSEOF

ROOT_WIN=$(to_windows_path "$ROOT")
TMP_PS1_WIN=$(to_windows_path "$TMP_PS1")

RESULT=$("$POWERSHELL" -NoProfile -ExecutionPolicy Bypass -File "$TMP_PS1_WIN" -RootPath "$ROOT_WIN" 2>&1)
PS_EXIT=$?

rm -f "$TMP_PS1"

if [[ $PS_EXIT -ne 0 ]]; then
    echo "❌ ERROR: PowerShell script failed"
    echo "$RESULT"
    exit 1
fi

echo "$RESULT" | awk '
/---CHANGED---/ { section="changed"; next }
/---SKIPPED---/ { section="skipped"; next }
/---NOPOSTBUILD---/ { section="none"; next }
section=="changed" && NF { print "   ✓ Fixed: " $0 }
section=="skipped" && NF { print "   – Already has Condition: " $0 }
section=="none" && NF { print "   · No PostBuild target: " $0 }
'

echo ""
echo "✅ Done."