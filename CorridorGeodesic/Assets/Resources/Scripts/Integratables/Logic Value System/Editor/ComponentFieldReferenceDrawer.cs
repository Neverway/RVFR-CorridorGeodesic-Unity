//=-------- Script by Errynei ----------=//
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ComponentFieldReference<>))]
public class ComponentFieldReferenceDrawer : EasyDrawer
{
    public string targetGameObject = "EDITOR_targetGameObject";
    public string targetComponent = "targetComponent";
    public string fieldName = "fieldName";
    public string hideTypeFilterText = "EDITOR_hideTypeFilterText";

    public Component oldComponent;

    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        HorizontalGroup line = new HorizontalGroup();

        oldComponent = GetComponent;

        if (HasComponent)
        {
            SetGameObjectAsComponentsGameObject();
            GUISelectField(line);
        }
        else
        {
            if (HasGameObject)
                GUISelectComponentFromGameObject(line);
            else
                GUISelectGameObjectOrComponent(line);
        }
        if (property[hideTypeFilterText].Property.boolValue)
            contents.Add(new Boxed(line));
        else
            contents.Add(new FittedLabel($" {ReferenceType.SelectedName(true, true)}: ", new Boxed(line)).MaxWidthFactor(0.33f));


        return contents;
    }
    public override void OnAfterGUI()
    {
        if (!HasComponent || oldComponent == null)
            return;

        Component newComponent = GetComponent;
        if ((oldComponent != newComponent) && HasGameObject && (newComponent is Transform))
        {
            property[targetComponent].Property.objectReferenceValue = null;
            property[targetGameObject].Property.objectReferenceValue = oldComponent.gameObject;
        }
    }

    public void GUISelectGameObjectOrComponent(HorizontalGroup line)
    {
        line.Add(new Property(property[targetGameObject]).HideLabel());
        line.Add(new Property(property[targetComponent]).HideLabel());
    }
    public void GUISelectComponentFromGameObject(HorizontalGroup line)
    {
        line.Add(new Property(property[targetGameObject]).HideLabel());
        line.Add(new SelectComponentFromGameObject(
            (GameObject)property[targetGameObject].Property.objectReferenceValue,
            property[targetComponent]
            ));
    }
    public void GUISelectField(HorizontalGroup line)
    {
        line.Add(new Property(property[targetComponent]).HideLabel());
        line.Add(new SelectFieldDropdown(
            property[targetComponent].Property.objectReferenceValue,
            property[fieldName])
            .FilterByType(ReferenceType)
            );
    }

    public void SetGameObjectAsComponentsGameObject()
    {
        property[targetGameObject].Property.objectReferenceValue = 
            ((Component)property[targetComponent].Property.objectReferenceValue).gameObject;
    }

    public bool HasComponent => property[targetComponent].Property.objectReferenceValue != null;
    public bool HasGameObject => property[targetGameObject].Property.objectReferenceValue != null;

    public GameObject GetGameObject => (GameObject)property[targetGameObject].Property.objectReferenceValue;
    public Component GetComponent => (Component)property[targetComponent].Property.objectReferenceValue;


    public Type ReferenceType => fieldInfo.FieldType.GetGenericArguments()[0];
}