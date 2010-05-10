﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Persistence.Interfaces;
using HeuristicLab.Persistence.Core;
using System.Reflection;
using HeuristicLab.Persistence.Auxiliary;
using System.Text;
using System.Reflection.Emit;

namespace HeuristicLab.Persistence.Default.CompositeSerializers.Storable {

  /// <summary>
  /// Intended for serialization of all custom classes. Classes should have the
  /// <c>[StorableClass]</c> attribute set. The default mode is to serialize
  /// members with the <c>[Storable]</c> attribute set. Alternatively the
  /// storable mode can be set to <c>AllFields</c>, <c>AllProperties</c>
  /// or <c>AllFieldsAndAllProperties</c>.
  /// </summary>
  [StorableClass]
  public sealed class StorableSerializer : ICompositeSerializer {

    #region ICompositeSerializer implementation

    /// <summary>
    /// Priority 200, one of the first default composite serializers to try.
    /// </summary>
    /// <value></value>
    public int Priority {
      get { return 200; }
    }

    /// <summary>
    /// Determines for every type whether the composite serializer is applicable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can serialize the specified type; otherwise, <c>false</c>.
    /// </returns>
    public bool CanSerialize(Type type) {
      bool markedStorable = StorableReflection.HasStorableClassAttribute(type);
      if (GetConstructor(type) == null)
        if (markedStorable)
          throw new Exception("[Storable] type has no default constructor and no [StorableConstructor]");
        else
          return false;
      if (!StorableReflection.IsEmptyOrStorableType(type, true))
        if (markedStorable)
          throw new Exception("[Storable] type has non emtpy, non [Storable] base classes");
        else
          return false;
      return true;
    }

    /// <summary>
    /// Give a reason if possibly why the given type cannot be serialized by this
    /// ICompositeSerializer.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A string justifying why type cannot be serialized.
    /// </returns>
    public string JustifyRejection(Type type) {
      StringBuilder sb = new StringBuilder();
      if (GetConstructor(type) == null)
        sb.Append("class has no default constructor and no [StorableConstructor]");
      if (!StorableReflection.IsEmptyOrStorableType(type, true))
        sb.Append("class (or one of its bases) is not empty and not marked [Storable]; ");      
      return sb.ToString();
    }

    /// <summary>
    /// Creates the meta info.
    /// </summary>
    /// <param name="o">The object.</param>
    /// <returns>A list of storable components.</returns>
    public IEnumerable<Tag> CreateMetaInfo(object o) {
      InvokeHook(HookType.BeforeSerialization, o);
      return new Tag[] { };
    }

    /// <summary>
    /// Decompose an object into <see cref="Tag"/>s, the tag name can be null,
    /// the order in which elements are generated is guaranteed to be
    /// the same as they will be supplied to the Populate method.
    /// </summary>
    /// <param name="obj">An object.</param>
    /// <returns>An enumerable of <see cref="Tag"/>s.</returns>
    public IEnumerable<Tag> Decompose(object obj) {
      foreach (var accessor in GetStorableAccessors(obj)) {
        yield return new Tag(accessor.Name, accessor.Get());
      }
    }

    /// <summary>
    /// Create an instance of the object using the provided meta information.
    /// </summary>
    /// <param name="type">A type.</param>
    /// <param name="metaInfo">The meta information.</param>
    /// <returns>A fresh instance of the provided type.</returns>
    public object CreateInstance(Type type, IEnumerable<Tag> metaInfo) {
      try {
        return GetConstructor(type)();
      } catch (TargetInvocationException x) {
        throw new PersistenceException(
          "Could not instantiate storable object: Encountered exception during constructor call",
          x.InnerException);
      }
    }

    /// <summary>
    /// Populates the specified instance.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="objects">The objects.</param>
    /// <param name="type">The type.</param>
    public void Populate(object instance, IEnumerable<Tag> objects, Type type) {
      var memberDict = new Dictionary<string, Tag>();
      IEnumerator<Tag> iter = objects.GetEnumerator();
      while (iter.MoveNext()) {
        memberDict.Add(iter.Current.Name, iter.Current);
      }
      foreach (var accessor in GetStorableAccessors(instance)) {
        if (memberDict.ContainsKey(accessor.Name)) {
          accessor.Set(memberDict[accessor.Name].Value);
        } else if (accessor.DefaultValue != null) {
          accessor.Set(accessor.DefaultValue);
        }
      }
      InvokeHook(HookType.AfterDeserialization, instance);
    }

    #endregion

    #region constants & private data types

    private const BindingFlags ALL_CONSTRUCTORS =
      BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly object[] emptyArgs = new object[] { };

    private sealed class HookDesignator {
      public Type Type { get; private set; }
      public HookType HookType { get; private set; }
      public HookDesignator() { }
      public HookDesignator(Type type, HookType hookType) {
        Type = type;
        HookType = HookType;
      }
    }

    private sealed class MemberCache : Dictionary<Type, IEnumerable<StorableMemberInfo>> { }

    #endregion

    #region caches

    private MemberCache storableMemberCache = new MemberCache();

    private delegate object Constructor();

    private Dictionary<Type, Constructor> constructorCache =
      new Dictionary<Type, Constructor>();

    private Dictionary<HookDesignator, List<StorableReflection.Hook>> hookCache =
      new Dictionary<HookDesignator, List<StorableReflection.Hook>>();    

    #endregion

    #region attribute access

    private IEnumerable<StorableMemberInfo> GetStorableMembers(Type type) {
      lock (storableMemberCache) {
        if (storableMemberCache.ContainsKey(type))
          return storableMemberCache[type];
        var storablesMembers = StorableReflection.GenerateStorableMembers(type);
        storableMemberCache[type] = storablesMembers;
        return storablesMembers;
      }
    }

    private Constructor GetConstructor(Type type) {
      lock (constructorCache) {
        if (constructorCache.ContainsKey(type))
          return constructorCache[type];
        Constructor c = FindStorableConstructor(type) ?? GetDefaultConstructor(type);
        constructorCache.Add(type, c);
        return c;
      }
    }

    private Constructor GetDefaultConstructor(Type type) {
      ConstructorInfo ci = type.GetConstructor(ALL_CONSTRUCTORS, null, Type.EmptyTypes, null);
      if (ci == null)
        return null;
      DynamicMethod dm = new DynamicMethod("", typeof(object), null, type);
      ILGenerator ilgen = dm.GetILGenerator();
      ilgen.Emit(OpCodes.Newobj, ci);
      ilgen.Emit(OpCodes.Ret);
      return (Constructor)dm.CreateDelegate(typeof(Constructor));
    }

    private Constructor FindStorableConstructor(Type type) {
      foreach (ConstructorInfo ci in type.GetConstructors(ALL_CONSTRUCTORS)) {
        if (ci.GetCustomAttributes(typeof(StorableConstructorAttribute), false).Length > 0) {
          if (ci.GetParameters().Length != 1 ||
              ci.GetParameters()[0].ParameterType != typeof(bool))
            throw new PersistenceException("StorableConstructor must have exactly one argument of type bool");
          DynamicMethod dm = new DynamicMethod("", typeof(object), null, type);
          ILGenerator ilgen = dm.GetILGenerator();
          ilgen.Emit(OpCodes.Ldc_I4_1); // load true
          ilgen.Emit(OpCodes.Newobj, ci);
          ilgen.Emit(OpCodes.Ret);
          return (Constructor)dm.CreateDelegate(typeof(Constructor));
        }
      }
      return null;
    }

    private IEnumerable<DataMemberAccessor> GetStorableAccessors(object obj) {
      return GetStorableMembers(obj.GetType())
        .Select(mi => new DataMemberAccessor(mi.MemberInfo, mi.DisentangledName, mi.DefaultValue, obj));
    }

    private void InvokeHook(HookType hookType, object obj) {
      if (obj == null)
        throw new ArgumentNullException("Cannot invoke hooks on null");
      foreach (StorableReflection.Hook hook in GetHooks(hookType, obj.GetType())) {
        hook(obj);
      }
    }

    private IEnumerable<StorableReflection.Hook> GetHooks(HookType hookType, Type type) {
      lock (hookCache) {
        List<StorableReflection.Hook> hooks;
        var designator = new HookDesignator(type, hookType);
        hookCache.TryGetValue(designator, out hooks);
        if (hooks != null)
          return hooks;
        hooks = new List<StorableReflection.Hook>(StorableReflection.CollectHooks(hookType, type));
        hookCache.Add(designator, hooks);
        return hooks;
      }
    }

    #endregion



  }

}