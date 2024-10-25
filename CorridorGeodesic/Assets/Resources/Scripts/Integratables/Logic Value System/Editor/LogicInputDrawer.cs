using Neverway.Framework.LogicValueSystem;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[CustomPropertyDrawer(typeof(LogicInput<>))]
public class LogicInputDrawer : EasyDrawer
{
    string sourceField = "sourceField";
    string value = "value";
    string handle = "handleTarget";

    string _hideTypeFilterText = "EDITOR_hideTypeFilterText";
    string _fieldName = "fieldName";
    string _targetComponent = "targetComponent";

    bool showHandleField = true; 

    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        area.x += 2;

        if (!property[sourceField][_hideTypeFilterText].Bool)
        {
            property[sourceField][_hideTypeFilterText].Property.boolValue = true;
            SetModified = true;
        }

        DrawerObject valueDrawer = new Disable(new Property(property[value]).HideLabel(), HasOutputTarget);
        FittedLabel fieldValueWithLabel = 
        new FittedLabel(property.Property.displayName + " ->", valueDrawer).Padding(4).Big().AlignUpperLeft() as FittedLabel;

        contents.Add(fieldValueWithLabel);
        contents.Add(new Divider());

        if (showHandleField)
        {
            contents.Add(new Property(property[handle]));
            contents.Add(new Divider());
        }

        contents.Add(new FittedLabel(Text_LinkedToOutput, new Property(property[sourceField])).Padding(4f).UseRichText());

        return new Boxed(contents);
    }
    public override void OnBeforeFinishGUI(Rect area) 
    {
        area.width = 16;
        area.height = 16;
        area.y += 4;

        GUIContent content = new GUIContent("", EditorGUIUtility.IconContent("Profiler.NextFrame").image);
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.contentOffset = Vector2.left * 14f;

        EditorGUI.LabelField(area, content, style);
    }
    public override void OnAfterGUI() { }

    public bool HasOutputTarget =>
        (!string.IsNullOrEmpty(property[sourceField][_fieldName].Property.stringValue)) 
        && property[sourceField][_targetComponent].Property.objectReferenceValue != null;
    public string Text_LinkedToOutput => 
        $"<size=10>Link to </size>" +
        $"{property[sourceField].FieldType.Generic(0).Generic(0).HumanName(true)} " +
        $"<size=10>Output: </size>";
}
