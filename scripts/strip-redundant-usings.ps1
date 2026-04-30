# Remove de cada arquivo .cs os 'using X;' que ja estao em GlobalUsings.cs
# do mesmo projeto.

$projects = @(
    'src/MauiMovies.Core',
    'src/MauiMovies.Infrastructure',
    'src/MauiMovies.UI',
    'tests/MauiMovies.Core.Tests',
    'tests/MauiMovies.Infrastructure.Tests'
)

$root = Resolve-Path "$PSScriptRoot\.."
$exclude = @('\bin\', '\obj\', '\Migrations\', '\Resources\Strings\AppResources.Designer.cs', '\GlobalUsings.cs')
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$totalChanged = 0

foreach ($projRel in $projects) {
    $projPath = Join-Path $root $projRel
    $globalsFile = Join-Path $projPath 'GlobalUsings.cs'
    if (-not (Test-Path $globalsFile)) { continue }

    # Lista de namespaces globais
    $globalNs = @()
    foreach ($line in [System.IO.File]::ReadAllLines($globalsFile)) {
        if ($line -match '^\s*global\s+using\s+(?!static\b)([^=;]+);') {
            $globalNs += $matches[1].Trim()
        }
    }

    if ($globalNs.Count -eq 0) { continue }

    Write-Host ""
    Write-Host ("===== {0} =====" -f $projRel) -ForegroundColor Cyan
    Write-Host ("Globals: {0}" -f ($globalNs -join ', '))

    Get-ChildItem -Path $projPath -Recurse -File -Filter *.cs | Where-Object {
        $path = $_.FullName
        foreach ($pattern in $exclude) {
            if ($path -like "*$pattern*") { return $false }
        }
        $true
    } | ForEach-Object {
        $f = $_.FullName
        $rel = $f.Substring($root.Path.Length + 1)
        $original = [System.IO.File]::ReadAllText($f)

        $lines = [System.IO.File]::ReadAllLines($f)
        $newLines = New-Object System.Collections.Generic.List[string]
        $removed = 0

        foreach ($line in $lines) {
            $skip = $false
            if ($line -match '^\s*using\s+(?!global\b|static\b)([^=;]+);') {
                $ns = $matches[1].Trim()
                if ($globalNs -contains $ns) {
                    $skip = $true
                    $removed++
                }
            }
            if (-not $skip) { $newLines.Add($line) }
        }

        if ($removed -gt 0) {
            $newContent = ($newLines -join "`r`n")
            # preserva final newline se existia
            if ($original.EndsWith("`r`n") -or $original.EndsWith("`n")) {
                $newContent += "`r`n"
            }
            [System.IO.File]::WriteAllText($f, $newContent, $utf8NoBom)
            $totalChanged++
            Write-Host ("  -{0,-2}  {1}" -f $removed, $rel)
        }
    }
}

Write-Host ""
Write-Host ("Total files changed: {0}" -f $totalChanged) -ForegroundColor Green
