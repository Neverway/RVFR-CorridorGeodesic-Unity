using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugReadOnlyAttribute : PropertyAttribute
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DebugReadOnlyAttribute))]
public class DebugReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool previouslyEnabled = GUI.enabled;
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = previouslyEnabled;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif

public class BIGLABELAttribute : PropertyAttribute
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BIGLABELAttribute))]
public class BIGLABELDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldFontSize = EditorStyles.label.fontSize;
        EditorStyles.label.fontSize = 30;
        EditorGUI.PropertyField(position, property, label, true);
        EditorStyles.label.fontSize = oldFontSize;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif