//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_SetShadowTextures : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Texture2D polkaDotTex;
    [SerializeField] private Texture2D stripeTex;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        SetTextures();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    [ContextMenu("Set Shadow Textures")]
    public void SetTextures()
    {
        Shader.SetGlobalTexture("_DarkShadowTex", stripeTex);
        Shader.SetGlobalTexture("_LightShadowTex", polkaDotTex);
    }
}