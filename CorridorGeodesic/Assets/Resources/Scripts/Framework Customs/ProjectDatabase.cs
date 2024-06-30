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

// ---------------------------------------------------------
public class ProjectData : MonoBehaviour
{
    public List<TileMemoryGroup> tiles;
    public List<PropMemoryGroup> props;
    public List<ItemMemoryGroup> items;
    public List<CharacterMemoryGroup> characters;
    public List<Sprite> sprites;
    public Tile missingTileFallback;
    public GameObject missingObjectFallback;
    public Sprite missingSpriteFallback;
    public Sprite missingItemIconFallback;
    public Sprite missingCharacterIconFallback;

    public Tile GetTileFromMemory(string _tileId)
    {
        foreach (var tileMemoryGroup in tiles)
        {
            foreach (var tile in tileMemoryGroup.tiles)
            {
                if (tile.name == _tileId) return tile;
            }
        }

        return null;
    }

    public Actor GetActorFromMemory(string _id)
    {
        foreach (var actorMemoryGroup in props)
        {
            foreach (var asset in actorMemoryGroup.props)
            {
                if (asset.id == _id) return asset;
            }
        }

        foreach (var actorMemoryGroup in items)
        {
            foreach (var asset in actorMemoryGroup.items)
            {
                if (asset.id == _id) return asset;
            }
        }

        foreach (var actorMemoryGroup in characters)
        {
            foreach (var asset in actorMemoryGroup.characters)
            {
                if (asset.id == _id) return asset;
            }
        }

        return null;
    }

    public Prop GetPropFromMemory(string _propId)
    {
        foreach (var actorMemoryGroup in props)
        {
            foreach (var asset in actorMemoryGroup.props)
            {
                if (asset.id == _propId) return asset;
            }
        }

        return null;
    }

    public Item GetItemFromMemory(string _itemId)
    {
        foreach (var actorMemoryGroup in items)
        {
            foreach (var asset in actorMemoryGroup.items)
            {
                if (asset.id == _itemId) return asset;
            }
        }

        return null;
    }

    public CharacterData GetCharacterFromMemory(string _characterId)
    {
        foreach (var actorMemoryGroup in characters)
        {
            foreach (var asset in actorMemoryGroup.characters)
            {
                if (asset.id == _characterId) return asset;
            }
        }

        return null;
    }

    public Sprite GetSpriteFromMemory(string _spriteId)
    {
        foreach (var sprite in sprites)
        {
            if (sprite.name == _spriteId) return sprite;
        }

        return null;
    }

    public String GetActorType(string _id)
    {
        if (GetPropFromMemory(_id)) return "prop";
        if (GetItemFromMemory(_id)) return "item";
        if (GetCharacterFromMemory(_id)) return "character";

        return null;
    }
}
// ---------------------------------------------------------


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
// ---------------------------------------------------------


// ---------------------------------------------------------
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
    public string assetId;
    public int uniqueInstanceId;
    public Vector3Int tilePosition;
    public Vector3 worldPosition;
    public List<VariableData> assetData = new List<VariableData>();
}