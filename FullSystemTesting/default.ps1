# required parameters :
# 	$databaseName

$framework = '4.0x86'

properties {
	$projectName = "CodeCampServerLite"
	
	$unitTestAssembly = "$projectName.UnitTests.dll"
	$integrationTestAssembly = "$projectName.IntegrationTests.dll"
	
	$projectConfig = "release"
	
	$base_dir = resolve-path .\
	$source_dir = "$base_dir\Code"
	
	$build_dir = "$base_dir\build"
	$test_dir = "$build_dir\net-$framework-$projectConfig"
	$lib_dir = "$base_dir\lib"
	$results_dir = "$build_dir\results"
	$package_dir = "$build_dir\package"	
	$package_file = "$base_dir\latestVersion\$projectName`Package.exe"
	
	$nunitPath = "$lib_dir\nunit"
	
	$databaseName = $projectName
	$databaseServer = "localhost\sqlexpress"
	$databaseScripts = "$source_dir\Infrastructure\Database"
	$hibernateConfig = "$source_dir\hibernate.cfg.xml"
	$schemaDatabaseName = $databaseName + "_schema"
	
	$connection_string = "server=$databaseserver;database=$databasename;Integrated Security=true;"
	
	$cassini_app = 'C:\Program Files (x86)\Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer40.EXE'
	$port = 1234
        
    $webApplicationsToPackage =
    (
        @{Source = "UI"; Destination = "website"}
    )
    
	$nunitExe = "$lib_dir\NUnit-2.5.10.11092\bin2\net-2.0\nunit-console-x86.exe"
}

task default -depends Init, CommonAssemblyInfo, RebuildDatabase, Compile, CopyForTest, UnitTest, IntegrationTest, DataLoader
task ci -depends Init, CommonAssemblyInfo, RebuildDatabase, Compile, CopyForTest, UnitTest, IntegrationTest
task testdata -depends Init, CommonAssemblyInfo, RebuildDatabase, Compile, CopyForTest, DataLoader

FormatTaskName (("*"*25) + " {0, -25} " + ("*"*25))

task Init {
    #delete_file $package_file
    delete_directory $build_dir
    create_directory $test_dir
    create_directory $results_dir
	create_directory $build_dir
}

task CommonAssemblyInfo {
    $version = (Get-Date).ToString("yyyy.MM.dd.HHmm")
		
    create-commonAssemblyInfo "$version" $projectName "$source_dir\CommonAssemblyInfo.cs"
}

task RebuildDatabase {
	write-host ("Database Server: " + $databaseServer) -ForegroundColor Green
	write-host ("Database Name: " + $databaseName) -ForegroundColor Green
	write-host ("Database Scripts: " + $databaseScripts) -ForegroundColor Green

    exec { 
		& $lib_dir\tarantinodbmigrate\DatabaseDeployer.exe Rebuild $databaseServer $databaseName $databaseScripts 
	}
}

task Compile -depends Init {
	write-host ("Config: " + $projectConfig) -ForegroundColor Green
	write-host ("Clean Project: " + $source_dir + "\" + $projectName + ".sln") -ForegroundColor Green
	write-host ("Build Project: " + $source_dir + "\" + $projectName + ".sln") -ForegroundColor Green

    exec { msbuild /t:clean /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln }
    exec { msbuild /t:build /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln }
}

task CopyForTest -depends Compile {
	write-host ("Copy Assembiles: " + $test_dir) -ForegroundColor Green
	
	create_directory $test_dir
	
    $excludeFolders = [string[]][object[]] "_*", "Api", "UI", "packages"
    copy_and_flatten_library_projects $source_dir $test_dir $excludeFolders $projectConfig

    $webFolders = [string[]][object[]] "UI"
    copy_and_flatten_web_projects $source_dir $test_dir $webFolders
}

task UnitTest -depends CopyForTest {
	write-host ("Unit Tests: " + $test_dir + "\" + $unitTestAssembly) -ForegroundColor Green
	write-host ("Unit Tests Result File: " + $results_dir + "\" + $unitTestAssembly + ".xml") -ForegroundColor Green

	exec {
		& $nunitExe $test_dir\$unitTestAssembly /noshadow /nologo /nodots /xml=$results_dir\$unitTestAssembly.xml
	}
}

task IntegrationTest -depends CopyForTest {
	write-host ("Integration Tests: " + $test_dir + "\" + $integrationTestAssembly) -ForegroundColor Green
	write-host ("Integration Tests Result File: " + $results_dir + "\" + $integrationTestAssembly + ".xml") -ForegroundColor Green

	exec {
		& $nunitExe $test_dir\$integrationTestAssembly /exclude=DataLoader /noshadow  /nologo /nodots /xml=$results_dir\$integrationTestAssembly.xml
	}

}

task DataLoader -depends CopyForTest {
	exec {
		& $nunitExe $test_dir\$integrationTestAssembly /include=DataLoader /noshadow  /nologo /nodots
	}
}

task Package -depends Compile {
# -depends Compile

    delete_directory $package_dir
    
    foreach($application in $webApplicationsToPackage)
    {    
        $source = $application.Source
        $dest = $application.Destination
        copy_website_files "$source_dir\$source" "$package_dir\$dest"
    }
        
    copy_files "$lib_dir\nant" "$package_dir\nant" ("*.pdb", "*.xml")
    copy_files "$lib_dir\tarantinodbmigrate" "$package_dir\dbmigrate"
	
	zip_directory $package_dir $package_file 
}

task dbmigration -depends CopyForTest {
    delete_file "$databaseScripts\_New_Script.sql"
	exec { & $nunitExe $test_dir\$integrationTestAssembly /include=SchemaExport /noshadow  /nologo /nodots }
}

#************************************************************************

function global:copy_website_files($source,$destination){
    Write-Host "Copy Website Files $source to $destination"
    
    $exclude = @('*.user','*.dtd','*.tt','*.cs','*.csproj','*.orig', '*.log') 
    copy_files $source $destination $exclude $false
	delete_directory "$destination\obj"
}

function global:copy_files($source, $destination, $exclude=@(), $includeEmptyDirs=$true){
    Write-Host "Copy Files $source to $destination"

    create_directory $destination
    $childItems = Get-ChildItem $source -Recurse -Exclude $exclude
    
    foreach($childItem in $childItems)
    {
        if (($childItem.psIsContainer -eq $true) -and ($includeEmptyDirs -eq $false))
        {
            $subItems = Get-ChildItem $childItem.FullName -Recurse -Exclude $exclude | Where { $_.psIsContainer -eq $false }
                        
            if ($subItems -eq $null)
            {
                continue
            }
        }
        
        $dest =  Join-Path $destination $childItem.FullName.Substring($source.length)
        Copy-Item $childItem.FullName -Destination $dest
    }
    
}

function global:copy_files_with_include($source,$destination, [string[]] $include){
    Write-Host "Copy Files $source to $destination"

    create_directory $destination
    Get-ChildItem $source -Recurse -Include $include | Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:copy_and_flatten_library_projects([string]$sourceDir, [string]$destination, [string[]]$excludeFromSource, [string]$projectConfig)
{
	$targetSrcDirs = Get-ChildItem -Path $sourceDir -Exclude $excludeFromSource | Where { $_.psIsContainer -eq $true }
	
	foreach($targetSrcDir in $targetSrcDirs)
	{
		Write-Host "Copy and Flatten Library $targetSrcDir\bin\$projectConfig\* to $destination"
	
		Get-ChildItem -Path "$targetSrcDir\bin\$projectConfig\*" | Where { $_.psIsContainer -eq $false } | Copy-Item -dest $destination -force
	}
}
function global:copy_and_flatten_web_projects([string]$sourceDir, [string]$destination, [string[]]$webFolders)
{
    foreach($webFolder in $webFolders)
    {
    	Write-Host "Copy and Flatten Web $source_dir\$webFolder\bin\* to $destination"
    	Get-ChildItem -Path "$source_dir\$webFolder\bin\*" | Copy-Item -dest $destination -force
    }
}

function global:copy_and_flatten_with_include([string]$sourceDir, [string]$destination, [string[]]$include)
{
    foreach($webFolder in $webFolders)
    {
    	Write-Host "Copy and Flatten With Include $source_dir to $destination include $include"
    	Get-ChildItem -Path "$sourceDir" -Recurse -Include $include | Copy-Item -dest $destination -force
    }
}

function global:delete_directory($directory_name)
{
  write-host ("Deleting Directory: " + $directory_name) -ForegroundColor Green
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create_directory($directory_name)
{
  write-host ("Creating Directory: " + $directory_name) -ForegroundColor Green
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:create-commonAssemblyInfo($version,$applicationName,$filename)
{
  write-host ("Version: " + $version) -ForegroundColor Green
  write-host ("Project Name: " + $projectName) -ForegroundColor Green

"using System;
using System.Reflection;
using System.Runtime.InteropServices;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4927
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyCopyrightAttribute(""Copyright 2011"")]
[assembly: AssemblyProductAttribute(""$projectName"")]
[assembly: AssemblyCompanyAttribute("""")]
[assembly: AssemblyConfigurationAttribute(""release"")]
[assembly: AssemblyInformationalVersionAttribute(""$version"")]"  | out-file $filename -encoding "ASCII"    
}

function global:zip_directory($directory, $file) {
    write-host "Zipping folder: " $directory
    delete_file $file
    cd $directory
    & "$lib_dir\7zip\7za.exe" a -r -sfx $file
    cd $base_dir
}

function script:poke-xml($filePath, $xpath, $value, $namespaces = @{}) {
    [xml] $fileXml = Get-Content $filePath
    
    if($namespaces -ne $null -and $namespaces.Count -gt 0) {
        $ns = New-Object Xml.XmlNamespaceManager $fileXml.NameTable
        $namespaces.GetEnumerator() | %{ $ns.AddNamespace($_.Key,$_.Value) }
        $node = $fileXml.SelectSingleNode($xpath,$ns)
    } else {
        $node = $fileXml.SelectSingleNode($xpath)
    }
    
    Assert ($node -ne $null) "could not find node @ $xpath"
        
    if($node.NodeType -eq "Element") {
        $node.InnerText = $value
    } else {
        $node.Value = $value
    }

    $fileXml.Save($filePath) 
} 

function global:delete_file($file) {
    if(Test-Path $file)
    {
       write-host ("Deleting File: " + $file) -ForegroundColor Green

        remove-item $file -force -ErrorAction SilentlyContinue | out-null
    } 
}
