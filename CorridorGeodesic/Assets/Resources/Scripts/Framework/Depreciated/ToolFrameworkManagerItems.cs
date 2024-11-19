//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: The script is designed to help manage items in a project by providing
// a user-friendly interface for creating, editing, and deleting items. Items are
// represented as scriptable objects, which are stored in a specific folder
// (Assets/Resources/Actors/Items).
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neverway.Framework
{
    public class ToolFrameworkManagerItems : EditorWindow
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        // Path to the folder where Item scriptable objects are stored.
        private const string ActorsFolder = "Assets/Resources/Actors/Items";

        // List of actor identifiers for the items.
        private List<string> actors = new List<string>();

        // Name for a new item to be created.
        //private string _frameworkManager.newActorName = "";
        public bool initialized;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private List<Item> items = new List<Item>();


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
                "Add and modify the items that appear in your project. This will only handel the scriptable object data, you will still need to create a prefab with the same name as the item in /Resources/Actors/Objects! Also don't forget to add each of these scriptables to a item group in the GameInstance prefab's AssetData script found in /Resources/Actors/System. (Sorry, I know this a bit clunky right now but in the future this tool will be able to assign actors to the asset database. Good Luck! ~Liz)",
                MessageType.None);
            GUILayout.Space(10);

            // Headers
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Icon", GUILayout.Width(65));
            EditorGUILayout.LabelField("Scriptable", GUILayout.MinWidth(135));
            EditorGUILayout.LabelField("ID", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Actor Name", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Associated Prefab", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Stack Count", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("Description", GUILayout.MinWidth(100));
            EditorGUILayout.LabelField("", GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            // Actor list
            _frameworkManager.scrollPosition = EditorGUILayout.BeginScrollView(_frameworkManager.scrollPosition);
            ShowActors();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(10);

            // New actor button
            EditorGUILayout.BeginHorizontal();
            string[] types = new string[]
            {
                "Utility Throwable",
                "Utility Consumable",
                "Weapon Bladed",
                "Weapon Bludgeoning",
                "Weapon Polearm",
                "Weapon Projectile",
                "Weapon Hitscan",
                "Magic",
                "Defense",
                "Defense Head",
                "Defense Chest",
                "Defense Back",
                "Defense Legs",
                "Defense Feet",
                "Other"
            };
            int selectedIndex = Array.IndexOf(types, _frameworkManager.selectedItemType);
            selectedIndex = EditorGUILayout.Popup("Type", selectedIndex, types);
            _frameworkManager.selectedItemType = types[selectedIndex];
            _frameworkManager.newActorName = EditorGUILayout.TextField(_frameworkManager.newActorName);
            if (GUILayout.Button("Create New Item") && !string.IsNullOrEmpty(_frameworkManager.newActorName))
            {
                CreateNewItem(_frameworkManager, _frameworkManager.newActorName);
                _frameworkManager.newActorName = "";
            }

            EditorGUILayout.EndHorizontal();

            // Fix ... buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fix IDs"))
            {
                FixItemIDs(_frameworkManager);
            }

            if (GUILayout.Button("Fix Actor Names"))
            {
                FixItemActorNames(_frameworkManager);
            }

            if (GUILayout.Button("Fix Associated Prefabs"))
            {
                FixItemAssociatedPrefabs(_frameworkManager);
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
            items.Clear();
            actors.Clear();
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
                if (item is null) continue;
                actors.Add(item.id);
                items.Add(item);
            }

            initialized = false;
        }

        private void ShowActors()
        {
            foreach (Item item in items)
            {
                DisplayActorField(item);
            }
        }

        /// <summary>
        /// Display the fields for the actor
        /// </summary>
        /// <param name="_item"></param>
        private void DisplayActorField(Item _item)
        {
            EditorGUILayout.BeginHorizontal();

            // Icon preview
            if (_item.icon)
            {
                Texture2D itemImage = _item.icon.texture;
                GUILayout.Label(itemImage, GUILayout.Width(25), GUILayout.Height(25));
            }
            else
            {
                GUILayout.Label(GUIContent.none, GUILayout.Width(25), GUILayout.Height(25));
            }

            // Icon Field
            _item.icon = (Sprite)EditorGUILayout.ObjectField(_item.icon, typeof(Sprite), false, GUILayout.Width(35));

            // Scriptable
            EditorGUILayout.ObjectField(_item, typeof(Item), false, GUILayout.MinWidth(100));

            // Id Field
            _item.id = EditorGUILayout.TextField(_item.id);

            // Actor Name Field
            _item.actorName = EditorGUILayout.TextField(_item.actorName);

            // Prefab Field
            _item.AssociatedGameObject =
                (GameObject)EditorGUILayout.ObjectField(_item.AssociatedGameObject, typeof(GameObject), false);

            // Stack Count Field
            _item.stackCount = EditorGUILayout.IntField(_item.stackCount);

            // Description Field
            _item.description = EditorGUILayout.TextField(_item.description);

            // Delete button
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                if (EditorUtility.DisplayDialog("Delete Item", "Are you sure you want to delete this item?", "Yes",
                        "No"))
                {
                    DeleteItem(_item);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Button assign ids, used to reference object in terminal and scripts, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixItemIDs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (var guid in guidList)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
                if (item is not null)
                {
                    item.id = _frameworkManager.GenerateIdFromActor(item, "item");
                }
            }
        }

        /// <summary>
        /// Button assign runtime names, used in menus, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixItemActorNames(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
                if (item is not null)
                {
                    item.actorName = _frameworkManager.GenerateActorNameFromActor(item);
                }
            }
        }

        /// <summary>
        /// Button assign game object, to all found actors
        /// </summary>
        /// <param name="_frameworkManager"></param>
        private void FixItemAssociatedPrefabs(ToolFrameworkManager _frameworkManager)
        {
            string[] guidList = AssetDatabase.FindAssets("", new[] { ActorsFolder });
            foreach (string guid in guidList)
            {
                Item item = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(guid));
                if (item is not null)
                {
                    item.AssociatedGameObject =
                        _frameworkManager.GetPrefabAtPath(item.name, ToolFrameworkManager.ObjectsFolder);
                }
            }
        }

        private void DeleteItem(Item _item)
        {
            string assetPath = AssetDatabase.GetAssetPath(_item);
            AssetDatabase.DeleteAsset(assetPath);
            actors.Remove(_item.id);
            initialized = false;
        }

        private void CreateNewItem(ToolFrameworkManager _frameworkManager, string _itemName)
        {
            Item newItem = _frameworkManager.selectedItemType switch
            {
                "Utility Throwable" => CreateInstance<Item_Utility_Throwable>(),
                "Utility Consumable" => CreateInstance<Item_Utility_Consumable>(),
                "Weapon Bladed" => CreateInstance<Item_Weapon_Bladed>(),
                "Weapon Bludgeoning" => CreateInstance<Item_Weapon_Bludgeoning>(),
                "Weapon Polearm" => CreateInstance<Item_Weapon_Polearm>(),
                "Weapon Projectile" => CreateInstance<Item_Weapon_Projectile>(),
                "Weapon Hitscan" => CreateInstance<Item_Weapon_Hitscan>(),
                "Magic" => CreateInstance<Item_Magic>(),
                "Defense" => CreateInstance<Item_Defense>(),
                "Defense Head" => CreateInstance<Item_Defense_Head>(),
                "Defense Chest" => CreateInstance<Item_Defense_Chest>(),
                "Defense Back" => CreateInstance<Item_Defense_Back>(),
                "Defense Legs" => CreateInstance<Item_Defense_Legs>(),
                "Defense Feet" => CreateInstance<Item_Defense_Feet>(),
                _ => CreateInstance<Item>()
            };

            if (newItem is null)
            {
                Debug.LogError("Failed to create new item: unknown type " + _frameworkManager.selectedItemType);
                return;
            }

            // Set the name of the new item
            newItem.name = _itemName;
            _frameworkManager.newActorName = "";

            // Create asset file for the new item
            string assetPath = $"{ActorsFolder}/{_itemName}.asset";
            AssetDatabase.CreateAsset(newItem, assetPath);
            initialized = false;
        }
    }
}
