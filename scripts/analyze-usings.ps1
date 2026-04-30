# Analisa usings por projeto e mostra os mais frequentes
# (candidatos a global using).

$projects = @(
    @{ Name = 'Core'; Path = 'src/MauiMovies.Core' },
    @{ Name = 'Infrastructure'; Path = 'src/MauiMovies.Infrastructure' },
    @{ Name = 'UI'; Path = 'src/MauiMovies.UI' },
    @{ Name = 'Core.Tests'; Path = 'tests/MauiMovies.Core.Tests' },
    @{ Name = 'Infrastructure.Tests'; Path = 'tests/MauiMovies.Infrastructure.Tests' }
)

$root = Resolve-Path "$PSScriptRoot\.."
$exclude = @('\bin\', '\obj\', '\Migrations\', '\Resources\Strings\AppResources.Designer.cs', '\GlobalUsings.cs')

foreach ($proj in $projects) {
    $projPath = Join-Path $root $proj.Path
    if (-not (Test-Path $projPath)) { continue }

    $usingCounts = @{}
    $totalFiles = 0

    Get-ChildItem -Path $projPath -Recurse -File -Filter *.cs | Where-Object {
        $path = $_.FullName
        foreach ($pattern in $exclude) {
            if ($path -like "*$pattern*") { return $false }
        }
        $true
    } | ForEach-Object {
        $totalFiles++
        $lines = [System.IO.File]::ReadAllLines($_.FullName)
        $seen = @{}
        foreach ($line in $lines) {
            if ($line -match '^\s*using\s+(?!global\b)([^=;]+);') {
                $ns = $matches[1].Trim()
                if (-not $seen.ContainsKey($ns)) {
                    $seen[$ns] = $true
                    if ($usingCounts.ContainsKey($ns)) { $usingCounts[$ns]++ }
                    else { $usingCounts[$ns] = 1 }
                }
            }
        }
    }

    Write-Host ""
    Write-Host ("===== {0} ({1} arquivos) =====" -f $proj.Name, $totalFiles) -ForegroundColor Cyan
    $usingCounts.GetEnumerator() | Sort-Object -Property Value -Descending | Where-Object { $_.Value -ge 3 } | ForEach-Object {
        Write-Host ("{0,4}x  {1}" -f $_.Value, $_.Key)
    }
}
