//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectData : MonoBehaviour
{
    public List<TileMemoryGroup> tiles;
    public List<PropMemoryGroup> props;
    public List<ItemMemoryGroup> items;
    public List<CharacterMemoryGroup> characters;
}

// ---------------------------------------------------------
[Serializable]
public class Spacer
{
    public int index;
    public int spacerCount;
}

[Serializable]
public class MemoryGroup
{
    [Tooltip("The name of the group")]
    public string name;
    [Tooltip("Used to add empty space in the level editor to make the asset browser UI cleaner")]
    public List<Spacer> spacers;
}
// ---------------------------------------------------------

[Serializable]
public class TileMemoryGroup : MemoryGroup
{
    public List<Tile> tiles = new List<Tile>();
}

[Serializable]
public class PropMemoryGroup : MemoryGroup
{
    public List<Prop> props = new List<Prop>();
}

[Serializable]
public class ItemMemoryGroup : MemoryGroup
{
    public List<Item> items = new List<Item>();
}

[Serializable]
public class CharacterMemoryGroup : MemoryGroup
{
    public List<CharacterData> characters = new List<CharacterData>();
}





[Serializable]
public class LevelData
{
    public List<SpotData> tiles = new List<SpotData>();
    public List<SpotData> assets = new List<SpotData>();
}

[Serializable]
public class SpotData
{
    public int layer;
    public string layerID; // WIP value to track actual layer name, instead of layer hierarchy position, to hopefully make level file corruption less common when changing what's stored in level files
    public string id;
    public int uniqueId;
    public Vector3Int position;
    public Vector3 unsnappedPosition;
    public List<VariableData> assetData = new List<VariableData>();
}