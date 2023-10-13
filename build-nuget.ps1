$scriptDir = split-path -parent $MyInvocation.MyCommand.Definition
$srcDir = (Join-Path -path $scriptDir .)
# nuget.exe needs to be on the path or aliased
function Reset-Templates{
    [cmdletbinding()]
    param(
        [string]$templateEngineUserDir = (join-path -Path $env:USERPROFILE -ChildPath .templateengine)
    )
    process{
        'resetting dotnet new templates. folder: "{0}"' -f $templateEngineUserDir | Write-host
        get-childitem -path $templateEngineUserDir -directory | Select-Object -ExpandProperty FullName | remove-item -recurse
        &dotnet new --debug:reinit
    }
}
function Clean(){
    [cmdletbinding()]
    param(
        [string]$rootFolder = $scriptDir
    )
    process{
        'clean started, rootFolder "{0}"' -f $rootFolder | write-host
        # delete folders that should not be included in the nuget package
        Get-ChildItem -path $rootFolder -include bin,obj,nupkg -Recurse -Directory | Select-Object -ExpandProperty FullName | Remove-item -recurse
    }
}



# start script
Clean

# create nuget package
$outputpath = Join-Path $scriptDir nupkg
$pathtonuspec = Join-Path $srcDir Shofy.nuspec
if(Test-Path $pathtonuspec){
    nuget.exe pack $pathtonuspec -OutputDirectory $outputpath
}
else{
    'ERROR: nuspec file not found at {0}' -f $pathtonuspec | Write-Error
    return
}

$pathtonupkg = join-path $scriptDir nupkg/Clean.Architecture.Cn.Template.7.0.1.nupkg
# install nuget package using dotnet new --install
if(test-path $pathtonupkg){
    Reset-Templates
	
	'uninstall template'
	&dotnet new uninstall Clean.Architecture.Cn.Template
	
    'installing template'
	&dotnet new install $pathtonupkg
}
else{
    'Not installing template because it was not found at "{0}"' -f $pathtonupkg | Write-Error
}