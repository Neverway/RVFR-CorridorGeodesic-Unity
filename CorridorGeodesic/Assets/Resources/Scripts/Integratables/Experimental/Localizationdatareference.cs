//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using UnityEngine;

[Serializable]
public class Localizationdatareference
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    [Tooltip("Which localization file should be searched for the key")]
    public TextAsset localizationFileID;
    [Tooltip("The name of the field to pull from the file")] 
    public string key;
}
