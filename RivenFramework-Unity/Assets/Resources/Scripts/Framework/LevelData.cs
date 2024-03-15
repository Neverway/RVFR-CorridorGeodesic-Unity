//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

[Serializable]
public class tileMemoryGroup
{
    public string name;
    public List<Tile> tiles = new List<Tile>();
    [Tooltip("Used to add empty space in the level editor to make the tile selection UI cleaner")]
    public List<Spacer> spacers;
}

[Serializable]
public class AssetMemoryGroup
{
    public string name;
    public List<GameObject> assets = new List<GameObject>();
    [Tooltip("Used to add empty space in the level editor to make the tile selection UI cleaner")]
    public List<Spacer> spacers;
}

[Serializable]
public class Spacer
{
    public int index;
    public int spacerCount;
}

