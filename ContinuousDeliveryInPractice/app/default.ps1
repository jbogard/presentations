


$script:project_config = "Release"
properties {
	$project_name = "CodeCampServerLite"
	if(-not $version)
    {
        $version = "0.0.0.1"
    }

	$ReleaseNumber =  if ($env:BUILD_NUMBER) {"1.0.$env:BUILD_NUMBER.0"} else {$version}
	$OctopusEnvironment =  $env:OCTOPUS_ENVIRONMENT
	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$package_dir = "$build_dir\latestVersion"
	$source_dir = "$base_dir\Code"
	$test_dir = "$build_dir\test"
	$result_dir = "$build_dir\results"

    $db_server = if ($env:db_server) { $env:db_server } else { ".\SqlExpress" }
	
	$test_assembly_patterns_unit = @("*UnitTests.dll")
	$test_assembly_patterns_integration = @("*IntegrationTests.dll")
	$test_assembly_patterns_full = @("*FullSystemTests.dll")

	$cassini_exe = 'C:\Program Files (x86)\Common Files\microsoft shared\DevServer\11.0\WebDev.WebServer40.EXE'
	$port = 2627

    $db_name = if ($env:db_name) { $env:db_name } else { $project_name }
    $db_scripts_dir = "$source_dir\DatabaseMigration\Databases\$project_name"

    $roundhouse_dir = "$base_dir\tools\roundhouse"
    $roundhouse_output_dir = "$roundhouse_dir\output"
    $roundhouse_exe_path = "$roundhouse_dir\rh.exe"
    $roundhouse_local_backup_folder = "$base_dir\database_backups"
	
	$octopus_API_URL = if ($env:octopus_API_URL) { $env:octopus_API_URL } else { "http://jimmybogard-pc:8082/api" }
	$octopus_API_key = if ($env:octopus_API_key) { $env:octopus_API_key } else { "KUQOV8HXGQ8JCPXCTOWAJE8LZ0" } 
	$octopus_project = "CodeCampServerLite"
	$octopus_nuget_repo = "C:\Nugets"

    $all_database_info = @{
        "$db_name"="$db_scripts_dir"
    }
	
}

#These are aliases for other build tasks. They typically are named after the camelcase letters (rad = Rebuild All Databases)
#aliases should be all lowercase, conventionally
#please list all aliases in the help task
task default -depends InitialPrivateBuild
task dev -depends DeveloperBuild
task ci -depends IntegrationBuild
task uad -depends UpdateAllDatabases
task rad -depends RebuildAllDatabases
task tq -depends RunIntegrationTestsQuickly
task tt -depends RunIntegrationTestsThroughly
task unit -depends RunAllUnitTests

task help {
	Write-Help-Header
	Write-Help-Section-Header "Comprehensive Building"
	Write-Help-For-Alias "(default)" "Intended for first build or when you want a fresh, clean local copy"
	Write-Help-For-Alias "dev" "Optimized for local dev; Most noteably UPDATES databases instead of REBUILDING"
	Write-Help-For-Alias "ci" "Continuous Integration build (long and thorough) with packaging"
	Write-Help-Section-Header "Database Maintenance"
	Write-Help-For-Alias "uad" "Update All the Databases to the latest version (all db used for the app, that is)"
	Write-Help-For-Alias "rad" "Rebuild All the Databases to the latest version from scratch (useful while working on the schema)"
	Write-Help-Section-Header "Running Tests"
	Write-Help-For-Alias "unit" "All unit tests"
	Write-Help-For-Alias "tq" "Unit and Test Integration Quickly, aka, UPDATE databases before testing"
	Write-Help-For-Alias "tt" "Unit and Test Integration Thoroughly, aka, REBUILD databases before testing (useful while working on the schema)"
	Write-Help-Footer
	exit 0
}

#These are the actual build tasks. They should be Pascal case by convention
task InitialPrivateBuild -depends Clean, Compile, RebuildAllDatabases, RunAllUnitTests, RunIntegrationTestsThroughly , WarnSlowBuild

task DeveloperBuild -depends SetDebugBuild, Clean, Compile, UpdateAllDatabases, RunAllUnitTests, RunIntegrationTestsQuickly

task IntegrationBuild -depends SetReleaseBuild, CommonAssemblyInfo, Clean, Compile, RebuildAllDatabases, RunAllUnitTests, RunIntegrationTestsThroughly, GenerateNugetPackage

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task RebuildAllDatabases {
    $all_database_info.GetEnumerator() | %{ 
		Write-Host $_.Key
		deploy-database "Rebuild" $db_server $_.Key $_.Value
	}
}

task UpdateAllDatabases {
    $all_database_info.GetEnumerator() | %{ deploy-database "Update" $db_server $_.Key $_.Value}
}

task RebuildAllTestDatabases {
    $all_database_info.GetEnumerator() | %{deploy-database "Rebuild" $db_server ($_.Key + "_Test") $_.Value}
}

task UpdateAllTestDatabases {
    $all_database_info.GetEnumerator() | %{deploy-database "Update" $db_server ($_.Key + "_Test") $_.Value}
}

task RebuildAllComparisonDatabases {
    $all_database_info.GetEnumerator() | %{ deploy-database "Rebuild" $db_server ($_.Key + "_Comp") $_.Value}
}

task CommonAssemblyInfo {
    create-commonAssemblyInfo "$ReleaseNumber" $project_name "$source_dir\CommonAssemblyInfo.cs"
}

task CopyAssembliesForTest -Depends Compile {
    copy_all_assemblies_for_test $test_dir
}

task RunIntegrationTestsThroughly -Depends CopyAssembliesForTest, RebuildAllTestDatabases {
    $test_assembly_patterns_integration | %{ run_tests $_ }
}

task RunIntegrationTestsQuickly -Depends CopyAssembliesForTest, UpdateAllTestDatabases {
    $test_assembly_patterns_integration | %{ run_tests $_ }
}

task RunAllUnitTests -Depends CopyAssembliesForTest {
    $test_assembly_patterns_unit | %{ run_tests $_ }
}

task Compile -depends Clean, CommonAssemblyInfo { 
    exec { & $source_dir\.nuget\nuget.exe restore $source_dir\$project_name.sln }
    exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /nologo $source_dir\$project_name.sln }
}

task Clean {
    delete_file $package_file
    delete_directory $build_dir
    create_directory $test_dir 
    create_directory $result_dir
	
    exec { msbuild /t:clean /v:q /p:Configuration=$project_config $source_dir\$project_name.sln }
}

task WarnSlowBuild {
	Write-Host ""
	Write-Host "Warning: " -foregroundcolor Yellow -nonewline;
	Write-Host "The default build you just ran is primarily intended for initial "
	Write-Host "environment setup. While developing you most likely want the quicker dev"
	Write-Host "build task. For a full list of common build tasks, run: "
	Write-Host " > build.bat help"
}

task GenerateNugetPackage{
    exec { msbuild.exe $source_dir\$project_name.sln /t:build /p:RunOctoPack=true /v:q /p:Configuration=$project_config /nologo /p:OctoPackPackageVersion=$ReleaseNumber /p:OctoPackPublishPackageToFileShare=$package_dir }
}
# -------------------------------------------------------------------------------------------------------------
# generalized functions added by Headspring for Help Section
# --------------------------------------------------------------------------------------------------------------

function Write-Help-Header($description) {
	Write-Host ""
	Write-Host "********************************" -foregroundcolor DarkGreen -nonewline;
	Write-Host " HELP " -foregroundcolor Green  -nonewline; 
	Write-Host "********************************"  -foregroundcolor DarkGreen
	Write-Host ""
	Write-Host "This build script has the following common build " -nonewline;
	Write-Host "task " -foregroundcolor Green -nonewline;
	Write-Host "aliases set up:"
}

function Write-Help-Footer($description) {
	Write-Host ""
	Write-Host " For a complete list of build tasks, view default.ps1."
	Write-Host ""
	Write-Host "**********************************************************************" -foregroundcolor DarkGreen
}

function Write-Help-Section-Header($description) {
	Write-Host ""
	Write-Host " $description" -foregroundcolor DarkGreen
}

function Write-Help-For-Alias($alias,$description) {
	Write-Host "  > " -nonewline;
	Write-Host "$alias" -foregroundcolor Green -nonewline; 
	Write-Host " = " -nonewline; 
	Write-Host "$description"
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------
function deploy-database($action,$server,$db_name,$scripts_dir,$env) {
    $roundhouse_version_file = "$source_dir\UI\bin\CodeCampServerLite.UI.dll"

    if (!$env) {
        $env = "LOCAL"
        Write-Host "RoundhousE environment variable is not specified... defaulting to 'LOCAL'"
    } else {
        Write-Host "Executing RoundhousE for environment:" $env
    }
        
    if ($action -eq "Update"){
        exec { &$roundhouse_exe_path -s $server -d "$db_name"  --commandtimeout=300 -f $scripts_dir --env $env --silent -o $roundhouse_output_dir }
    }
    if ($action -eq "Rebuild"){
        exec { &$roundhouse_exe_path -s $server -d "$db_name" --commandtimeout=300 --env $env --silent -drop -o $roundhouse_output_dir }
        exec { &$roundhouse_exe_path -s $server -d "$db_name" --commandtimeout=300 -f $scripts_dir -env $env -vf $roundhouse_version_file --silent --simple -o $roundhouse_output_dir }
    }
}

function run_tests([string]$pattern) {
    
    $items = Get-ChildItem -Path $test_dir $pattern
    $items | %{ run_xunit $_.Name }
}

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name) {
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create_directory($directory_name) {
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:run_xunit ($test_assembly) {
	$assembly_to_test = $test_dir + "\" + $test_assembly
	$results_output = $result_dir + "\" + $test_assembly + ".xml"
    write-host "Running XUnit Tests in: " $test_assembly
    exec { & tools\xunit\xunit.console.clr4.exe $assembly_to_test /silent /nunit $results_output }
}

function global:Copy_and_flatten ($source,$include,$dest) {
	Get-ChildItem $source -include $include -r | cp -dest $dest
}

function global:copy_all_assemblies_for_test($destination){
	$bin_dir_match_pattern = "$source_dir\**\bin\$project_config"
	create_directory $destination
	Copy_and_flatten $bin_dir_match_pattern @("*.exe","*.dll","*.config","*.pdb","*.sql","*.xlsx","*.csv","*.xml") $destination
}

function global:create-commonAssemblyInfo($version,$applicationName,$filename) {
"using System.Reflection;

// Version information for an assembly consists of the following four values:
//
//      Year                    (Expressed as YYYY)
//      Major Release           (i.e. New Project / Namespace added to Solution or New File / Class added to Project)
//      Minor Release           (i.e. Fixes or Feature changes)
//      Build Date & Revsion    (Expressed as MMDD)
//
[assembly: AssemblyCompany(""Jimmy Bogard"")]
[assembly: AssemblyCopyright(""Copyright © Jimmy Bogard 2014"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]" | out-file $filename -encoding "utf8"
}


