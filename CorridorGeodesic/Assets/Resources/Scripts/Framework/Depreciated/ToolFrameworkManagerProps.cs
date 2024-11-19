//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: The script is designed to help manage props in a project by providing
// a user-friendly interface for creating, editing, and deleting props. Props are
// represented as scriptable objects, which are stored in a specific folder
// (Assets/Resources/Actors/Props).
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Neverway.Framework
{
    public class ToolFrameworkManagerProps : EditorWindow
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        // Path to the folder where Prop scriptable objects are stored.
        private const string ActorsFolder = "Assets/Resources/Actors/Props";

        // List of actor identifiers for the props.
        private List<string> actors = new List<string>();

        // Name for a new prop to be created.
        //private string _frameworkManager.newActorName = "";
        public bool initialized;


        //=-----------------=
        // Private Variables
        //=-----------------=
        private bool test;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private List<Prop> props = new List<Prop>();


        //=-----------------=
        // Window Functions
        //=-----------------=
        /// <summary>
        /// Show the main window for this script
        /// </summary>
        /// <param name="_frameworkManager"></param>
        public void Window(ToolFrameworkManager _frameworkManager)
        {
            if (!initialized)
            {
                EditorApplication.delayCall += () => GetActorList();
                initialized = true;
            }

            // Long description and guidance for users on how to use this editor window effectively.
            EditorGUILayout.HelpBox(
                "Add and modify the assets that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the prop in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a prop group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)",
                MessageType.None);
            GUILayout.Space(10);

            // Headers
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Icon", GUILayout.Width(65));
            EditorGUILayout.LabelField("Scriptable", GUILayout.MinWidth(135));
            EditorGUILayout.LabelField("ID", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Actor Name", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Associated Prefab", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("", GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            // Actor list
            _frameworkManager.scrollPosition = EditorGUILayout.BeginScrollView(_frameworkManager.scrollPosition);
            ShowActors();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(10);

            // New actor button
            EditorGUILayout.BeginHorizontal();
            _frameworkManager.newActorName = EditorGUILayout.TextField(_frameworkManager.newActorName);
            if (GUILayout.Button("Create New Prop") && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                CreateNewProp(_frameworkManager, _frameworkManager.newActorName);
                _frameworkManager.newActorName = "";
            }

            EditorGUILayout.EndHorizontal();

            // Fix ... buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix IDs"))
            {
                FixPropIDs(_frameworkManager);
            }

            if (GUILayout.Button("Fix Actor Names"))
            {
                FixPropActorNames(_frameworkManager);
            }

            if (GUILayout.Button("Fix Associated Prefabs"))
            {
                FixPropAssociatedPrefabs(_frameworkManager);
            }

            EditorGUILayout.EndHorizontal();

            // Issue check and back buttons
            if (GUILayout.Button("Check for Issues"))
            {
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Back"))
            {
                _frameworkManager.newActorName = "";
                _frameworkManager.currentWindow = "Home";
            }
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=
        /// <summary>
        /// Find all the scriptable objects in the specified asset folder
        /// </summary>
        private void GetActorList()
        {
            actors.Clear();
            props.Clear();
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Prop prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
                if (prop is null) continue;
                actors.Add(prop.id);
                props.Add(prop);
            }

            initialized = false;
        }

        private void ShowActors()
        {
            foreach (Prop prop in props)
            {
                DisplayActorField(prop);
            }
        }

        /// <summary>
        /// Display the fields for the actor
        /// </summary>
        /// <param name="_prop"></param>
        private void DisplayActorField(Prop _prop)
        {
            EditorGUILayout.BeginHorizontal();
            // Icon preview
            if (_prop.icon)
            {
                Texture2D propImage = _prop.icon.texture;
                GUILayout.Label(propImage, GUILayout.Width(25), GUILayout.Height(25));
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
            }

            // Icon Field
            _prop.icon = (Sprite)EditorGUILayout.ObjectField(_prop.icon, typeof(Sprite), false, GUILayout.Width(35));

            // Scriptable
            EditorGUILayout.ObjectField(_prop, typeof(Prop), false, GUILayout.MinWidth(100));

            // Id Field
            _prop.id = EditorGUILayout.TextField(_prop.id);

            // Actor Name Field
            _prop.actorName = EditorGUILayout.TextField(_prop.actorName);

            // Prefab Field
            _prop.AssociatedGameObject =
                (GameObject)EditorGUILayout.ObjectField(_prop.AssociatedGameObject, typeof(GameObject), false);

            // Delete button
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("Delete Prop", "Are you sure you want to delete this prop?", "Yes",
                        "No"))
                {
                    DeleteProp(_prop);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Button assign ids, used to reference object in terminal and scripts, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixPropIDs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Prop prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
                if (prop is not null)
                {
                    prop.id = _frameworkManager.GenerateIdFromActor(prop, "prop");
                }
            }
        }

        /// <summary>
        /// Button assign runtime names, used in menus, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixPropActorNames(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Prop prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
                if (prop is not null)
                {
                    prop.actorName = _frameworkManager.GenerateActorNameFromActor(prop);
                }
            }
        }

        /// <summary>
        /// Button assign game object, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixPropAssociatedPrefabs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Prop prop = AssetDatabase.LoadAssetAtPath<Prop>(AssetDatabase.GUIDToAssetPath(guid));
                if (prop is not null)
                {
                    prop.AssociatedGameObject =
                        _frameworkManager.GetPrefabAtPath(prop.name, ToolFrameworkManager.ObjectsFolder);
                }
            }
        }

        private void DeleteProp(Prop _prop)
        {
            string assetPath = AssetDatabase.GetAssetPath(_prop);
            AssetDatabase.DeleteAsset(assetPath);
            actors.Remove(_prop.id);
            initialized = false;
        }

        private void CreateNewProp(ToolFrameworkManager _frameworkManager, string _propName)
        {
            // Create a new instance of the Prop scriptable object
            Prop newProp = CreateInstance<Prop>();

            // Set the name of the new prop
            newProp.name = _propName;
            _frameworkManager.newActorName = "";

            // Create asset file for the new prop
            string assetPath = $"{ActorsFolder}/{_propName}.asset";
            AssetDatabase.CreateAsset(newProp, assetPath);
            initialized = false;
        }
    }
}
