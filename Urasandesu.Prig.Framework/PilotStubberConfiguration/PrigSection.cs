﻿/* 
 * File: PrigSection.cs
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



using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System;

namespace Urasandesu.Prig.Framework.PilotStubberConfiguration
{
    public class PrigSection : ConfigurationSection
    {
        [ConfigurationProperty("stubs", Options = ConfigurationPropertyOptions.IsRequired)]
        internal StubCollection InternalStubs
        {
            get
            {
                return (StubCollection)this["stubs"];
            }
        }

        IList<IndirectionStub> m_stubs;
        public IEnumerable<IndirectionStub> Stubs
        {
            get
            {
                if (m_stubs == null)
                    m_stubs = MakeStubs(InternalStubs);
                return m_stubs;
            }
        }

        public IEnumerable<IGrouping<string, IGrouping<Type, IndirectionStub>>> GroupedStubs 
        {
            get { return Stubs.GroupBy(_ => _.Target.DeclaringType).GroupBy(_ => _.Key.Namespace); }
        }

        static IList<IndirectionStub> MakeStubs(StubCollection internalStubs)
        {
            Debug.Assert(internalStubs != null);

            var stubs = new List<IndirectionStub>();
            foreach (StubElement internalStub in internalStubs)
                stubs.Add(MakeStub(internalStub));
            return stubs;
        }

        static IndirectionStub MakeStub(StubElement internalStub)
        {
            Debug.Assert(internalStub != null);

            if (internalStub.Target == null)
                throw new KeyNotFoundException(""); // TODO: 
            
            return new IndirectionStub(internalStub.Name, internalStub.Alias, internalStub.Target);
        }
    }
}
