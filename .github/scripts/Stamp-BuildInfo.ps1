<#
.SYNOPSIS
    Stamps build-tracking metadata into the AssemblyInfo file(s) compiled by the
    given .csproj project(s), so the values surface in the DLL's Windows
    Properties -> Details tab (File version / Product version / Comments).

.DESCRIPTION
    Intended to run inside a GitHub Actions build step BEFORE msbuild. The runner
    works on a fresh checkout and never commits, so these edits are temporary and
    never reach the repository.

    For each project it auto-discovers the "Properties\AssemblyInfo*.cs" file the
    project compiles (the mapping is inconsistent across versions, so we read it
    from the .csproj instead of guessing), then sets-or-appends one SHORT value
    per Windows "Details" tab field so nothing gets truncated (that column is a
    fixed, non-resizable width):
      AssemblyFileVersion           -> "<maj>.<min>"               (Details: File version)
      AssemblyInformationalVersion  -> "<ver> (<shortSha>)"        (Details: Product version)
      AssemblyProduct               -> "Built <date time> PHT"     (Details: Product name)
      AssemblyCopyright             -> "Branch: <branch>"          (Details: Copyright)
      AssemblyDescription           -> full readable record        (CLI / VersionInfo.Comments)
    AssemblyVersion (strong-name binding identity) is intentionally left untouched.

.PARAMETER CsprojPaths
    One or more paths to the .csproj files being built.

.PARAMETER AcuVersion
    The Acumatica release label, e.g. "25R2". Used for the numeric file version.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)] [string[]] $CsprojPaths,
    [Parameter(Mandatory = $true)] [string]   $AcuVersion
)

$ErrorActionPreference = 'Stop'

# --- Gather metadata (GitHub-provided env vars, with safe fallbacks) ----------
$branch     = if ($env:GITHUB_REF_NAME)    { $env:GITHUB_REF_NAME }    else { 'local' }
$sha        = if ($env:GITHUB_SHA)         { $env:GITHUB_SHA }         else { '' }
$shortSha   = if ($sha.Length -ge 7)       { $sha.Substring(0, 7) }    else { $sha }

# Build timestamp in Philippine time. "Singapore Standard Time" is a fixed UTC+8
# zone with no DST (== Asia/Manila) and is always present on Windows runners.
$phZone = [System.TimeZoneInfo]::FindSystemTimeZoneById('Singapore Standard Time')
$phNow  = [System.TimeZoneInfo]::ConvertTimeFromUtc([DateTime]::UtcNow, $phZone)
$stamp  = $phNow.ToString('yyyy-MM-dd HH:mm:ss') + ' PHT'

# Numeric file version: "25R2" -> "25.2"
$fileVersion = "0.0"
if ($AcuVersion -match '(\d+)\s*R\s*(\d+)') {
    $fileVersion = "$($Matches[1]).$($Matches[2])"
}

# One short value per Details-tab field — the column is fixed-width and truncates
# long strings, so each piece of data gets its own field to stay fully visible.
$infoVersion = "$AcuVersion ($shortSha)"
$product     = "Built $stamp"
$copyright   = "Branch: $branch"
$description = "Acumatica $AcuVersion - branch '$branch', commit $shortSha, built $stamp"

Write-Host "Build info to stamp:"
Write-Host "  AssemblyFileVersion          = $fileVersion"
Write-Host "  AssemblyInformationalVersion = $infoVersion"
Write-Host "  AssemblyProduct              = $product"
Write-Host "  AssemblyCopyright            = $copyright"
Write-Host "  AssemblyDescription          = $description"

# --- Helper: replace an [assembly: Name(...)] line, or append if absent -------
function Set-AssemblyAttribute {
    param([string] $Content, [string] $Name, [string] $Value)

    $escaped     = $Value.Replace('\', '\\').Replace('"', '\"')
    $replacement = "[assembly: $Name(`"$escaped`")]"
    # Greedy .* so a value that itself contains "()" still matches the whole
    # existing line for replacement (keeps the helper robust to any value).
    $pattern     = "(?m)^[ \t]*\[assembly:[ \t]*$Name[ \t]*\(.*\)[ \t]*\]"

    if ([regex]::IsMatch($Content, $pattern)) {
        return [regex]::Replace($Content, $pattern, [System.Text.RegularExpressions.MatchEvaluator]{ param($m) $replacement })
    }
    return $Content.TrimEnd() + "`r`n$replacement`r`n"
}

# --- Apply to each project's AssemblyInfo file --------------------------------
foreach ($csproj in $CsprojPaths) {
    if (-not (Test-Path $csproj)) { throw "Project not found: $csproj" }

    $projText = Get-Content -LiteralPath $csproj -Raw
    $m = [regex]::Match($projText, 'Properties\\(AssemblyInfo[^"<]*\.cs)')
    if (-not $m.Success) { throw "No Properties\AssemblyInfo*.cs <Compile> entry found in $csproj" }

    $aiPath = Join-Path (Split-Path -Parent $csproj) ("Properties\" + $m.Groups[1].Value)
    if (-not (Test-Path $aiPath)) { throw "AssemblyInfo file not found: $aiPath" }

    Write-Host "Stamping $aiPath (from $(Split-Path -Leaf $csproj))"
    $content = Get-Content -LiteralPath $aiPath -Raw

    $content = Set-AssemblyAttribute -Content $content -Name 'AssemblyFileVersion'          -Value $fileVersion
    $content = Set-AssemblyAttribute -Content $content -Name 'AssemblyInformationalVersion' -Value $infoVersion
    $content = Set-AssemblyAttribute -Content $content -Name 'AssemblyProduct'              -Value $product
    $content = Set-AssemblyAttribute -Content $content -Name 'AssemblyCopyright'            -Value $copyright
    $content = Set-AssemblyAttribute -Content $content -Name 'AssemblyDescription'          -Value $description

    Set-Content -LiteralPath $aiPath -Value $content -Encoding UTF8 -NoNewline
}

Write-Host "Build info stamped successfully."
