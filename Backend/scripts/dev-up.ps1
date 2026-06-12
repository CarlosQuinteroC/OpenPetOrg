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
Write-Host "Using stable EF Core 9 + Npgsql 9 provider on net9." -ForegroundColor Yellow
Write-Host "If migrations fail, validate ConnectionStrings:Default and ensure PostgreSQL is reachable." -ForegroundColor Yellow
