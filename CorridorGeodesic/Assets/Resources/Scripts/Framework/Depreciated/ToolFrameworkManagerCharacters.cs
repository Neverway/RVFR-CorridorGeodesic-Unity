//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: The script is designed to help manage characters in a project by providing
// a user-friendly interface for creating, editing, and deleting characters. Characters are
// represented as scriptable objects, which are stored in a specific folder
// (Assets/Resources/Actors/Characters).
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Neverway.Framework.Customs;


namespace Neverway.Framework
{
    public class ToolFrameworkManagerCharacters : EditorWindow
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        // Path to the folder where Character scriptable objects are stored.
        private const string ActorsFolder = "Assets/Resources/Actors/Characters";

        // List of actor identifiers for the characters.
        private List<string> actors = new List<string>();

        // Name for a new character to be created.
        //private string _frameworkManager.newActorName = "";
        public bool initialized;


        //=-----------------=
        // Private Variables
        //=-----------------=
        private bool test;


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private List<CharacterData> characters = new List<CharacterData>();


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
                "Add and modify the assets that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the character in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a character group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)",
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
            if (GUILayout.Button("Create New Character") && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                CreateNewCharacter(_frameworkManager, _frameworkManager.newActorName);
                _frameworkManager.newActorName = "";
            }

            EditorGUILayout.EndHorizontal();

            // Fix ... buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix IDs"))
            {
                FixCharacterIDs(_frameworkManager);
            }

            if (GUILayout.Button("Fix Actor Names"))
            {
                FixCharacterActorNames(_frameworkManager);
            }

            if (GUILayout.Button("Fix Associated Prefabs"))
            {
                FixCharacterAssociatedPrefabs(_frameworkManager);
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
            characters.Clear();
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                CharacterData character =
                    AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
                if (character is null) continue;
                actors.Add(character.id);
                characters.Add(character);
            }

            initialized = false;
        }

        private void ShowActors()
        {
            foreach (CharacterData character in characters)
            {
                DisplayActorField(character);
            }
        }

        /// <summary>
        /// Display the fields for the actor
        /// </summary>
        /// <param name="_character"></param>
        private void DisplayActorField(CharacterData _character)
        {
            EditorGUILayout.BeginHorizontal();
            // Icon preview
            if (_character.icon)
            {
                Texture2D characterImage = _character.icon.texture;
                GUILayout.Label(characterImage, GUILayout.Width(25), GUILayout.Height(25));
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
            }

            // Icon Field
            _character.icon =
                (Sprite)EditorGUILayout.ObjectField(_character.icon, typeof(Sprite), false, GUILayout.Width(35));

            // Scriptable
            EditorGUILayout.ObjectField(_character, typeof(CharacterData), false, GUILayout.MinWidth(100));

            // Id Field
            _character.id = EditorGUILayout.TextField(_character.id);

            // Actor Name Field
            _character.actorName = EditorGUILayout.TextField(_character.actorName);

            // Prefab Field
            _character.AssociatedGameObject =
                (GameObject)EditorGUILayout.ObjectField(_character.AssociatedGameObject, typeof(GameObject), false);

            // Hide Field
            //_character.hideFromBuild = GUILayout.Toggle(_character.hideFromBuild, "", GUILayout.Width(15));

            // Delete button
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("Delete Character", "Are you sure you want to delete this character?",
                        "Yes", "No"))
                {
                    DeleteCharacter(_character);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Button assign ids, used to reference object in terminal and scripts, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixCharacterIDs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                CharacterData character =
                    AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
                if (character is not null)
                {
                    character.id = _frameworkManager.GenerateIdFromActor(character, "character");
                }
            }
        }

        /// <summary>
        /// Button assign runtime names, used in menus, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixCharacterActorNames(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                CharacterData character =
                    AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
                if (character is not null)
                {
                    character.actorName = _frameworkManager.GenerateActorNameFromActor(character);
                }
            }
        }

        /// <summary>
        /// Button assign game object, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixCharacterAssociatedPrefabs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                CharacterData character =
                    AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
                if (character is not null)
                {
                    character.AssociatedGameObject =
                        _frameworkManager.GetPrefabAtPath(character.name, ToolFrameworkManager.ObjectsFolder);
                }
            }
        }

        private void DeleteCharacter(CharacterData _character)
        {
            string assetPath = AssetDatabase.GetAssetPath(_character);
            AssetDatabase.DeleteAsset(assetPath);
            actors.Remove(_character.id);
            initialized = false;
        }

        private void CreateNewCharacter(ToolFrameworkManager _frameworkManager, string _characterName)
        {
            // Create a new instance of the Character scriptable object
            CharacterData newCharacter = CreateInstance<CharacterData>();

            // Set the name of the new character
            newCharacter.name = _characterName;
            _frameworkManager.newActorName = "";

            // Create asset file for the new character
            string assetPath = $"{ActorsFolder}/{_characterName}.asset";
            AssetDatabase.CreateAsset(newCharacter, assetPath);
            initialized = false;
        }
    }
}