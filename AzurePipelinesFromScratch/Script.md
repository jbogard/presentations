Azure Pipelines from Scratch

### CI/CD pipeline
Add new file, azure-pipelines.yml
```
name: 1.0.$(BuildID).0
queue:
  name: Hosted VS2017

trigger:
- master

steps:

- task: PowerShell@2
  inputs:
    targetType: filePath
    filePath: '.\Build.ps1'
  displayName: Run Full Build
```
 - dev.azure.com
 - Run build, see failure
 - Add steps to create localdb
```
- task: CmdLine@1
  inputs:
    filename: sqllocaldb
    arguments: 'create mssqllocaldb'
  displayName: Create SQL LocalDB Instance
```
 - Run build
 - Show 
 - Add test results
```
- task: PublishTestResults@2
  condition: succeededOrFailed()
  inputs:
    testRunner: VSTest
    testResultsFiles: '**/*.trx'
```
 - Run build

 - Add artifacts
```
- task: PublishBuildArtifacts@1
  inputs:
    PathToPublish: '.\publish'
    ArtifactName: 'ContosoUniversity'
    ArtifactType: Container
  condition: and(succeeded(), eq(variables['Build.SourceBranchName'], 'master'))
```
 

 - Create release pipeline
 - Create artifact
 - Create step for azure web app
 - Set Package
 - Set Continuous Deploy trigger
 - Deploy

NOW WE NEED DB MIGRATIONS
 - Add new field to Student
 - Create migration script
 - Run local build
 - Edit csproj
```
  <ItemGroup>
    <Content Include="App_Data\**\*" CopyToPublishDirectory="Always" />
  </ItemGroup>

  <Target Name="PostpublishScript" AfterTargets="Publish">
    <Exec Command="xcopy $(ProjectDir)..\tools\rh.exe $(PublishDir)App_Data\ /Y" />
  </Target>

```

 - Create step for powershell
```
$roundhouse_exe_path = ".\App_Data\rh.exe"
$scripts_dir = ".\App_Data"
&$roundhouse_exe_path -c "$(ConnectionStrings.DefaultConnection)" -f $scripts_dir --env $(RoundhousE.ENV) --silent -o ".\App_Data\output"
```
 - Create variables
```
$(ConnectionStrings.DefaultConnection)
$(RoundhousE.ENV)
```
 - Grab connection string from azure portal
 - DEV and PROD
 

