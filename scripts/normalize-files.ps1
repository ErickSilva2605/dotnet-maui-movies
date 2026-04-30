# Normaliza todos os arquivos texto do projeto:
#   - Remove BOM
#   - Converte EOL para CRLF (LF e CR isolados viram CRLF)
#   - Salva como UTF-8 sem BOM
#
# Diretórios excluídos: bin, obj, .vs, .git, .idea, .gradle, node_modules, .dotnet, .packages, artifacts
# Arquivos binários (png, dll, etc.) são ignorados pela whitelist de extensões.

$root = Resolve-Path "$PSScriptRoot\.."
$extensions = @(
    'cs', 'csx', 'vb',
    'xaml', 'xml',
    'json',
    'csproj', 'vbproj', 'props', 'targets', 'config',
    'sln', 'slnx',
    'resx',
    'editorconfig', 'gitignore', 'gitattributes',
    'md', 'txt',
    'yml', 'yaml',
    'ps1', 'cmd', 'bat',
    'svg'
)
$specialNames = @('Settings.XamlStyler')
$exclude = @('\bin\', '\obj\', '\.vs\', '\.git\', '\.idea\', '\.gradle\', '\node_modules\', '\.dotnet\', '\.packages\', '\artifacts\')

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$changed = New-Object System.Collections.Generic.List[string]
$total = 0

Get-ChildItem -Path $root -Recurse -File | Where-Object {
    $path = $_.FullName
    foreach ($pattern in $exclude) {
        if ($path -like "*$pattern*") { return $false }
    }
    $true
} | Where-Object {
    $name = $_.Name
    $ext = $_.Extension.TrimStart('.').ToLowerInvariant()
    ($extensions -contains $ext) -or ($specialNames -contains $name) -or ($name -like '.*' -and ($extensions -contains $name.TrimStart('.').ToLowerInvariant()))
} | ForEach-Object {
    $f = $_.FullName
    $rel = $f.Substring($root.Path.Length + 1)
    $total++

    $bytes = [System.IO.File]::ReadAllBytes($f)
    if ($bytes.Length -eq 0) { return }

    $hadBom = ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF)
    if ($hadBom) {
        $bytes = $bytes[3..($bytes.Length - 1)]
    }

    $content = [System.Text.Encoding]::UTF8.GetString($bytes)
    $original = $content

    # Normaliza EOL: qualquer combinacao vira \n primeiro, depois \n vira \r\n
    $content = $content -replace "`r`n", "`n"
    $content = $content -replace "`r", "`n"
    $content = $content -replace "`n", "`r`n"

    if ($hadBom -or $content -ne $original) {
        [System.IO.File]::WriteAllText($f, $content, $utf8NoBom)
        $reasons = @()
        if ($hadBom) { $reasons += 'BOM stripped' }
        if ($content -ne $original) { $reasons += 'EOL -> CRLF' }
        $changed.Add(("{0,-30} {1}" -f ($reasons -join ', '), $rel))
    }
}

Write-Host ""
Write-Host "===== NORMALIZATION RESULT =====" -ForegroundColor Cyan
Write-Host ("Files inspected : {0}" -f $total)
Write-Host ("Files changed   : {0}" -f $changed.Count)
Write-Host ""
if ($changed.Count -gt 0) {
    Write-Host "===== CHANGED =====" -ForegroundColor Yellow
    $changed | ForEach-Object { Write-Host $_ }
}
else {
    Write-Host "No changes needed." -ForegroundColor Green
}
