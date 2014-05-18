﻿
using Urasandesu.Prig.Framework;

namespace System.Prig
{
    public class PDateTimeParse : PDateTimeParseBase
    {
        public static class TryParse
        {
            public static IndirectionOutFunc<System.String, System.Globalization.DateTimeFormatInfo, System.Globalization.DateTimeStyles, System.DateTime, System.Boolean> Body
            {
                set
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    info.Token = TokenOfTryParse_string_DateTimeFormatInfo_DateTimeStyles_DateTimeRef;
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionOutFunc<System.String, System.Globalization.DateTimeFormatInfo, System.Globalization.DateTimeStyles, System.DateTime, System.Boolean>>>();
                    holder.AddOrUpdate(info, value);
                }
            }
        }
    }
}
