#Invoke app in container
Write-Host "Running project in container ..."
Write-Host "`nWait for the next step...`n"
Start-Sleep -s 5
docker exec -it mcp_seriesgenerator_app bash -c "dotnet McpSeriesGenerator.App.dll &"

#List files for copy
Write-Host "`nList files in /artifacts/ from container ..."
Start-Sleep -s 5
docker exec -it mcp_seriesgenerator_app ls ./artifacts/

#Copy files...
Write-Host "`nCopy artifacts files from container to /artifacts/docker/ ..."
Start-Sleep -s 5
$destination = ".\artifacts\docker"
# Creates the destination folder if it does not exist
if (-not (Test-Path $destination)) {
    New-Item -ItemType Directory -Path $destination | Out-Null
}
docker cp mcp_seriesgenerator_app:/app/artifacts/. ./artifacts/docker

Write-Host "`nFinished. Press Enter to exit..."
Read-Host