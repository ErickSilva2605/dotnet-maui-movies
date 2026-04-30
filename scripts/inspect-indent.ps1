# Detecta arquivos .cs que indentam com espacos em vez de tabs
# (regra do .editorconfig: *.cs deve usar tab)

$root = Resolve-Path "$PSScriptRoot\.."
$exclude = @('\bin\', '\obj\', '\.vs\', '\.git\', '\.idea\', '\.gradle\', '\node_modules\', '\.dotnet\', '\.packages\', '\artifacts\', '\Migrations\')

$problems = New-Object System.Collections.Generic.List[string]

Get-ChildItem -Path $root -Recurse -File -Filter *.cs | Where-Object {
    $path = $_.FullName
    foreach ($pattern in $exclude) {
        if ($path -like "*$pattern*") { return $false }
    }
    $true
} | ForEach-Object {
    $f = $_.FullName
    $rel = $f.Substring($root.Path.Length + 1)
    $lines = [System.IO.File]::ReadAllLines($f)

    $tabIndentLines = 0
    $spaceIndentLines = 0
    foreach ($line in $lines) {
        if ($line -match '^\t') { $tabIndentLines++ }
        elseif ($line -match '^[ ]{2,}\S') { $spaceIndentLines++ }
    }

    if ($spaceIndentLines -gt 0) {
        $problems.Add(("space={0,-4} tab={1,-4}  {2}" -f $spaceIndentLines, $tabIndentLines, $rel))
    }
}

Write-Host ""
if ($problems.Count -gt 0) {
    Write-Host "===== .CS FILES WITH SPACE INDENT (should be tab) =====" -ForegroundColor Yellow
    $problems | ForEach-Object { Write-Host $_ }
    Write-Host ""
    Write-Host ("Total: {0}" -f $problems.Count) -ForegroundColor Yellow
}
else {
    Write-Host "All .cs files use tab indentation." -ForegroundColor Green
}
