﻿# 
# File: ChocolateyUninstall.ps1
# 
# Author: Akira Sugiura (urasandesu@gmail.com)
# 
# 
# Copyright (c) 2015 Akira Sugiura
#  
#  This software is MIT License.
#  
#  Permission is hereby granted, free of charge, to any person obtaining a copy
#  of this software and associated documentation files (the "Software"), to deal
#  in the Software without restriction, including without limitation the rights
#  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
#  copies of the Software, and to permit persons to whom the Software is
#  furnished to do so, subject to the following conditions:
#  
#  The above copyright notice and this permission notice shall be included in
#  all copies or substantial portions of the Software.
#  
#  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
#  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
#  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
#  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
#  THE SOFTWARE.
#


$chocoToolsPath = [IO.Path]::Combine($env:chocolateyPackageFolder, 'tools')


'  Uninstalling Prig.vsix...'
$vscomntoolsPaths = gci env:vs* | ? { $_.name -match 'VS\d{3}COMNTOOLS' } | sort name -Descending | % { $_.value }
if (0 -eq $vscomntoolsPaths.Length) {
    Write-Error "'VsDevCmd.bat' is not found."
    exit 1805378044
}
$vsDevCmdPath = [System.IO.Path]::Combine($vscomntoolsPaths[0], 'VsDevCmd.bat')
if (![System.IO.File]::Exists($vsDevCmdPath)) {
    Write-Error "'VsDevCmd.bat' is not found."
    exit 1805378044
}
cmd /c ('" "{0}" & vsixinstaller /q /u:0a06101d-8de3-40c4-b083-c5c16ca227ae "' -f $vsDevCmdPath)


$prig = [IO.Path]::Combine($chocoToolsPath, 'prig.exe')
$packageName = "All"
"  Uninstalling the all package..."
& $prig uninstall $packageName


$profilers = [System.IO.Path]::Combine($env:chocolateyPackageFolder, 'tools\x64\Urasandesu.Prig.dll'), 
             [System.IO.Path]::Combine($env:chocolateyPackageFolder, 'tools\x86\Urasandesu.Prig.dll')
foreach ($profiler in $profilers) {
    "  Unregistering the profiler '$profiler' from Registry..."
    regsvr32 /s /u $profiler
}


$variableName = "URASANDESU_PRIG_PACKAGE_FOLDER"
"  Unregistering the environment variable '$variableName'..."
[System.Environment]::SetEnvironmentVariable($variableName, $null, 'User')


$name = "Prig Source"
"  Unregistering the nuget source '$name'..."
if (0 -lt @(nuget sources list | ? { $_ -match 'Prig Source' }).Length) {
    nuget sources remove -name $name
}
