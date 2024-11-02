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
    string showHandle = "editorHandles_showHandle";
    string handle = "editorHandles_handleTarget";
    string customName = "editorHandles_customName";

    string _hideTypeFilterText = "EDITOR_hideTypeFilterText";
    string _fieldName = "fieldName";
    string _targetComponent = "targetComponent";

    bool showHandleField = false;

    LogicInput underlyingValue;
    public bool HasOutputTarget => underlyingValue.HasLogicOutputSource;

    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        area.x += 2;
        if (underlyingValue == null)
            underlyingValue = property.Property.GetUnderlyingValue() as LogicInput;

        if (!property[sourceField][_hideTypeFilterText].Bool)
        {
            property[sourceField][_hideTypeFilterText].Property.boolValue = true;
            SetModified = true;
        }
        bool hasOutput = HasOutputTarget;
        if (Application.isPlaying && hasOutput)
        {
            
        }


        DrawerObject valueDrawer = new Disable(new Property(property[value]).HideLabel(), hasOutput);
        FittedLabel fieldValueWithLabel = 
        new FittedLabel(property.Property.displayName + " ->", valueDrawer).Padding(4).Big().AlignUpperLeft() as FittedLabel;


        contents.Add(new SizedHorizontalGroup(fieldValueWithLabel)
            .AddOnRight(new Button("H", ToggleHandleEditing).AlignCenter(), 22)
            .AddOnRight(new EmptySpace(), 3));

        contents.Add(new EmptySpace(5));
        contents.Add(new FittedLabel(Text_LinkedToOutput, new Property(property[sourceField])).Padding(4f).UseRichText());

        if (showHandleField)
        {
            contents.Add(new Divider());
            VerticalGroup handleEditor = new VerticalGroup();

            handleEditor.Add(new Property(property[showHandle]).Label("Show Handle in Editor? "));
            if (property[showHandle].Bool)
            {
                handleEditor.Add(new Property(property[handle]).Label("Handle Transform :"));
                handleEditor.Add(new Property(property[customName]).Label("Handle Custom Name :"));
            }
            Boxed boxedHandleEditor = new Boxed(handleEditor);
            contents.Add(boxedHandleEditor);
            contents.Add(new Divider());
        }

        Boxed boxedContents = new Boxed(contents);
        boxedContents.boldness = 2;
        return boxedContents;
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

    public void ToggleHandleEditing() => showHandleField = !showHandleField;

    //    (!string.IsNullOrEmpty(property[sourceField][_fieldName].Property.stringValue))
    //    && property[sourceField][_targetComponent].Property.boxedValue != null;
    public string Text_LinkedToOutput => 
        $"<size=10>Link to </size>" +
        $"{property[sourceField].FieldType.Generic(0).Generic(0).HumanName(true)} " +
        $"<size=10>Output: </size>";
}
