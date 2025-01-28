//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.Voxel
{
public struct Voxel
{
    public byte ID;

    public bool isSolid
    {
        get
        {
            return ID != 0;
        }
    }
}
}
