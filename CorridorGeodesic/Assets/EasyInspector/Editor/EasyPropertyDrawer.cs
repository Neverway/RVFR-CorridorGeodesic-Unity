using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static EasyDrawer;

namespace EasyInspector
{
    public abstract class EasyPropertyDrawer : PropertyDrawer
    {
        public float propertyHeight;
        public Rect area;
        public EasyProperty property;
        public GUIContent label;

        private List<string> alreadyDisplayedErrors = new();
        private static bool propertiesWereModified = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = new EasyProperty(property);
            this.label = label;
            this.area = position;

            DrawerObject.currentlyDrawing = new();

            EditorGUI.BeginProperty(position, label, property);
            try
            {
                DrawerObject contents = OnGUIEasyDrawer(new VerticalGroup());

                position.height = propertyHeight = contents.GetHeight();

                contents.Draw(position);

                OnBeforeFinishGUI(position);
            }
            catch (Exception e)
            {
                if (e is not ExitGUIException && !(string.IsNullOrEmpty(e.StackTrace) || alreadyDisplayedErrors.Contains(e.StackTrace)))
                {
                    UnityEngine.Object unityObject = property.serializedObject.targetObject;
                    try
                    {
                        string errorMessage =
                            $"<size=10><color=yellow>Error drawing property drawer. Context for next error:</color></size>" +
                            $"   Type: {property.GetUnderlyingType().SelectedName(false, true)}" +
                            $"  |  Field: {property.displayName}  " +
                            $"  |  Object: {unityObject}" +
                            $"\n{DrawerObject.CurrentDrawerChainNames()}";

                        alreadyDisplayedErrors.Add(e.StackTrace);

                        Debug.LogError(errorMessage, unityObject);
                    }
                    catch
                    {
                        Debug.Log(property.GetUnderlyingField());
                        Debug.LogWarning($"Had error trying to display extra context information for property drawer error???", unityObject);
                    }

                    Debug.LogException(e, unityObject);
                }
            }
            EditorGUI.EndProperty();
            OnAfterGUI();

            if (propertiesWereModified)
            {
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                propertiesWereModified = false;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            Mathf.Max(propertyHeight, EditorGUIUtility.singleLineHeight);

        public abstract DrawerObject OnGUIEasyDrawer(VerticalGroup contents);
        public virtual void OnBeforeFinishGUI(Rect position) { }
        public abstract void OnAfterGUI();

        public static bool SetModified { set { propertiesWereModified |= value; } }

    }


}