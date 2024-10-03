//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

[Serializable]
public class DataObject : ScriptableObject
{
    [Tooltip("The ID of this object, used for defining this object in the level editor and map files")]
    public string id;
    public Sprite icon;
}
