using Neverway.Framework.LogicValueSystem;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

[CustomPropertyDrawer(typeof(LogicOutput<>))]
public class LogicOutputDrawer : EasyDrawer
{
    string value = "value";
    string OnOutputChanged = "OnOutputChanged";
    string handle = "editorHandles_handleTarget";
    string customName = "editorHandles_customName";
    bool showHandleField = true;

    public override DrawerObject OnGUIEasyDrawer(VerticalGroup contents)
    {
        DrawerObject valueDrawer = new Property(property[value]).HideLabel();
        FittedLabel fieldValueWithLabel = 
            new FittedLabel("<- " + property.Property.displayName, valueDrawer).Padding(6).Big().AlignUpperLeft() as FittedLabel;

        contents.Add(fieldValueWithLabel);
        contents.Add(new Divider());

        if (showHandleField)
        {
            contents.Add(new Property(property[handle]));
            contents.Add(new Property(property[customName]));
            contents.Add(new Divider());
        }

        contents.Add(new Property(property[OnOutputChanged]));

        return new Boxed(contents);
    }
    public override void OnBeforeFinishGUI(Rect area)
    {
        area.width = 16;
        area.height = 16;
        area.y += 4;

        GUIContent content = new GUIContent("", EditorGUIUtility.IconContent("Profiler.PrevFrame").image);
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.contentOffset = Vector2.left * 14f;

        EditorGUI.LabelField(area, content, style);
    }
    public override void OnAfterGUI() { }
}