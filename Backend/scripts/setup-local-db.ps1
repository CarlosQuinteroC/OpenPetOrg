<#
Setup script for local DB and migrations.

What this does:
- Initializes dotnet user-secrets for Backend/src/PetOrg.Api (if missing)
- Stores the ConnectionStrings:Default in user-secrets (safe for local dev)
- Ensures dotnet-ef tool is available (installs if missing)
- Runs EF migrations to update the target DB
- Optionally runs build and tests

Usage (from repo root):
  pwsh -File .\Backend\scripts\setup-local-db.ps1
  OR with custom connection string:
  pwsh -File .\Backend\scripts\setup-local-db.ps1 -ConnectionString "Host=localhost;Port=5432;Database=petorg;Username=petorg;Password=petorg"

Notes:
- This script uses the API project at Backend/src/PetOrg.Api
- It writes secrets to the current user's dotnet user-secrets store (local only)
- Do NOT commit secrets into source control
#>

param(
    [string]$ConnectionString = "Host=localhost;Port=5432;Database=petorg;Username=petorg;Password=petorg",
    [switch]$RunTests = $true
)

function Info($msg) { Write-Host "[INFO] $msg" -ForegroundColor Cyan }
function ErrorExit($msg) { Write-Host "[ERROR] $msg" -ForegroundColor Red; exit 1 }

$apiProjectPath = Join-Path $PSScriptRoot "..\src\PetOrg.Api"
if (-not (Test-Path $apiProjectPath)) { ErrorExit "API project path not found: $apiProjectPath" }

Info "Using API project: $apiProjectPath"

Push-Location $apiProjectPath
try {
    Info "Initializing dotnet user-secrets (if not already initialized)..."
    dotnet user-secrets init 2>$null | Out-Null

    Info "Setting ConnectionStrings:Default in user-secrets"
    dotnet user-secrets set "ConnectionStrings:Default" "$ConnectionString"
} catch {
    ErrorExit "Failed to set user-secrets. $_"
} finally {
    Pop-Location
}

Info "Ensuring dotnet-ef tool is available (global)..."
try {
    $toolList = & dotnet tool list -g 2>$null
    if ($toolList -notmatch "dotnet-ef") {
        Info "Installing dotnet-ef global tool..."
        try {
            & dotnet tool install --global dotnet-ef
            if ($LASTEXITCODE -ne 0) { Write-Host "dotnet-ef install returned non-zero; continuing..." -ForegroundColor Yellow }
        } catch {
            Write-Host "dotnet-ef install failed: $_" -ForegroundColor Yellow
        }
    } else {
        Info "dotnet-ef already present"
    }
} catch {
    Write-Host "Warning: could not verify/install dotnet-ef: $_" -ForegroundColor Yellow
}

Info "Running EF database update (migrations) against connection string stored in user-secrets..."
try {
    dotnet ef database update `
      --project ..\src\PetOrg.Infrastructure.Persistence\PetOrg.Infrastructure.Persistence.csproj `
      --startup-project ..\src\PetOrg.Api\PetOrg.Api.csproj
} catch {
    Write-Host "EF update failed: $_" -ForegroundColor Yellow
    Write-Host "Ensure .NET 9 runtime is installed and packages are restored." -ForegroundColor Yellow
}

Info "Building solution..."
& dotnet build ..\..\PetOrg.sln
if ($LASTEXITCODE -ne 0) { ErrorExit "Build failed" }

if ($RunTests) {
    Info "Running test suite..."
    & dotnet test ..\..\PetOrg.sln
    if ($LASTEXITCODE -ne 0) { ErrorExit "Tests failed" }
}

Info "Done. To start the API pointing to this DB, run (same session):"
Write-Host "  dotnet run --project .\Backend\src\PetOrg.Api" -ForegroundColor Green
Write-Host "Then check: http://localhost:5000/health and http://localhost:5000/swagger" -ForegroundColor Green

Exit 0
