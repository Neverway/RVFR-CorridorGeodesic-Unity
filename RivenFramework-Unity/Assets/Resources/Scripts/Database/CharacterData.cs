//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

[CreateAssetMenu(fileName="CharacterData", menuName="Neverway/ScriptableObjects")]
public class CharacterData : DataObject
{
    public string objectName;
    public float health;
    public float movementSpeed;
    public string team;
}
