using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;

public class StaticFieldFinder : EditorWindow
{
    [DebugReadOnly] public List<string> staticFields;
    public AssemblyDefinitionAsset assemblyDef;
    private bool displayFields;

    private bool makeInstanceFieldsGrey;


    private Vector2 fieldsScrollPosition;

    [MenuItem("Tools/Find Static Fields")]
    public static void ShowWindow()
    {
        GetWindow<StaticFieldFinder>("Static Field Finder");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;

        assemblyDef = (AssemblyDefinitionAsset)
            EditorGUILayout.ObjectField("Filter", assemblyDef, typeof(AssemblyDefinitionAsset));

        makeInstanceFieldsGrey ^= GUILayout.Button("Make fields named \"Instance\" grey? " + (makeInstanceFieldsGrey ? "ON" : "OFF"));
        
        if (GUILayout.Button("Find All Static Fields"))
        {
            staticFields = FindStaticFields();
            displayFields = false;
        }

        if (displayFields)
        {
            fieldsScrollPosition = GUILayout.BeginScrollView(fieldsScrollPosition);
            foreach (string staticField in staticFields)
            {
                GUILayout.Label(staticField, style);
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Hide fields"))
            {
                displayFields = false;
            }
        }
        else
        {
            if (GUILayout.Button("Display " + staticFields.Count + " fields?"))
            {
                displayFields = true;
            }
        }
    }

    private List<string> FindStaticFields()
    {
        UnityEditor.Compilation.Assembly[] assemblies = CompilationPipeline.GetAssemblies(AssembliesType.Player);
        List<Type> allTypes = new List<Type>();
        foreach(UnityEditor.Compilation.Assembly assembly in assemblies)
        {
            if (assembly.name != assemblyDef.name)
                continue;

            System.Reflection.Assembly currentAssembly = System.Reflection.Assembly.Load(assembly.name);
            allTypes.AddRange(currentAssembly.GetTypes());
        }

        List<string> staticFields = new List<string>();

        foreach (var type in allTypes)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    if (field.IsInitOnly || field.IsLiteral)
                        continue;

                    bool hasAttribute = field.HasAttribute<IsDomainReloadedAttribute>();

                    if (field.IsStatic)
                    {
                        string typeName = "<b>" + type.FullName + "</b>";
                        string fieldName = field.Name;
                        string fieldType = "<size=12>(" + field.FieldType.ToString() + ")</size>";
                        string fullString = typeName + " --- " + fieldName + " --- " + fieldType;

                        if (makeInstanceFieldsGrey && field.Name.ToLower().Contains("instance") && field.FieldType == type)
                            fullString = "<color=grey>" + fullString + "</color>";
                        else if (type.FullName.Contains("<>c"))
                            fullString = "<color=grey>" + fullString + "</color>";
                        else if (hasAttribute)
                            fullString = "<color=green>" + fullString + "</color>";
                        else
                            fullString = "<color=white>" + fullString + "</color>";

                        staticFields.Add(fullString);
                    }
                }
            }
        }
        return staticFields;
    }
}