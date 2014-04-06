﻿/* 
 * File: PDateTime.cs
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


using Urasandesu.Prig.Framework;

#if _M_IX86
[assembly: Indirectable(0x060002BC)]
[assembly: Indirectable(0x060002D2)]
[assembly: Indirectable(0x060002B8)]
#else
[assembly: Indirectable(0x060002BE)]
[assembly: Indirectable(0x060002D4)]
[assembly: Indirectable(0x060002BA)]
#endif
[assembly: Indirectable(0x060002B3)]

namespace System.Prig
{
    public class PDateTime
    {
        public static class NowGet
        {
            public static IndirectionFunc<DateTime> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
#if _M_IX86
                    info.Token = 0x060002D2;
#else
                    info.Token = 0x060002D4;
#endif
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<DateTime>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }

        public static class FromBinary
        {
            public static IndirectionFunc<long, DateTime> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
#if _M_IX86
                    info.Token = 0x060002BC;
#else
                    info.Token = 0x060002BE;
#endif
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<long, DateTime>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }

        public static class DoubleDateToTicks
        {
            public static IndirectionFunc<double, long> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
#if _M_IX86
                    info.Token = 0x060002B8;
#else
                    info.Token = 0x060002BA;
#endif
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<double, long>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }

        public static class CompareTo
        {
            public static IndirectionRefThisFunc<DateTime, object, int> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    info.Token = 0x060002B3;
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionRefThisFunc<DateTime, object, int>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }
    }
}
