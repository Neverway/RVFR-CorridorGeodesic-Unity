using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using EasyInspector;

[CustomPropertyDrawer(typeof(SimplifiedFields.FieldContainer))]
public class SimplifiedFieldsPropertiesEditor : EasyDrawer
{
    bool pickFieldsMode = false;
    static DrawerObject space = new EmptySpace(5);
    EasyProperty mainProperty;
    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        DrawerObject pickFieldsModeButton = 
            new Button(pickFieldsMode ? "Back to Editing Fields" : "Pick Fields", TogglePickFieldsMode).AlignCenter();

        mainProperty = new EasyProperty(property.Property);

        if (pickFieldsMode)
        {
            contents.Add(new Property(mainProperty["categories"]).IncludeChildren());
        }
        else
            contents.Add(GetSimplifiedFields(mainProperty));

        contents.Add(new Divider());
        contents.Add(new HorizontalGroup().Add(space).Add(space).Add(pickFieldsModeButton));

        return contents;
    }
    
    public static DrawerObject GetSimplifiedFields(EasyProperty mainProperty)
    {
        VerticalGroup contents = new VerticalGroup();
        foreach (EasyProperty categoryProp in mainProperty["categories"])
        {
            if (categoryProp["fields"].Count == 0)
                continue;

            string title = categoryProp["categoryName"].AsString;
            if (!string.IsNullOrEmpty(title))
            {
                Label categoryHeader = new Label(title);
                categoryHeader.Bold().style.fontSize = 16;
                contents.Add(categoryHeader);
            }

            VerticalGroup categoryContent = new VerticalGroup();

            foreach (EasyProperty fieldProp in categoryProp["fields"])
            {
                Component selectedComponent = fieldProp["targetComponent"].Get<Component>();

                if (selectedComponent == null)
                {
                    categoryContent.Add(GetErrorLabel("Component Not Found"));
                    continue;
                }

                string selectedFieldName = fieldProp["fieldName"].AsString;

                if (string.IsNullOrEmpty(selectedFieldName))
                {
                    categoryContent.Add(GetErrorLabel("No Field Selected"));
                    continue;
                }

                EasyProperty selectedProperty = new EasyProperty(selectedComponent, selectedFieldName);

                if (selectedProperty.Property == null)
                {
                    categoryContent.Add(GetErrorLabel("Invalid Field Selected"));
                    continue;
                }

                categoryContent.Add(new Property(selectedProperty)
                    .IncludeChildren()
                    .UpdateSerializedObject()
                    );
            }
            contents.Add(new Boxed(categoryContent));
            contents.Add(space);
        }
        return contents;
    }
    
    public static DrawerObject GetErrorLabel(string errorText)
    {
        errorText = "<b><i><color=#dd3333ff>Error: " + errorText + "</color></i></b>";
        Divider line = new Divider().Color(new Color(0.866f, 0.2f, 0.2f, 0.5f));
        FittedLabel warningLabel = new FittedLabel(errorText, line).AndBeforeLabel(line);
        warningLabel.UseRichText().AlignCenter();
        return warningLabel;
    }

    public override void OnAfterGUI() 
    {
        Component self = mainProperty.SerializedObject.targetObject as Component;

        foreach (EasyProperty category in mainProperty["categories"])
        {
            foreach(EasyProperty fieldRef in category["fields"])
            {
                GameObject gameObject = fieldRef["targetGameObject"].AsUnityObject as GameObject;
                if (gameObject == null)
                    continue;

                if (!gameObject.transform.IsChildOf(self.transform))
                {
                    fieldRef["targetGameObject"].AsUnityObject = null;
                    fieldRef["targetComponent"].AsUnityObject = null;
                    fieldRef["fieldName"].AsString = null;
                }

                if (fieldRef["targetComponent"].AsUnityObject is SimplifiedFields)
                {
                    fieldRef["targetComponent"].AsUnityObject = null;
                    fieldRef["fieldName"].AsString = null;
                }

                if (fieldRef["targetComponent"].AsUnityObject is Transform)
                {
                    fieldRef["targetComponent"].AsUnityObject = null;
                    fieldRef["fieldName"].AsString = null;
                }
            }
        }
    }

    public void TogglePickFieldsMode() => pickFieldsMode = !pickFieldsMode;
}

//[CustomPropertyDrawer(typeof(SimplifiedFields.Category))]
public class SimplifiedFieldsCategoryEditor : EasyDrawer
{
    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        DrawerObject space = new EmptySpace(5);

        contents.Add(new FittedLabel("Category Name: ", new Property(property["categoryName"]).HideLabel()));

        SerializedProperty arraySizeProp = fieldsProp.FindPropertyRelative("Array.size");
        contents.Add(new Label("Is Array: " + fieldsProp.isArray));
        contents.Add(new Property(arraySizeProp));

        for (int i = 0; i < arraySizeProp.intValue; i++)
        {
            SerializedProperty element = fieldsProp.GetArrayElementAtIndex(i);
            contents.Add(new SizedHorizontalGroup(new Property(element).IncludeChildren())
                .AddOnRight(new Button("-", () => Remove(i)), 24)
                );
        }
        contents.Add(new HorizontalGroup().Add(space).Add(new Button("+", AddNew)).Add(space));

        return new Boxed(contents);
    }
    public SerializedProperty fieldsProp => property["fields"].Property;
    public void AddNew()
    {
        fieldsProp.InsertArrayElementAtIndex(fieldsProp.arraySize);
        fieldsProp.serializedObject.ApplyModifiedProperties();
    }
    public void Remove(int i)
    {
        fieldsProp.DeleteArrayElementAtIndex(i);
        fieldsProp.serializedObject.ApplyModifiedProperties();
    }

    public override void OnAfterGUI() { }
}

[CustomPropertyDrawer(typeof(SimplifiedFields.FieldReference))]
public class SimplifiedFieldsFieldReferenceEditor : EasyDrawer
{
    public string targetGameObject = nameof(SimplifiedFields.FieldReference.targetGameObject);
    public string targetComponent = nameof(SimplifiedFields.FieldReference.targetComponent);
    public string fieldName = nameof(SimplifiedFields.FieldReference.fieldName);

    public Component oldComponent;
    private EasyProperty mainProperty;
    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        HorizontalGroup line = new HorizontalGroup();

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

        contents.Add(line);

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
        //line.Add(new Property(property[targetGameObject]).HideLabel());
        line.Add(new Property(property[targetComponent]).HideLabel());
    }
    public void GUISelectComponentFromGameObject(HorizontalGroup line)
    {
        line.Add(new Property(property[targetGameObject]).HideLabel());
        line.Add(new SelectComponentFromGameObject(
            (GameObject)property[targetGameObject].Property.objectReferenceValue,
            property[targetComponent])
            );
    }
    public void GUISelectField(HorizontalGroup line)
    {
        line.Add(new Property(property[targetComponent]).HideLabel());
        line.Add(new SelectFieldDropdown(
            property[targetComponent].Property.objectReferenceValue,
            property[fieldName])
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

}
