﻿/* 
 * File: AdditionalDelegatesAssemblyRepository.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2014 Akira Sugiura
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


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Urasandesu.Prig.Framework
{
    public class AdditionalDelegatesAssemblyRepository
    {
        static List<Assembly> ms_indirectionDelegatesList;

        public virtual IEnumerable<Assembly> FindAll()
        {
            if (ms_indirectionDelegatesList != null)
                return ms_indirectionDelegatesList;

            var pkgPath = Environment.GetEnvironmentVariable("URASANDESU_PRIG_PACKAGE_FOLDER");
            var toolsPath = Path.Combine(pkgPath, "tools");
            var prigConfigPath = Path.Combine(toolsPath, "Prig.config");

            var config = DeserializeOrEmpty<PrigConfig>(prigConfigPath);
            // This is by design, all additional delegates settings have same information. See also the help for `prig update` command.
            var pkg = config.Packages.item.DefaultIfEmpty(new PrigPackageConfig()).First();
            var additionalDlgts = pkg.AdditionalDelegates.item;
            ms_indirectionDelegatesList = additionalDlgts.Select(ToIndirectionDelegate).Where(_ => _ != null).ToList();

            var corVersion = typeof(object).Assembly.ImageRuntimeVersion;
            var libPath = Path.Combine(pkgPath, "lib");
            var prigDelegatesNames = new string[4];
            prigDelegatesNames[0] = "Urasandesu.Prig.Delegates.dll";
            prigDelegatesNames[1] = "Urasandesu.Prig.Delegates.0404.dll";
            prigDelegatesNames[2] = "Urasandesu.Prig.Delegates.0804.dll";
            prigDelegatesNames[3] = "Urasandesu.Prig.Delegates.1205.dll";
            foreach (var prigDelegatesName in prigDelegatesNames)
                ms_indirectionDelegatesList.Add(Assembly.LoadFrom(Path.Combine(libPath, prigDelegatesName)));

            return ms_indirectionDelegatesList;
        }

        T DeserializeOrEmpty<T>(string path) where T : new()
        {
            if (!File.Exists(path))
                return new T();

            using (var sr = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        static Assembly ToIndirectionDelegate(PrigAdditionalDelegateConfig config)
        {
            try
            {
                return Assembly.LoadFrom(config.HintPath.Native);
            }
            catch
            {
                return null;
            }
        }
    }
}
