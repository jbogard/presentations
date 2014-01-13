$roundhouse_version_file = ".\TechShare.Prosecutor.UI.dll"
$roundhouse_exe_path = ".\rh.exe"
$scripts_dir = ".\Databases\TechShare-Prosecutor"
$roundhouse_output_dir = ".\output"

if ($OctopusParameters) {$env = $OctopusParameters["Octopus.Environment.Name"]} else {$env="dev"}
$db_name = "TechShare-Prosecutor_" + $env.ToLower()


if (!$OctopusParameters) { $DBServer = ".\SqlExpress" }

if (!$env) {
	$env = "LOCAL"
	Write-Host "RoundhousE environment variable is not specified... defaulting to 'LOCAL'"
} else {
	Write-Host "Executing RoundhousE for environment:" $env
}


Write-Host "roundhouse_exe_path" $roundhouse_exe_path
Write-Host "DBServer" $DBServer
Write-Host "db_name" $db_name
Write-Host "env" $env
Write-Host "scripts_dir" $scripts_dir
Write-Host "roundhouse_output_dir" $roundhouse_output_dir


#&$roundhouse_exe_path -s $DBServer -d "$db_name" --env $env --silent -drop -o $roundhouse_output_dir
&$roundhouse_exe_path -s $DBServer -d $db_name -f $scripts_dir --env $env --silent -o $roundhouse_output_dir -vf $roundhouse_version_file
