# Define the project path
$projectPath = "./HexAndReplace.csproj"

# Publish for win-x64 with AOT and single file compression
dotnet publish $projectPath `
    -c Release `
    -r win-x64 `
    /p:PublishAot=true `
    /p:SelfContained=true `
    -o "./bin/release/net8.0/publish/win-x64"

# delete pdb files
Remove-Item -Path "./bin/release/net8.0/publish/win-x64/*.pdb"

# zip the output exe with max compression
Compress-Archive -Path "./bin/release/net8.0/publish/win-x64/HexAndReplace.exe" -DestinationPath "./bin/release/net8.0/publish/win-x64/HexAndReplace.zip" -CompressionLevel Optimal -Force

# Publish for linux-x64 with single file compression
dotnet publish $projectPath `
    -c Release `
    -r linux-x64 `
    /p:PublishSingleFile=true `
    /p:SelfContained=true `
    -o "./bin/release/net8.0/publish/linux-x64"

# delete pdb files
Remove-Item -Path "./bin/release/net8.0/publish/linux-x64/*.pdb"

# zip the output exe with max compression
Compress-Archive -Path "./bin/release/net8.0/publish/linux-x64/HexAndReplace" -DestinationPath "./bin/release/net8.0/publish/linux-x64/HexAndReplace.zip" -CompressionLevel Optimal -Force

Write-Host "Publishing completed."