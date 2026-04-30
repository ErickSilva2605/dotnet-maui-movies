$root = Resolve-Path "$PSScriptRoot\.."
$extensions = @('cs', 'csx', 'vb', 'xaml', 'xml', 'json', 'csproj', 'props', 'targets', 'config', 'resx', 'sln', 'slnx', 'editorconfig', 'gitignore', 'gitattributes', 'md', 'txt', 'yml', 'yaml', 'XamlStyler')
$exclude = @('\bin\', '\obj\', '\.vs\', '\.git\', '\.idea\', '\.gradle\', '\node_modules\', '\.dotnet\', '\.packages\', '\artifacts\')

$summary = @{
    BOM = 0
    NoBOM = 0
    LF = 0
    CRLF = 0
    Mixed = 0
    Total = 0
}
$problems = New-Object System.Collections.Generic.List[string]

Get-ChildItem -Path $root -Recurse -File | Where-Object {
    $path = $_.FullName
    $excluded = $false
    foreach ($pattern in $exclude) {
        if ($path -like "*$pattern*") { $excluded = $true; break }
    }
    -not $excluded
} | Where-Object {
    $name = $_.Name
    $ext = $_.Extension.TrimStart('.')
    ($extensions -contains $ext) -or ($extensions -contains $name.TrimStart('.')) -or ($name -eq 'Settings.XamlStyler')
} | ForEach-Object {
    $f = $_.FullName
    $rel = $f.Substring($root.Path.Length + 1)
    $bytes = [System.IO.File]::ReadAllBytes($f)
    if ($bytes.Length -eq 0) { return }
    $hasBom = ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF)
    $content = [System.IO.File]::ReadAllText($f)
    $crlf = ([regex]::Matches($content, "`r`n")).Count
    $lfOnly = ([regex]::Matches($content, "(?<!`r)`n")).Count

    $summary.Total++
    if ($hasBom) { $summary.BOM++ } else { $summary.NoBOM++ }
    if ($crlf -gt 0 -and $lfOnly -eq 0) { $summary.CRLF++ }
    elseif ($lfOnly -gt 0 -and $crlf -eq 0) { $summary.LF++ }
    elseif ($crlf -gt 0 -and $lfOnly -gt 0) { $summary.Mixed++ }

    $issues = @()
    if ($hasBom) { $issues += 'BOM' }
    if ($lfOnly -gt 0 -and $crlf -eq 0) { $issues += 'LF' }
    if ($lfOnly -gt 0 -and $crlf -gt 0) { $issues += "MIXED($lfOnly LF, $crlf CRLF)" }
    if ($issues.Count -gt 0) {
        $problems.Add(("{0,-30} {1}" -f ($issues -join ' + '), $rel))
    }
}

Write-Host ""
Write-Host "===== SUMMARY =====" -ForegroundColor Cyan
Write-Host ("Total files inspected : {0}" -f $summary.Total)
Write-Host ("With BOM              : {0}" -f $summary.BOM)
Write-Host ("Without BOM           : {0}" -f $summary.NoBOM)
Write-Host ("Pure CRLF             : {0}" -f $summary.CRLF)
Write-Host ("Pure LF               : {0}" -f $summary.LF)
Write-Host ("Mixed EOL             : {0}" -f $summary.Mixed)
Write-Host ""
if ($problems.Count -gt 0) {
    Write-Host "===== FILES THAT NEED NORMALIZATION =====" -ForegroundColor Yellow
    $problems | ForEach-Object { Write-Host $_ }
}
else {
    Write-Host "All files already match the standard." -ForegroundColor Green
}
