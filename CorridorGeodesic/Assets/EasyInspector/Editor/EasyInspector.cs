using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Linq;
using static EasyDrawer;
using EasyInspector;

namespace EasyInspector
{
    public class EasyInspector
    {

    }

    public class EasyProperty : IEnumerable<EasyProperty>
    {
        private SerializedProperty property;
        private Type cachedType;
        private FieldInfo cachedFieldInfo;
        private Dictionary<string, EasyProperty> properties;

        public EasyProperty(SerializedProperty property)
        {
            properties = new Dictionary<string, EasyProperty>();
            this.property = property;
            this.cachedType = null;
            this.cachedFieldInfo = null;
        }
        public EasyProperty(SerializedObject obj, string propertyName) : this(obj.FindProperty(propertyName)) { }
        public EasyProperty(UnityEngine.Object obj, string propertyName) : this(new SerializedObject(obj), propertyName) { }

        public SerializedProperty Property => property;

        public EasyProperty this[params string[] fields]
        {
            get
            {
                EasyProperty toReturn = this;
                foreach (string field in fields)
                {
                    if (!toReturn.properties.ContainsKey(field))
                        toReturn.properties.Add(field, new EasyProperty(property.FindPropertyRelative(field)));

                    toReturn = toReturn.properties[field];
                }
                return toReturn;
            }
        }
        public EasyProperty this[params int[] arrayIndexs]
        {
            get
            {
                EasyProperty toReturn = this;
                foreach (int index in arrayIndexs)
                {
                    //if (!toReturn.properties.ContainsKey(field))
                    //    toReturn.properties.Add(field, new EasyProperty(property.FindPropertyRelative(field)));
                    //
                    //toReturn = toReturn.properties[field];
                }
                return toReturn;
            }
        }

        public Type FieldType => (cachedType == null) ? cachedType = property.GetUnderlyingType() : cachedType;
        public FieldInfo FieldInfo => (cachedFieldInfo == null) ? cachedFieldInfo = property.GetFieldInfo() : cachedFieldInfo;
        public bool IsPropertyInArrayOrList() => property.propertyPath.Contains("Array.data");
        public EasyProperty[] GetArray()
        {
            List<EasyProperty> list = new List<EasyProperty>();
            IEnumerator enumerator = property.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(new EasyProperty(enumerator.Current as SerializedProperty));
            }
            return list.ToArray();
        }
        public static implicit operator SerializedProperty(EasyProperty easyProp) => easyProp.property;

        #region Quick Access To Property Values
        public AnimationCurve AsAnimationCurve
        {
            get { return property.animationCurveValue; }
            set { property.animationCurveValue = value; }
        }
        public bool AsBool
        {
            get { return property.boolValue; }
            set { property.boolValue = value; }
        }
        public string AsString
        {
            get { return property.stringValue; }
            set { property.stringValue = value; }
        }
        public float AsFloat
        {
            get { return property.floatValue; }
            set { property.floatValue = value; }
        }
        public int AsInt
        {
            get { return property.intValue; }
            set { property.intValue = value; }
        }
        public Vector3 AsVector3
        {
            get { return property.vector3Value; }
            set { property.vector3Value = value; }
        }
        public Vector3Int AsVector3Int
        {
            get { return property.vector3IntValue; }
            set { property.vector3IntValue = value; }
        }
        public Vector2 AsVector2
        {
            get { return property.vector2Value; }
            set { property.vector2Value = value; }
        }
        public Vector2Int AsVector2Int
        {
            get { return property.vector2IntValue; }
            set { property.vector2IntValue = value; }
        }
        public UnityEngine.Object AsUnityObject
        {
            get { return property.objectReferenceValue; }
            set { property.objectReferenceValue = value; }
        }
        #endregion

        public bool IsArray => property.isArray;
        public int Count => property.arraySize;

        public SerializedObject SerializedObject => property.serializedObject;

        //GetExpensive
        public T Get<T>() => (T)property.boxedValue;
        //SetExpensive
        public void Set(object value) => property.boxedValue = value;

        public IEnumerator<EasyProperty> GetEnumerator()
        {
            foreach(SerializedProperty prop in property)
            {
                yield return new EasyProperty(prop);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}