using Neverway.Framework.LogicValueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ReflectionUtility
{
    public static Assembly[] AllAssemblies => _allAssemblies == null ? AllAssemblies : _allAssemblies;
    private static Assembly[] _allAssemblies;
    private static Assembly[] CacheAllAssemblies() => _allAssemblies = 
        AppDomain.CurrentDomain.GetAssemblies();

    public static Type[] AllTypes => _allTypes == null ? CacheAllTypes() : _allTypes;
    private static Type[] _allTypes;
    private static Type[] CacheAllTypes() => _allTypes = 
        AllAssemblies.SelectMany(assembly => assembly.GetTypes()).ToArray();

    //private static Dictionary<Type, FieldInfo[]> _typeToFields = new Dictionary<Type, FieldInfo[]>();
    //public static FieldInfo[] GetFields<T>() => GetFieldsCACHED(typeof(T));
    //public static FieldInfo[] GetFieldsCACHED(this Type type) => 
    //    _typeToFields.ContainsKey(type) ? _typeToFields[type] : _typeToFields[type] = type.GetFields();

}