//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Sprite_DataExposer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string spriteOverride;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private SpriteRenderer spriteRenderer;
    private ProjectData projectData;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectData = FindObjectOfType<ProjectData>();
    }

    private void Update()
    {
        spriteRenderer.sprite = projectData.GetSpriteFromMemory(spriteOverride);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
