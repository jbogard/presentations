# required parameters :
# 	$databaseName

$framework = '4.0x86'

properties {
	$projectName = "CodeCampServerLite"
	
	$base_dir = resolve-path .\
    $website_dir = "$base_dir\website"

	$databaseName = $env:database_name
	$databaseServer = "localhost\sqlexpress"
	$databaseScripts = "$base_dir\scripts"
	$hibernateConfig = "$website_dir\hibernate.cfg.xml"

    $remote_dir = $env:remote_dir
	
	$connection_string = "server=$databaseserver;database=$databasename;Integrated Security=true;"
}

task default -depends UpdateDatabase, DeployWebsite

task UpdateDatabase {
	write-host ("Database Server: " + $databaseServer) -ForegroundColor Green
	write-host ("Database Name: " + $databaseName) -ForegroundColor Green
	write-host ("Database Scripts: " + $databaseScripts) -ForegroundColor Green

    exec { 
		& $base_dir\dbmigrate\DatabaseDeployer.exe Update $databaseServer $databaseName $databaseScripts 
	}
}

task DeployWebsite {

    poke-xml $website_dir\bin\hibernate.cfg.xml "//*/hbm:property[@name='connection.connection_string']" $connection_string @{"hbm" = "urn:nhibernate-configuration-2.2"}

    exec {
        & xcopy $website_dir $remote_dir /e /y
    } 
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
