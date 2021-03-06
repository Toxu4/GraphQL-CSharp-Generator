# Paths
$storedFolder = Get-Location
$packFolder = (Get-Item -Path "./nupkg" -Verbose).FullName
$slnPath = (Get-Item -Path "./src/cs/" -Verbose).FullName

# List of projects
$projects = (
    "Toxu4.GraphQl.Client"
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach ($project in $projects) {
    
    $projectFolder = Join-Path $slnPath $project

    # Create nuget pack
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet msbuild /t:pack /p:Configuration=Release /p:IncludeSymbols=true /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    Move-Item $projectPackPath $packFolder

}

# Go back to the stored
Set-Location $storedFolder
