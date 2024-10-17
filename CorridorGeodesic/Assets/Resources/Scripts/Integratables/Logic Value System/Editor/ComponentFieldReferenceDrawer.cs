//=-------- Script by Errynei ----------=//
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ComponentFieldReference<>))]
public class ComponentFieldReferenceDrawer : PropertyDrawer
{
    public int propertyDrawerLines = 0;
    public const float SPACING = 2f;
    public float totalBoxSpacing = 0f;

    public SerializedProperty prop_this;
    public SerializedProperty prop_targetGameObject;
    public SerializedProperty prop_targetComponent;
    public SerializedProperty prop_fieldName;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Grab properties
        prop_this = property;
        prop_targetGameObject = property.FindPropertyRelative("EDITOR_targetGameObject");
        prop_targetComponent = property.FindPropertyRelative("targetComponent");
        prop_fieldName = property.FindPropertyRelative("fieldName");

        //Validate inputs
        ValidateValues();

        //-------------------------- Draw Property ----------------------------
        //Initialize
        EditorGUI.BeginProperty(position, label, property);
        position = StartLineCounting(position);


        // ----- Draw Label, and indent -----
        if (label.text != null && label.text != "")
        {
            GUI_Label(position, label.text);
            position = Indent(position);
            position = AdvanceLine(position);
        }
        
        //Draw box around body. 2 expected lines
        position = GUI_DrawBox(position, 2, 5f, 4);

        // ----- Line 1 : Select gameObject | component dropdown -----
        Rect[] halves = DivideRectHorizontally(position, 2);
        GUI_SelectGameObject(halves[0]);
        GUI_SelectComponentDropdown(halves[1]);
        position = AdvanceLine(position);

        // ----- Line 2: Select component | fieldName dropdown -----
        halves = DivideRectHorizontally(position, 2);
        GUI_SelectComponentField(halves[0]);
        GUI_SelectFieldDropdown(halves[1]);
        position = AdvanceLine(position);

        //Finalize
        EditorGUI.EndProperty();
        EndLineCounting();
        //---------------------------------------------------------------------
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LineCountToHeight(propertyDrawerLines) + totalBoxSpacing;
    }

    public Rect GUI_DrawBox(Rect position, int lines, float spacing, int boldness)
    {
        totalBoxSpacing += spacing * 2f;
        Rect boxRect = position;
        boxRect.height = LineCountToHeight(lines) + (spacing * 2f);
        position.y += spacing;
        position.x += spacing;
        position.width -= spacing * 2f;

        for(int i = 0; i < boldness; i++)
            EditorGUI.HelpBox(boxRect, "", MessageType.None);

        return position;
    }
    public void GUI_Label(Rect position, string label)
    {
        EditorGUI.LabelField(position, label);
    }
    public void GUI_SelectGameObject(Rect position)
    {
        EditorGUI.PropertyField(position, prop_targetGameObject, GUIContent.none);

        if (prop_targetGameObject.objectReferenceValue == null)
        {
            prop_targetComponent.objectReferenceValue = null;
            prop_fieldName.stringValue = "";
        }
    }
    public void GUI_SelectComponentDropdown(Rect position)
    {
        if (prop_targetGameObject.objectReferenceValue == null)
        {
            GUI_Label(position, "<Needs Game Object>");
            return;
        }

        GameObject selectedObj = prop_targetGameObject.objectReferenceValue as GameObject;
        Component[] componentsToSelect = selectedObj.GetComponents(typeof(Component));
        string[] componentNames = componentsToSelect.Select(c => c.GetType().Name).ToArray();

        int selectedIndex;

        if (prop_targetComponent.objectReferenceValue == null)
            selectedIndex = -1;
        else
            selectedIndex = Mathf.Max(0, System.Array.IndexOf(componentNames, 
                prop_targetComponent.objectReferenceValue.GetType().Name));

        selectedIndex = EditorGUI.Popup(position, selectedIndex, componentNames);

        if (selectedIndex >= 0)
            prop_targetComponent.objectReferenceValue = componentsToSelect[selectedIndex];
    }
    public void GUI_SelectComponentField(Rect position)
    {
        EditorGUI.PropertyField(position, prop_targetComponent, GUIContent.none);

        if (prop_targetComponent.objectReferenceValue == null)
            prop_fieldName.stringValue = "";
    }
    public void GUI_SelectFieldDropdown(Rect position)
    {
        if (prop_targetComponent.objectReferenceValue == null)
        {
            GUI_Label(position, "<Needs Component>");
            return;
        }

        Type fieldReferenceType = fieldInfo.FieldType.GetGenericArguments()[0];
        Component selectedComponent = prop_targetComponent.objectReferenceValue as Component;

        string[] fieldNames = selectedComponent.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.FieldType == fieldReferenceType)
            .Select(f => f.Name).ToArray();

        if (fieldNames.Length > 0)
        {
            int selectedIndex = Mathf.Max(0, Array.IndexOf(fieldNames, prop_fieldName.stringValue));
            selectedIndex = EditorGUI.Popup(position, selectedIndex, fieldNames);

            if (selectedIndex >= 0)
            {
                prop_fieldName.stringValue = fieldNames[selectedIndex];
            }
        }
        else
        {
            GUI_Label(position, "<No fields found>");
        }
    }
    public void ValidateValues()
    {
        if (prop_targetComponent.objectReferenceValue != null)
        {
            GameObject componentsGameObject = ((Component)prop_targetComponent.objectReferenceValue).gameObject;

            //If component is selected but gameobject is not...
            if (prop_targetGameObject.objectReferenceValue == null)
                //... then use gameobject from component as selected gameobject
                prop_targetGameObject.objectReferenceValue = componentsGameObject;

            //If the component selected is of a different gameobject than the gameobject selected
            else if (prop_targetGameObject.objectReferenceValue != componentsGameObject)
                //... then clear the selected component
                prop_targetComponent.objectReferenceValue = null;
        }
    }

    public Rect StartLineCounting(Rect position)
    {
        totalBoxSpacing = 0f;
        propertyDrawerLines = 1;
        position.height = EditorGUIUtility.singleLineHeight;
        return position;
    }
    public void EndLineCounting()
    {
        propertyDrawerLines--;
    }
    
    public Rect AdvanceLine(Rect position)
    {
        propertyDrawerLines++;
        position.y += EditorGUIUtility.singleLineHeight + SPACING;
        return position;
    }
    public float LineCountToHeight(int lines) =>
        EditorGUIUtility.singleLineHeight * lines + (2 * (lines - 1));

    public Rect Indent(Rect position)
    {
        //Make sure indentAmount is reasonable
        float indentAmount = Mathf.Min(EditorGUIUtility.singleLineHeight, position.width * 0.1f);

        position.x += indentAmount;
        position.width -= indentAmount;

        return position;
    }
    public Rect[] DivideRectHorizontally(Rect position, int divisions)
    {
        Rect[] dividedRects = new Rect[divisions];
        position.width /= divisions;

        for (int i = 0; i < divisions; i++)
        {
            dividedRects[i] = position;
            position.x += position.width;
        }
        return dividedRects;
    }
}