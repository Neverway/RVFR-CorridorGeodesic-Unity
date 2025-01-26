using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomThumbnailRenderer
{
    static CustomThumbnailRenderer()
    {
        // Make sure we unbind first since we could bind twice when scripts are reloaded
        EditorApplication.projectWindowItemOnGUI -= ProjectWindowItemOnGUICallback;
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUICallback;
    }
    static void ProjectWindowItemOnGUICallback(string guid, Rect selectionRect)
    {
        string assetPath = AssetDatabase.GUIDToAssetPath(guid);
        // If we're a directory, ignore it.
        if (assetPath.LastIndexOf('.') == -1)
        {
            return;
        }

        // If we're not a prefab, ignore it
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        if (go == null)
        {
            return;
        }

        if (assetPath.EndsWith(".prefab"))
        {
            string fileName = assetPath.Replace(".prefab", ".png");
            string thumbPath = Application.dataPath + "/Scenes/Thumbnails/" + fileName;

            if (File.Exists(thumbPath))
            {
                thumbPath = "Assets/Scenes/Thumbnails/" + fileName; // need relative path
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(thumbPath, typeof(Texture2D));
                if (tex == null)
                {
                    Debug.LogError("Failed to load thumb for " + assetPath);
                }
                Rect drawRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height - 15);
                EditorGUI.DrawPreviewTexture(drawRect, tex);
            }

        }

    }
}