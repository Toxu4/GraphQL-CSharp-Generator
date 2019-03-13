# Paths
$packFolder = (Get-Item -Path "./nupkg" -Verbose).FullName
$slnPath = (Get-Item -Path "./src/cs/Toxu4.GraphQl.Client" -Verbose).FullName

Write-Host "sln", $slnPath
Write-Host "pack", $packFolder

dir *.*

$srcPath = $slnPath

# List of projects
$projects = (
    ""
)

# Rebuild solution
Set-Location $slnPath
& dotnet restore

# Copy all nuget packages to the pack folder
foreach ($project in $projects) {
    
    $projectFolder = Join-Path $srcPath $project

    # Create nuget pack
    Set-Location $projectFolder
    Remove-Item -Recurse (Join-Path $projectFolder "bin/Release")
    & dotnet msbuild /t:pack /p:Configuration=Release /p:IncludeSymbols=true /p:SourceLinkCreate=true

    # Copy nuget package
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $project + ".*.nupkg")
    
    Write-Host "From"
    Write-Host $projectPackPath
    Write-Host "To"
    Write-Host $packFolder
    
    Move-Item $projectPackPath $packFolder
}

# Go back to the pack folder
Set-Location $slnPath

dir *.*
