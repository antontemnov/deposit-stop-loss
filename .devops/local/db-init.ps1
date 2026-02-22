#Requires -Version 7.0
<#
.SYNOPSIS
    Clean-slate database initialization for local development.
.DESCRIPTION
    Destroys existing container + volume, starts fresh PostgreSQL,
    and applies all EF Core migrations. Npgsql EnableRetryOnFailure
    handles waiting for PostgreSQL readiness automatically.
.EXAMPLE
    .\.devops\local\db-init.ps1
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = git rev-parse --show-toplevel
$composeFile = Join-Path $repoRoot '.devops\local\compose.yaml'
$backendDir = Join-Path $repoRoot 'backend'
$infraProject = Join-Path $backendDir 'src\DepositStopLoss.Infrastructure'
$startupProject = Join-Path $backendDir 'src\DepositStopLoss.Api'

# --- Step 1: Tear down previous container + volume ---
Write-Host "`n[1/3] Tearing down existing container and volume..." -ForegroundColor Cyan
docker compose -f $composeFile down -v 2>&1 | Out-Null
Write-Host "  Done." -ForegroundColor Green

# --- Step 2: Start fresh PostgreSQL ---
Write-Host "`n[2/3] Starting PostgreSQL container..." -ForegroundColor Cyan
docker compose -f $composeFile up -d
Write-Host "  Container started. EF Core will retry until PostgreSQL is ready." -ForegroundColor Green

# --- Step 3: Apply EF Core migrations ---
Write-Host "`n[3/3] Applying EF Core migrations..." -ForegroundColor Cyan
Push-Location $backendDir
dotnet ef database update `
    --project $infraProject `
    --startup-project $startupProject
Pop-Location

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nMigration failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "  Database ready: deposit_stop_loss" -ForegroundColor Green
Write-Host "  Host: localhost:5432" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Green
