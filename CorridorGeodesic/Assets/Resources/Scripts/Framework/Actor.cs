//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Inherits from DataObject, inherited from to give objects the
// functionality to be used in the level editor and loaded from level files
// Notes:
//
//=============================================================================

using UnityEngine;

/// <summary>
/// Inherits from DataObject, inherited from to give objects the functionality to be used in the level editor and loaded from level files
/// </summary>
public class Actor : DataObject
{
    [Tooltip("The name of this object as it appears in an inventory or in the level editor asset browser")]
    public string actorName;
    [Tooltip("The object to be created when loading a map file from this actor")]
    public GameObject AssociatedGameObject;
}
