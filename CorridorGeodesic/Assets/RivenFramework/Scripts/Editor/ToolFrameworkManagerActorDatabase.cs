//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Neverway.Framework
{
    public class ToolFrameworkManagerActorDatabase : EditorWindow
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        [Tooltip("This is the path to the scriptable objects that represent each actor in the project")]
        private const string ProjectActorsFolder = "Assets/Resources/Objects/Data";
        [Tooltip("This is the path to the scriptable objects that represent each actor in the framework")]
        private const string FrameworkActorsFolder = "Assets/Resources/Objects/Data";


        //=-----------------=
        // Private Variables
        //=-----------------=
        private List<string> actors = new List<string>();
        public bool initialized;
        
        int         _selected   = 0;
        string[]    _options    = new string[9] { "All", "Framework", "Project", "Prop", "PhysProp", "FuncProp", "Pawn", "Volume", "Logic" };
        int         _selectedType   = 0;
        string[]    _actorTypes    = new string[6] { "Prop", "PhysProp", "FuncProp", "Pawn", "Volume", "Logic" };
        GameObject         _associatedObject   = null;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private List<Actor> actorDataObjects = new List<Actor>();


        //=-----------------=
        // Mono Functions
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
            EditorGUILayout.HelpBox("Add and modify the actors that appear in your project. This data is used when creating or loading cartographer maps, or when new actors are spawned in. The Icon represents how the actor appears when viewing it in things like the cartographer asset library, Good Luck! ~Liz)",
                MessageType.None);
            GUILayout.Space(10);
            
            // Sort Dropdown
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            _selected = EditorGUILayout.Popup("", _selected, _options);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log(_options[_selected]);
            }
            EditorGUILayout.EndHorizontal();

            // Headers
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(10));
            EditorGUILayout.LabelField("Icon", GUILayout.Width(65));
            EditorGUILayout.LabelField("Data", GUILayout.MinWidth(135));
            EditorGUILayout.LabelField("ID", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Actor Name", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Associated Object", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("", GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();
            var test = new GUIContent("");
            
            // Select All Button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(false, "Select All"))
            {
            }
            EditorGUILayout.EndHorizontal();

            // Actor list
            _frameworkManager.scrollPosition = EditorGUILayout.BeginScrollView(_frameworkManager.scrollPosition);
            ShowActors();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(10);
            
            
            // New actor button
            EditorGUILayout.LabelField("Selected Actors", GUILayout.Width(150));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix IDs", GUILayout.Width(150)) && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                if (EditorUtility.DisplayDialog(
                        "Reassign Selected IDs?", 
                        "Are you sure you want to rename this Actors ID? Map files that reference this actor by this ID will need to be updated!",
                        "Yes", "No"))
                {
                    Debug.Log("Reassign");
                }
            }
            if (GUILayout.Button("Fix Name", GUILayout.Width(150)) && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
            }
            if (GUILayout.Button("Delete", GUILayout.Width(150)) && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                if (EditorUtility.DisplayDialog(
                        "Delete Selected Actors?", 
                        "Are you sure you want to delete this Actors? This will only remove the actor entry from the database and will not delete the associated object.",
                        "Yes", "No"))
                {
                    Debug.Log("Deleted");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField("Create New Actor", GUILayout.Width(150));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Actor Type", GUILayout.Width(150));
            EditorGUILayout.LabelField("Actor Name");
            EditorGUILayout.LabelField("Associated Object");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _selectedType = EditorGUILayout.Popup("", _selectedType, _actorTypes, GUILayout.Width(150));
            _frameworkManager.newActorName = EditorGUILayout.TextField(_frameworkManager.newActorName);
            _associatedObject = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false);
            if (GUILayout.Button("Create New Actor", GUILayout.Width(150)) && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                //CreateNewCharacter(_frameworkManager, _frameworkManager.newActorName);
                _frameworkManager.newActorName = "";
                _associatedObject = null;
            }

            EditorGUILayout.EndHorizontal();

            // New actor button
            /*EditorGUILayout.BeginHorizontal();
            _frameworkManager.newActorName = EditorGUILayout.TextField(_frameworkManager.newActorName);
            if (GUILayout.Button("Create New Character") && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                //CreateNewCharacter(_frameworkManager, _frameworkManager.newActorName);
                _frameworkManager.newActorName = "";
            }

            EditorGUILayout.EndHorizontal();

            // Fix ... buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix IDs"))
            {
                //FixCharacterIDs(_frameworkManager);
            }

            if (GUILayout.Button("Fix Actor Names"))
            {
                ///FixCharacterActorNames(_frameworkManager);
            }

            if (GUILayout.Button("Fix Associated Prefabs"))
            {
                //FixCharacterAssociatedPrefabs(_frameworkManager);
            }

            EditorGUILayout.EndHorizontal();

            // Issue check and back buttons
            if (GUILayout.Button("Check for Issues"))
            {
            }*/

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
            actorDataObjects.Clear();
            string[] guidList = AssetDatabase.FindAssets("", new[] { FrameworkActorsFolder });
            foreach (string guid in guidList)
            {
                Actor character =
                    AssetDatabase.LoadAssetAtPath<Actor>(AssetDatabase.GUIDToAssetPath(guid));
                if (character is null) continue;
                actors.Add(character.id);
                actorDataObjects.Add(character);
            }

            initialized = false;
        }

        private void ShowActors()
        {
            foreach (Actor character in actorDataObjects)
            {
                DisplayActorField(character);
            }
        }

        /// <summary>
        /// Display the fields for the actor
        /// </summary>
        /// <param name="_actor"></param>
        private void DisplayActorField(Actor _actor)
        {
            EditorGUILayout.BeginHorizontal();
            // Select Button
            if (GUILayout.Toggle(false, "", GUILayout.Width(25), GUILayout.Height(25)))
            {
            }
            
            // Icon preview
            if (_actor.icon)
            {
                Texture2D characterImage = _actor.icon.texture;
                GUILayout.Label(characterImage, GUILayout.Width(25), GUILayout.Height(25));
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
            }

            // Icon Field
            _actor.icon =
                (Sprite)EditorGUILayout.ObjectField(_actor.icon, typeof(Sprite), false, GUILayout.Width(35));

            // Scriptable
            EditorGUILayout.ObjectField(_actor, typeof(Actor), false, GUILayout.MinWidth(100));

            // Id Field
            _actor.id = EditorGUILayout.TextField(_actor.id);

            // Actor Name Field
            _actor.actorName = EditorGUILayout.TextField(_actor.actorName);

            // Prefab Field
            _actor.AssociatedGameObject =
                (GameObject)EditorGUILayout.ObjectField(_actor.AssociatedGameObject, typeof(GameObject), false);

            // Hide Field
            //_actor.hideFromBuild = GUILayout.Toggle(_actor.hideFromBuild, "", GUILayout.Width(15));

            // Delete button
            /*if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("Delete Character", "Are you sure you want to delete this character?",
                        "Yes", "No"))
                {
                    //DeleteCharacter(_actor);
                }
            }*/

            EditorGUILayout.EndHorizontal();
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}