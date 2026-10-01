param([string]$Runtime = "win-x64")
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root
$destination = "artifacts\Violet-Pulsar-$Runtime"
if (Test-Path $destination) { Remove-Item -LiteralPath $destination -Recurse -Force }
dotnet publish .\TaskManagerApp\TaskManagerApp\TaskManagerApp.csproj --configuration Release --runtime $Runtime --self-contained true --output $destination -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:IncludeAllContentForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed." }
Copy-Item README.md, LICENSE $destination
Copy-Item docs $destination -Recurse
$executable = Join-Path $destination "VioletPulsar.exe"
if (Test-Path $executable) {
    & $executable --diagnostics
    if ($LASTEXITCODE -ne 0) { throw "Packaged runtime diagnostic failed." }
}
Write-Output $destination
