﻿/* 
 * File: EnvironmentRepositoryTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2015 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */



using Microsoft.Win32;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Test.Urasandesu.Prig.VSPackage.Mixins.System;
using Urasandesu.Prig.VSPackage;

namespace Test.Urasandesu.Prig.VSPackage
{
    [TestFixture]
    class EnvironmentRepositoryTest
    {
        [Test]
        public void GetPackageFolder_should_return_executing_assembly_directory_path()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.GetPackageFolder();

            // Assert
            Assert.That(result, Is.StringMatching(GetShadowCopyTargetDirectoryPattern()));
        }



        [Test]
        [Explicit("This test case will be invoked from the test case that have the name `Invoke_ + <test case name same as this>` automatically. " + 
                  "Note that don't run this test case manually.")]
        public void SetPackageFolder_should_set_package_folder_as_environment_variable()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var variableValue = fixture.Create<string>();

            var envRepos = new EnvironmentRepository();


            // Act
            envRepos.SetPackageFolder(variableValue);


            // Assert
            Assert.AreEqual(variableValue, Environment.GetEnvironmentVariable("URASANDESU_PRIG_PACKAGE_FOLDER"));
        }
        [Test]
        [Explicit("This test has the possibility that your machine environment is changed. You have to understand the content if you will run it.")]
        public void Invoke_SetPackageFolder_should_set_package_folder_as_environment_variable()
        {
            try
            {
                VerifyEnvironmentRepositoryTest("SetPackageFolder_should_set_package_folder_as_environment_variable");
                Assert.IsNotNull(Environment.GetEnvironmentVariable("URASANDESU_PRIG_PACKAGE_FOLDER", EnvironmentVariableTarget.User));
            }
            finally
            {
                Environment.SetEnvironmentVariable("URASANDESU_PRIG_PACKAGE_FOLDER", null, EnvironmentVariableTarget.User);
            }
        }



        [Test]
        public void GetToolsPath_should_return_tools_directory_in_executing_assembly_directory_path()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.GetToolsPath();

            // Assert
            Assert.That(result, Is.StringMatching(ToShadowCopyTargetPattern(@"tools")));
        }



        [Test]
        public void GetProfilerLocations_should_always_return_fixed_values()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var results = envRepos.GetProfilerLocations();

            // Assert
            Assert.AreEqual(2, results.Length);
            Assert.AreEqual(RegistryView.Registry64, results[0].RegistryView);
            Assert.That(results[0].PathOfInstalling, Is.StringMatching(ToShadowCopyTargetPattern(@"tools\x64\Urasandesu.Prig.dll")));
            Assert.AreEqual(RegistryView.Registry32, results[1].RegistryView);
            Assert.That(results[1].PathOfInstalling, Is.StringMatching(ToShadowCopyTargetPattern(@"tools\x86\Urasandesu.Prig.dll")));
        }



        [Test]
        public void GetNuGetPath_should_return_nuget_path_in_tools_directory()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.GetNuGetPath();

            // Assert
            Assert.That(result, Is.StringMatching(ToShadowCopyTargetPattern(@"tools\NuGet.exe")));
        }



        [Test]
        public void GetRegsvr32Path_should_return_regsvr32_path_in_system32_directory()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.GetRegsvr32Path();

            // Assert
            Assert.That(result, Is.SamePath(@"C:\Windows\SysNative\regsvr32.exe"));
        }



        [Test]
        public void GetPrigPath_should_return_prig_path_in_tools_directory()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.GetPrigPath();

            // Assert
            Assert.That(result, Is.StringMatching(ToShadowCopyTargetPattern(@"tools\prig.exe")));
        }



        [Test]
        public void OpenRegistryBaseKey_should_open_specified_registry_key()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            using (var result = envRepos.OpenRegistryBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32))
            {
                // Assert
                Assert.AreEqual(Registry.ClassesRoot.ToString(), result.ToString());
            }
        }



        [Test]
        public void OpenRegistrySubKey_should_open_specified_registry_sub_key()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            using (var result = envRepos.OpenRegistrySubKey(Registry.ClassesRoot, @"TypeLib\{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}\1.1"))
            {
                // Assert
                Assert.IsNotNull(result);
            }
        }



        [Test]
        public void GetRegistryValue_should_get_specified_registry_value()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            using (var key = Registry.ClassesRoot.OpenSubKey(@"TypeLib\{EAB22AC0-30C1-11CF-A7EB-0000C05BAE0B}\1.1"))
            {
                var result = envRepos.GetRegistryValue(key, null);

                // Assert
                Assert.AreEqual("Microsoft Internet Controls", result);
            }
        }



        [Test]
        public void ExistsFile_should_return_true_if_file_exists()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.ExistsFile(Assembly.GetExecutingAssembly().Location);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ExistsFile_should_return_false_if_file_does_not_exist()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var envRepos = new EnvironmentRepository();

            // Act
            var result = envRepos.ExistsFile(fixture.Create<string>());

            // Assert
            Assert.IsFalse(result);
        }



        [Test]
        public void GetFileDescription_should_return_the_file_description_of_the_specified_file()
        {
            // Arrange
            var envRepos = new EnvironmentRepository();
            
            // Act
            var result = envRepos.GetFileDescription(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tools\x64\Urasandesu.Prig.dll"));

            // Assert
            Assert.That(result, Is.StringMatching(@"Prig Profiler \d+\.\d+\.\d+ Type Library"));
        }



        static int StartProcessWithoutShell(string fileName, string arguments)
        {
            var info = NewProcessStartInfoWithoutShell(fileName, arguments);
            using (var p = Process.Start(info))
            {
                p.WaitForExit();
                return p.ExitCode;
            }
        }

        static ProcessStartInfo NewProcessStartInfoWithoutShell(string fileName, string arguments)
        {
            var info = new ProcessStartInfo();
            info.FileName = fileName;
            info.Arguments = arguments;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            return info;
        }

        static int StartNUnitConsoleX86(string arguments)
        {
            var nunitDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var nunitConsoleX86 = Path.Combine(nunitDir, "nunit-console-x86.exe");
            return StartProcessWithoutShell(nunitConsoleX86, arguments);
        }

        static int StartEnvironmentRepositoryTest(string testCase, string resultXml)
        {
            var arguments = string.Format("/domain=None /framework=v4.5 /run:Test.Urasandesu.Prig.VSPackage.EnvironmentRepositoryTest.{0} Test.Urasandesu.Prig.VSPackage.dll /result={1}", testCase, resultXml);
            return StartNUnitConsoleX86(arguments);
        }

        static void VerifyEnvironmentRepositoryTest(string testCase)
        {
            var resultXml = Guid.NewGuid().ToString("N") + ".xml";
            Assert.AreEqual(0, StartEnvironmentRepositoryTest(testCase, resultXml), "For details, see " + resultXml);
        }



        static string GetShadowCopyTargetDirectoryPattern()
        {
            var shadowCopyTargetBase = AppDomain.CurrentDomain.GetShadowCopyTargetDirectory();
            var pattern = Regex.Escape(shadowCopyTargetBase);
            return pattern;
        }

        static string ToShadowCopyTargetPattern(string relativePathFromPackageDirectory)
        {
            var shadowCopyTargetDirPattern = GetShadowCopyTargetDirectoryPattern();
            var relativeDirFromPkgDir = Path.GetDirectoryName(relativePathFromPackageDirectory);
            if (string.IsNullOrEmpty(relativeDirFromPkgDir))
                relativeDirFromPkgDir = relativePathFromPackageDirectory;
            var gap = @"\\((?!" + relativeDirFromPkgDir + @").)+\\";
            var pattern = shadowCopyTargetDirPattern + gap + Regex.Escape(relativePathFromPackageDirectory);
            return pattern;
        }
    }
}

namespace Test.Urasandesu.Prig.VSPackage.Mixins.System
{
    public static class AppDomainMixin
    {
        public static string GetShadowCopyTargetDirectory(this AppDomain domain)
        {
            return Path.Combine(domain.SetupInformation.CachePath, domain.SetupInformation.ApplicationName);
        }

        public static string GetPathInBaseDirectory(this AppDomain domain, string relativePathInApplicationBase)
        {
            return Path.Combine(domain.BaseDirectory, relativePathInApplicationBase);
        }
    }
}