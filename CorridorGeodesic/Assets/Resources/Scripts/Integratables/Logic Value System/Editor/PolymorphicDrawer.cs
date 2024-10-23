using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using static EasyDrawer;
using System.Linq;
using Unity.VisualScripting;
/*
[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
public class PolymorphicAttributeDrawer : EasyDrawer
{
//public override VisualElement CreatePropertyGUI()
//{
//
//    return null;
//}
public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
{
contents.Add(new PolymorphicSelector(property, $"Polymorphic {property.FieldType}: "));
contents.Add(new Divider());
//contents.Add(new Property(property));
return contents;
}
public override void OnAfterGUI() { }
}
// */
[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
public class PolymorphicDrawer : PropertyDrawer
{
    public static Dictionary<Type, List<Type>> cachedDerivedTypes = new Dictionary<Type, List<Type>>();
    public static GUIStyle popupStyle;
    public static GUIStyle popupStyleIfNull;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }
        bool hasValue = property.managedReferenceValue != null;


        // Get the currently assigned type (if any)
        Type currentType = GetManagedReferenceType(property);
        if (currentType == null)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, "---Could not find type of field?---");
            EditorGUI.EndProperty();

            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        List<Type> derivedTypes;
        cachedDerivedTypes.TryGetValue(currentType, out derivedTypes);

        if (derivedTypes == null)
        {
            // Get all valid types for the field
            derivedTypes = GetAllDerivedTypes(currentType, fieldInfo.FieldType);
            cachedDerivedTypes.Add(currentType, derivedTypes);
        }    

        //Initialize the popupStyle if null
        if (popupStyle == null || popupStyleIfNull == null)
        {
            popupStyle = new GUIStyle(EditorStyles.popup);
            popupStyle.normal.textColor = new Color(.27f, 0.78f, 0.63f);
            popupStyle.active.textColor = new Color(.37f, 0.88f, 0.73f);
            popupStyle.hover.textColor = new Color(.47f, 0.98f, 0.83f);
            popupStyle.focused.textColor = new Color(.27f, 0.78f, 0.63f);
            popupStyle.fontSize += 2;

            popupStyleIfNull = new GUIStyle(EditorStyles.popup);
            popupStyleIfNull.normal.textColor = new Color(.85f, .9f, 0f);
            popupStyleIfNull.active.textColor = new Color(.85f, .9f, 0f);
            popupStyleIfNull.hover.textColor = new Color(1f, 1f, 0f);
            popupStyleIfNull.focused.textColor = new Color(.85f, .9f, 0f);
            popupStyleIfNull.fontSize += 2;
        }

        // Create dropdown options from the derived types
        List<string> typeOptions = new List<string> { "--- No Type Selected ---" };
        typeOptions.AddRange(derivedTypes.Select(t => t.FullName));

        // Get the current type's index in the dropdown
        int selectedIndex = hasValue ? typeOptions.IndexOf(currentType.FullName) : 0;

        // Draw the dropdown
        position.height = EditorGUIUtility.singleLineHeight;
        int newSelectedIndex = EditorGUI.Popup(position, " ", selectedIndex, typeOptions.ToArray(), 
            (hasValue ? popupStyle : popupStyleIfNull));

        // If the selected index changed, update the property
        if (newSelectedIndex != selectedIndex)
        {
            if (newSelectedIndex == 0)
            {
                property.managedReferenceValue = null;
            }
            else
            {
                Type newType = derivedTypes[newSelectedIndex - 1]; // Adjust for "(None)"
                property.managedReferenceValue = Activator.CreateInstance(newType);
            }
        }

        // Save original indent level and expand child properties
        int originalIndent = EditorGUI.indentLevel;

        position.height = EditorGUI.GetPropertyHeight(property, true);
        EditorGUI.PropertyField(position, property, true);

        EditorGUI.EndProperty();
    }

    private List<Type> GetAllDerivedTypes(Type currentType, Type baseType)
    {
        // Get all derived types of the base type or interface
        if (baseType == null)
            return new List<Type>();

        baseType = GetNonArrayType(baseType);

        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();
    }

    private Type GetManagedReferenceType(SerializedProperty property)
    {
        // If the property contains a valid reference, retrieve its type
        if (!string.IsNullOrEmpty(property.managedReferenceFullTypename))
        {
            string[] typeInfo = property.managedReferenceFullTypename.Split(' ');
            if (typeInfo.Length == 2)
            {
                string assemblyName = typeInfo[0];
                string className = typeInfo[1];
                return Type.GetType($"{className}, {assemblyName}");
            }
        }

        // If null, return the field's base type (the declared type of the field)
        return GetNonArrayType(fieldInfo.FieldType);
    }
    private Type GetNonArrayType(Type fieldType)
    {
        // Check if the type is an array
        if (fieldType.IsArray)
        {
            // Get the type of the elements in the array
            return fieldType.GetElementType();
        }

        // Check if the type is a generic collection (like List<T>)
        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
        {
            // Get the type of the elements in the List<T>
            return fieldType.GetGenericArguments()[0];
        }

        // If it's not an array or a generic type, return the type itself
        return fieldType;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}

/*: EasyDrawer
{
public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
{
    contents.Add(new PolymorphicSelector(property));
    //EditorGUILayout.PropertyField(property.Property);
    return contents;
}

public override void OnAfterGUI() { }
}
// */