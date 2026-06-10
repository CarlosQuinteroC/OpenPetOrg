param(
    [string]$ContainerName = "petorg-postgres",
    [string]$Database = "petorg",
    [string]$Username = "petorg",
    [string]$Password = "petorg"
)

$port = 5432

docker rm -f $ContainerName 2>$null | Out-Null

docker run --name $ContainerName `
  -e POSTGRES_DB=$Database `
  -e POSTGRES_USER=$Username `
  -e POSTGRES_PASSWORD=$Password `
  -p ${port}:5432 `
  -d postgres:16-alpine | Out-Null

Write-Host "PostgreSQL container '$ContainerName' started on port $port." -ForegroundColor Green
Write-Host "Connection string: Host=localhost;Port=$port;Database=$Database;Username=$Username;Password=$Password" -ForegroundColor Yellow
Write-Host ""
Write-Host "Note: Npgsql EF Core provider for net10 is currently preview-only (11.0.0-preview.1)." -ForegroundColor Yellow
Write-Host "Current scaffold keeps a provider-agnostic migration placeholder until stable provider support is selected." -ForegroundColor Yellow
