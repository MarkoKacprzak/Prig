﻿/* 
 * File: ProjectWideInstallerTest.cs
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



using EnvDTE;
using Moq;
using NuGet.VisualStudio;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Urasandesu.Prig.VSPackage.TestUtilities.Mixins.Moq;
using Test.Urasandesu.Prig.VSPackage.TestUtilities.Mixins.Ploeh.AutoFixture;
using Urasandesu.Prig.VSPackage;

namespace Test.Urasandesu.Prig.VSPackage
{
    [TestFixture]
    class ProjectWideInstallerTest
    {
        [Test]
        public void Install_should_install_if_a_package_has_not_been_installed_yet()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var proj = fixture.Freeze<Project>();
            var pwPkg = new ProjectWidePackage("Prig", "2.0.0", proj);
            var mocks = new MockRepository(MockBehavior.Strict);
            {
                var m = mocks.Create<IVsPackageInstallerServices>();
                m.Setup(_ => _.IsPackageInstalledEx(proj, "Prig", "2.0.0")).Returns(false);
                m.Setup(_ => _.IsPackageInstalled(proj, "Prig")).Returns(false);
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageUninstaller>();
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageInstaller>();
                m.Setup(_ => _.InstallPackage(default(string), proj, "Prig", "2.0.0", false));
                fixture.Inject(m);
            }

            var pwInstllr = fixture.NewProjectWideInstaller();


            // Act
            pwInstllr.Install(pwPkg);


            // Assert
            mocks.VerifyAll();
        }

        [Test]
        public void Install_should_uninstall_and_install_if_different_version_package_has_already_been_installed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var proj = fixture.Freeze<Project>();
            var pwPkg = new ProjectWidePackage("Prig", "2.0.0", proj);
            var mocks = new MockRepository(MockBehavior.Strict);
            {
                var m = mocks.Create<IVsPackageInstallerServices>();
                m.Setup(_ => _.IsPackageInstalledEx(proj, "Prig", "2.0.0")).Returns(false);
                m.Setup(_ => _.IsPackageInstalled(proj, "Prig")).Returns(true);
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageUninstaller>();
                m.Setup(_ => _.UninstallPackage(proj, "Prig", false));
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageInstaller>();
                m.Setup(_ => _.InstallPackage(default(string), proj, "Prig", "2.0.0", false));
                fixture.Inject(m);
            }

            var pwInstllr = fixture.NewProjectWideInstaller();


            // Act
            pwInstllr.Install(pwPkg);


            // Assert
            mocks.VerifyAll();
        }

        [Test]
        public void Install_should_do_nothing_if_same_version_package_has_already_been_installed()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var proj = fixture.Freeze<Project>();
            var pwPkg = new ProjectWidePackage("Prig", "2.0.0", proj);
            var mocks = new MockRepository(MockBehavior.Strict);
            {
                var m = mocks.Create<IVsPackageInstallerServices>();
                m.Setup(_ => _.IsPackageInstalledEx(proj, "Prig", "2.0.0")).Returns(true);
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageUninstaller>();
                fixture.Inject(m);
            }
            {
                var m = mocks.Create<IVsPackageInstaller>();
                fixture.Inject(m);
            }

            var pwInstllr = fixture.NewProjectWideInstaller();


            // Act
            pwInstllr.Install(pwPkg);


            // Assert
            mocks.VerifyAll();
        }

        [Test]
        public void Install_should_call_installation_steps_by_a_fixed_sequence()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var proj = fixture.Freeze<Project>();
            var pwPkg = new ProjectWidePackage("Prig", "2.0.0", proj);
            var callback = default(Action);
            var metadataArr = fixture.CreateMany<IVsPackageMetadata>(3).ToArray();
            {
                var m = fixture.Freeze<Mock<IVsPackageInstallerEvents>>();
                callback = () =>
                {
                    m.Raise(_ => _.PackageInstalling += null, metadataArr[0]);
                    m.Raise(_ => _.PackageInstalled += null, metadataArr[1]);
                    m.Raise(_ => _.PackageReferenceAdded += null, metadataArr[2]);
                };
            }
            {
                var m = fixture.Freeze<Mock<IVsPackageInstaller>>();
                m.Setup(_ => _.InstallPackage(It.IsAny<string>(), It.IsAny<Project>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Callback(callback);
            }
            var mocks = new MockRepository(MockBehavior.Strict);
            var order = new MockOrder();
            pwPkg.PackageInstalling += mocks.InOrder<VsPackageEventHandler>(order, m => m.Setup(_ => _(metadataArr[0]))).Object;
            pwPkg.PackageInstalled += mocks.InOrder<VsPackageEventHandler>(order, m => m.Setup(_ => _(metadataArr[1]))).Object;
            pwPkg.PackageReferenceAdded += mocks.InOrder<VsPackageEventHandler>(order, m => m.Setup(_ => _(metadataArr[2]))).Object;

            var pwInstllr = fixture.NewProjectWideInstaller();


            // Act
            pwInstllr.Install(pwPkg);


            // Assert
            mocks.VerifyAll();
        }
    }
}
