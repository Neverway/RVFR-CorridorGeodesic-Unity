//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_DecalRandomizer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool initialized = false;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private Renderer rend;

    //=-----------------=
    // Mono Functions
    //=-----------------=


    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void InitializeDecal(Color col)
    {
        if (!initialized)
        {
            Material mat = new Material(materials[Random.Range(0, materials.Count)]);
            mat.color = col;

            List<Material> mats = new List<Material>();
            mats.AddRange(rend.sharedMaterials);

            mats[1] = mat;

            rend.sharedMaterials = mats.ToArray();
            initialized = true;
        }
    }
}
