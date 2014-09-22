﻿
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Urasandesu.Prig.Framework;

namespace System.IO.Prig
{
    public class PMemoryStream : PMemoryStreamBase 
    {
        public static IndirectionBehaviors DefaultBehavior { get; internal set; }

        public static zzCanSeekGet CanSeekGet() 
        {
            return new zzCanSeekGet();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class zzCanSeekGet : IBehaviorPreparable 
        {
            public IndirectionFunc<System.IO.MemoryStream, System.Boolean> Body
            {
                get
                {
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>>();
                    return holder.GetOrDefault(Info);
                }
                set
                {
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>>();
                    if (value == null)
                    {
                        holder.Remove(Info);
                    }
                    else
                    {
                        holder.AddOrUpdate(Info, value);
                        RuntimeHelpers.PrepareDelegate(Body);
                    }
                }
            }

            public void Prepare(IndirectionBehaviors defaultBehavior)
            {
                var behavior = IndirectionDelegates.CreateDelegateOfDefaultBehaviorIndirectionFunc<System.IO.MemoryStream, System.Boolean>(defaultBehavior);
                Body = behavior;
            }

            public IndirectionInfo Info
            {
                get
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    info.Token = TokenOfCanSeekGet;
                    return info;
                }
            }
            internal void SetTargetInstanceBody(System.IO.MemoryStream target, IndirectionFunc<System.IO.MemoryStream, System.Boolean> value)
            {
                RuntimeHelpers.PrepareDelegate(value);

                var holder = LooseCrossDomainAccessor.GetOrRegister<GenericHolder<TaggedBag<zzCanSeekGet, Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>>>>>();
                if (holder.Source.Value == null)
                    holder.Source = TaggedBagFactory<zzCanSeekGet>.Make(new Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>>());

                if (holder.Source.Value.Count == 0)
                {
                    var behavior = Body == null ? IndirectionDelegates.CreateDelegateOfDefaultBehaviorIndirectionFunc<System.IO.MemoryStream, System.Boolean>(IndirectionBehaviors.Fallthrough) : Body;
                    RuntimeHelpers.PrepareDelegate(behavior);
                    holder.Source.Value[target] = new TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>(behavior, value);
                    {
                        // Prepare JIT
                        var original = holder.Source.Value[target].Original;
                        var indirection = holder.Source.Value[target].Indirection;
                    }
                    Body = IndirectionDelegates.CreateDelegateExecutingDefaultOrIndirectionFunc<System.IO.MemoryStream, System.Boolean>(behavior, holder.Source.Value);
                }
                else
                {
                    Debug.Assert(Body != null);
                    var before = holder.Source.Value[target];
                    holder.Source.Value[target] = new TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>(before.Original, value);
                }
            }

            internal void RemoveTargetInstanceBody(System.IO.MemoryStream target)
            {
                var holder = LooseCrossDomainAccessor.GetOrRegister<GenericHolder<TaggedBag<zzCanSeekGet, Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>>>>>();
                if (holder.Source.Value == null)
                    return;

                if (holder.Source.Value.Count == 0)
                    return;

                var before = default(TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Boolean>>);
                if (holder.Source.Value.ContainsKey(target))
                    before = holder.Source.Value[target];
                holder.Source.Value.Remove(target);
                if (holder.Source.Value.Count == 0)
                    Body = before.Original;
            }
        }
 
        public static zzSeekInt64SeekOrigin SeekInt64SeekOrigin() 
        {
            return new zzSeekInt64SeekOrigin();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class zzSeekInt64SeekOrigin : IBehaviorPreparable 
        {
            public IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64> Body
            {
                get
                {
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>>();
                    return holder.GetOrDefault(Info);
                }
                set
                {
                    var holder = LooseCrossDomainAccessor.GetOrRegister<IndirectionHolder<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>>();
                    if (value == null)
                    {
                        holder.Remove(Info);
                    }
                    else
                    {
                        holder.AddOrUpdate(Info, value);
                        RuntimeHelpers.PrepareDelegate(Body);
                    }
                }
            }

            public void Prepare(IndirectionBehaviors defaultBehavior)
            {
                var behavior = IndirectionDelegates.CreateDelegateOfDefaultBehaviorIndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>(defaultBehavior);
                Body = behavior;
            }

            public IndirectionInfo Info
            {
                get
                {
                    var info = new IndirectionInfo();
                    info.AssemblyName = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    info.Token = TokenOfSeekInt64SeekOrigin;
                    return info;
                }
            }
            internal void SetTargetInstanceBody(System.IO.MemoryStream target, IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64> value)
            {
                RuntimeHelpers.PrepareDelegate(value);

                var holder = LooseCrossDomainAccessor.GetOrRegister<GenericHolder<TaggedBag<zzSeekInt64SeekOrigin, Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>>>>>();
                if (holder.Source.Value == null)
                    holder.Source = TaggedBagFactory<zzSeekInt64SeekOrigin>.Make(new Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>>());

                if (holder.Source.Value.Count == 0)
                {
                    var behavior = Body == null ? IndirectionDelegates.CreateDelegateOfDefaultBehaviorIndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>(IndirectionBehaviors.Fallthrough) : Body;
                    RuntimeHelpers.PrepareDelegate(behavior);
                    holder.Source.Value[target] = new TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>(behavior, value);
                    {
                        // Prepare JIT
                        var original = holder.Source.Value[target].Original;
                        var indirection = holder.Source.Value[target].Indirection;
                    }
                    Body = IndirectionDelegates.CreateDelegateExecutingDefaultOrIndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>(behavior, holder.Source.Value);
                }
                else
                {
                    Debug.Assert(Body != null);
                    var before = holder.Source.Value[target];
                    holder.Source.Value[target] = new TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>(before.Original, value);
                }
            }

            internal void RemoveTargetInstanceBody(System.IO.MemoryStream target)
            {
                var holder = LooseCrossDomainAccessor.GetOrRegister<GenericHolder<TaggedBag<zzSeekInt64SeekOrigin, Dictionary<System.IO.MemoryStream, TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>>>>>();
                if (holder.Source.Value == null)
                    return;

                if (holder.Source.Value.Count == 0)
                    return;

                var before = default(TargetSettingValue<IndirectionFunc<System.IO.MemoryStream, System.Int64, System.IO.SeekOrigin, System.Int64>>);
                if (holder.Source.Value.ContainsKey(target))
                    before = holder.Source.Value[target];
                holder.Source.Value.Remove(target);
                if (holder.Source.Value.Count == 0)
                    Body = before.Original;
            }
        }


        public static TypeBehaviorSetting ExcludeGeneric()
        {
            var preparables = typeof(PMemoryStream).GetNestedTypes().
                                          Where(_ => _.GetInterface(typeof(IBehaviorPreparable).FullName) != null).
                                          Where(_ => !_.IsGenericType).
                                          Select(_ => Activator.CreateInstance(_)).
                                          Cast<IBehaviorPreparable>();
            var setting = new TypeBehaviorSetting();
            foreach (var preparable in preparables)
                setting.Include(preparable);
            return setting;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class TypeBehaviorSetting : BehaviorSetting
        {
            public override IndirectionBehaviors DefaultBehavior
            {
                set
                {
                    PMemoryStream.DefaultBehavior = value;
                    foreach (var preparable in Preparables)
                        preparable.Prepare(PMemoryStream.DefaultBehavior);
                }
            }
        }
    }
}
