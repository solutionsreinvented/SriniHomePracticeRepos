param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$SolutionDir = $ScriptDir
$UIProject = Join-Path $SolutionDir "ProdActivity.UI\ProdActivity.UI.csproj"
$InstallerProject = Join-Path $SolutionDir "ProdActivity.Installer\ProdActivity.Installer.csproj"

$StagingDir = Join-Path $SolutionDir "artifacts\Staging"
$PublishDir = Join-Path $SolutionDir "artifacts\Publish"

if (Test-Path $StagingDir) { Remove-Item $StagingDir -Recurse -Force }
if (Test-Path $PublishDir) { Remove-Item $PublishDir -Recurse -Force }

New-Item -ItemType Directory -Force -Path $StagingDir | Out-Null
New-Item -ItemType Directory -Force -Path $PublishDir | Out-Null

Write-Host "Publishing ProdActivity.UI to Staging..." -ForegroundColor Cyan
dotnet publish $UIProject -c $Configuration -o $StagingDir

Write-Host "Building Installer..." -ForegroundColor Cyan
dotnet publish $InstallerProject -c $Configuration -o $PublishDir

Write-Host "Copying Payload to Installer Directory..." -ForegroundColor Cyan
$PayloadDir = Join-Path $PublishDir "Payload"
New-Item -ItemType Directory -Force -Path $PayloadDir | Out-Null
Copy-Item "$StagingDir\*" -Destination $PayloadDir -Recurse -Force

Write-Host "Installer package successfully built at: $PublishDir" -ForegroundColor Green
