﻿
using System.ComponentModel;
using Urasandesu.Prig.Framework;

namespace System.IO.Prig
{
    public class PStream : PStreamBase 
    {
        public static zzBeginRead BeginRead() 
        {
            return new zzBeginRead();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class zzBeginRead 
        {
            public IndirectionFunc<System.IO.Stream, System.Byte[], System.Int32, System.Int32, System.AsyncCallback, System.Object, System.IAsyncResult> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    info.Token = TokenOfBeginRead_byteArray_int_int_AsyncCallback_Object;
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<System.IO.Stream, System.Byte[], System.Int32, System.Int32, System.AsyncCallback, System.Object, System.IAsyncResult>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }
    }
}
