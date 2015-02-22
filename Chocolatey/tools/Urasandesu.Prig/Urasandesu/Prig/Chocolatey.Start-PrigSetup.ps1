﻿# 
# File: Chocolatey.Start-PrigSetup.ps1
# 
# Author: Akira Sugiura (urasandesu@gmail.com)
# 
# 
# Copyright (c) 2012 Akira Sugiura
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



function Start-PrigSetup {
<#
    .SYNOPSIS
        Starts the Prig setup session.

    .DESCRIPTION
        The Prig setup session is a special PowerShell session to add the indirection stub settings and analyze target assemblies. In this session, you can access all information of the assemblies that are referenced from current project through the global variable `$ReferencedAssemblies`.

    .PARAMETER  NoIntro
        Omits the introduction help that is displayed when startup the session.

    .EXAMPLE
        PM> Start-PrigSetup
        
        ------- (The following commands are executed in new PowerShell window)
        Welcome to Prig setup session!!
        
        
        You can add the indirection settings from here. In this session, you can use `$ReferencedAssemblies` that contains all referenced assemblies information of current project. For example, if you want to get the indirection settings for all members of the type `Foo` that belongs to the referenced assembly `UntestableLibrary`, the following commands will achieve it:
        
        PS> $ReferencedAssemblies
        
        FullName
        --------
        mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        MyLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
        UntestableLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
        
        
        PS> padd -ra $ReferencedAssemblies[-1]
        PS> $ReferencedAssemblies[-1].GetTypes() | ? { $_.Name -eq 'Foo' } | pfind | pget | clip
        PS> exit   # Then, paste the content on the clipboard to the stub setting file(e.g. `UntestableLibrary.v4.0.30319.v1.0.0.0.prig`).
        
        
        
        See also the command's help `padd`, `pfind` and `pget`.
        
        
        
        Current Project: MyLibraryTest
        WARNING: Change the Current Project from `Default Project: ` on the Package Manager Console if it isn't what you want.
        
        


        DESCRIPTION
        -----------
        In this example, start the Prig setup session from the Package Manager Console. When the Prig setup session is started, new PowerShell window is opened with the introduction help like the above.

    .EXAMPLE
        PM> pstart -NoIntro
        
        ------- (The following commands are executed in new PowerShell window)
        Current Project: MyLibraryTest
        WARNING: Change the Current Project from `Default Project: ` on the Package Manager Console if it isn't what you want.
        
        
        PS> $ReferencedAssemblies
        
        FullName
        --------
        mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
        System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Data.DataSetExtensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
        System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        
        
        PS> $ReferencedAssemblies | ? { $_.FullName -cmatch 'Con' }
        
        FullName
        --------
        System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
        
        
        PS> padd -ra ($ReferencedAssemblies | ? { $_.FullName -cmatch 'Con' })
        PS>


        DESCRIPTION
        -----------
        In this example, start the Prig setup session without the introduction help when opened. Then, confirm the assemblies that are referenced from current project, choose `System.Configuration` from those assemblies, and add the indirection stub setting.

    .INPUTS
        None

    .OUTPUTS
        None

    .NOTES
        You can also refer to the Start-PrigSetup command by its built-in alias, "PStart".

    .LINK
        Add-PrigAssembly

    .LINK
        Find-IndirectionTarget

    .LINK
        Get-IndirectionStubSetting

    .LINK
        Invoke-Prig

#>

    [CmdletBinding()]
    param (
        [switch]
        $NoIntro, 

        [string]
        $AdditionalInclude, 

        [string]
        $EditorialInclude, 

        $Project
    )

    $envProj = $(if ($null -eq $Project) { (Get-Project).Object.Project } else { $Project.Object.Project })
    if (!$envProj.Saved) {
        $envProj.Save($envProj.FullName)
    }

    [void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.Build')
    $msbProjCollection = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection
    $allMsbProjs = $msbProjCollection.GetLoadedProjects($envProj.FullName).GetEnumerator()
    if(!$allMsbProjs.MoveNext()) {
        throw New-Object System.InvalidOperationException ('"{0}" has not been loaded.' -f $envProj.FullName)
    }

    $curMsbProj = $allMsbProjs.Current
    
    $toolsPath = [IO.Path]::Combine($env:URASANDESU_PRIG_PACKAGE_FOLDER, 'tools')
    $importPrigSetupSession = [IO.Path]::Combine($toolsPath, 'Import-PrigSetupSession.ps1')

    $os = Get-WmiObject Win32_OperatingSystem
    [System.Reflection.ProcessorArchitecture]$osArch = 0
    $osArch = $(if ($os.OSArchitecture -match '64') { 'Amd64' } else { 'X86' })
    Write-Verbose ('OS Architecture: {0}' -f $osArch)

    $projPlatform = $curMsbProj.ExpandString('$(Platform)')
    if ($projPlatform -eq 'AnyCPU') {
        $projPlatform += ('|{0}' -f $curMsbProj.ExpandString('$(Prefer32Bit)'))
    }
    [System.Reflection.ProcessorArchitecture]$projArch = 0
    $projArch = ConvertTo-ProcessorArchitectureString $projPlatform
    Write-Verbose ('Project Architecture: {0}' -f $projArch)

    [System.Reflection.ProcessorArchitecture]$arch = 0
    $arch = $(if ($projArch -eq 'MSIL') { $osArch } else { $projArch })
    Write-Verbose ('Result Architecture: {0}' -f $arch)

    $powershell = $(if ($arch -eq 'Amd64') { '%windir%\SysNative\WindowsPowerShell\v1.0\powershell.exe' } else { '%windir%\system32\WindowsPowerShell\v1.0\powershell.exe' })
    $powershell = [System.Environment]::ExpandEnvironmentVariables($powershell)
    Write-Verbose ('PowerShell: {0}' -f $powershell)

    $outPath = $curMsbProj.ExpandString('$(OutputPath)')
    Write-Verbose ('OutputPath: {0}' -f $outPath)

    $refIncludes = New-Object 'System.Collections.Generic.List[string]' 
    $refHints =  New-Object 'System.Collections.Generic.List[string]'
    $refs = 
        $curMsbProj.GetItems('Reference') | 
            select @{ Name = "Include"; Expression = { $_.EvaluatedInclude } }, 
                   @{ Name = "HintPath"; Expression = { @($_.Metadata | ? { $_.Name -eq 'HintPath' })[0].EvaluatedValue }}
    foreach ($ref in $refs) {
        $refIncludes.Add($ref.Include)
        $refHints.Add($ref.HintPath)
    }
    $refInclude = $refIncludes -join ';'
    $refHint =  $refHints -join ';'
    Write-Verbose ('ReferenceInclude: {0}' -f $refInclude)
    Write-Verbose ('ReferenceHintPath: {0}' -f $refHint)

    $projRefIncludes = New-Object 'System.Collections.Generic.List[string]' 
    $projRefs = 
        $curMsbProj.GetItems('ProjectReference') | 
            select @{ Name = "Include"; Expression = { $_.EvaluatedInclude } }
    foreach ($projRef in $projRefs) {
        $projRefFullName = Resolve-Path ([System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($envProj.FullName), $projRef.Include))
        $allMsbProjs = $msbProjCollection.GetLoadedProjects($projRefFullName).GetEnumerator()
        if(!$allMsbProjs.MoveNext()) {
            throw New-Object System.InvalidOperationException ('"{0}" has not been loaded.' -f $projRefFullName)
        }

        $refMsbProj = $allMsbProjs.Current
        $projRefIncludes.Add($refMsbProj.ExpandString('$(TargetPath)'))
    }
    $projRefInclude = $projRefIncludes -join ';'
    Write-Verbose ('ProjectReferenceInclude: {0}' -f $projRefInclude)

    $targetFrmwrkVer = $curMsbProj.ExpandString('$(TargetFrameworkVersion)')
    $argList = '-NoLogo', '-File', """$importPrigSetupSession""", """$($envProj.Name)""", """$($envProj.FullName)""", """$targetFrmwrkVer""", """$outPath.""", """$refInclude""", """$refHint""", """$projRefInclude"""
    if ([string]::IsNullOrEmpty($AdditionalInclude)) {
        $argList = ,'-NoExit' + $argList
    }
    if ($targetFrmwrkVer -eq 'v3.5') {
        $argList = '-Version', '2' + $argList
    }
    if ($NoIntro) {
        $argList += '-NoIntro'
    }
    if (![string]::IsNullOrEmpty($AdditionalInclude)) {
        $argList += "-AdditionalInclude ""$AdditionalInclude"""
    }
    if (![string]::IsNullOrEmpty($EditorialInclude)) {
        $argList += "-EditorialInclude ""$EditorialInclude"""
    }
    if ($PSCmdlet.MyInvocation.BoundParameters["Verbose"].IsPresent) {
        $argList += '-Verbose'
    }
    Write-Verbose ('Argument List: {0}' -f ($argList -join ' '))

    Start-Process $powershell $argList -Wait -WindowStyle $(if (![string]::IsNullOrEmpty($AdditionalInclude)) { 'Hidden' } else { 'Normal' })
}

New-Alias PStart Start-PrigSetup
