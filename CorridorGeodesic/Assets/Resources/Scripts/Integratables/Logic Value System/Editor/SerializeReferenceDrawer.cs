using Neverway.Framework.LogicValueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PolymorphicAttribute))]
public class SerializeReferenceDrawer : DecoratorDrawer
{
    public override VisualElement CreatePropertyGUI()
    {
        return null;
    }
    //public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    //{
    //    contents.Add(new PolymorphicSelector(property, $"Polymorphic {property.FieldType}: "));
    //    contents.Add(new Divider());
    //    contents.Add(new Property(property));
    //    return contents;
    //}
    //public override void OnAfterGUI() { }
}
