Beyond right-click deploy

### Visual studio deploy
Right-click publish in VS
Pick all the options
kcdc2017 resource group
North-Central US
Lowest tier
Run app
Create database in portal
ContosoUniversityCore-KCDC2017
username: contosouniversitycore-kcdc2017
password: (pwd)
Tools -> Query editor
Run SchemaAndData.sql
Copy connection string
Put in Application Settings (Data:DefaultConnection:ConnectionString)
Make change to title, publish, don't commit

### Continuous deployment
Deployment -> Quickstart -> ASP.NET Core -> Cloud based source control
Deployments -> Setup -> Project (kcdc2017-cicd)
Sync....wait

### CI/CD pipeline
Add new file, appveyor.yml
```
version: 1.0.{build}
image: Visual Studio 2017
build_script:
- ps: .\Build.ps1
artifacts:
- path: artifacts\packages\*.nupkg
  name: nuget
test: off
```
ci.appveyor.com, add project
Run build, see failure
nuget install roundhouse
Copy rh.exe from .nuget into tools folder
Create App_data folder and runAfterCreateDatabase
Copy SchemaAndData.sql, rename to 0001_***
Build.ps1 add:
```
exec { & .\tools\rh.exe /d=ContosoUniversity /f=src\ContosoUniversityCore\App_Data /s="(LocalDb)\mssqllocaldb" /silent }
exec { & .\tools\rh.exe /d=ContosoUniversity-Test /f=src\ContosoUniversityCore\App_Data /s="(LocalDb)\mssqllocaldb" /silent /drop }
exec { & .\tools\rh.exe /d=ContosoUniversity-Test /f=src\ContosoUniversityCore\App_Data /s="(LocalDb)\mssqllocaldb" /silent /simple }
```
Remove the dotnet pack
Download octo.exe into tools https://octopus.com/downloads
To the build script:
```
exec { & dotnet publish src/ContosoUniversityCore --output .\..\..\publish --configuration Release }

$octo_revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = "0" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$octo_version = "1.0.$octo_revision"

exec { & .\tools\Octo.exe pack --id ContosoUniversityCore --version $octo_version --basePath publish --outFolder artifacts }
```
For the sql files in our .csproj:
```

  <ItemGroup>
    <None Include="App_Data\**\*" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>

```
Create project
Create step for azure web app
Disconnect
Create new release, deploy

### Resource templates

View resource

Download templates

Run them in powershell

Make a change and run again

Create step in octopus and run again

