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
    private LevelManager levelManager;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        spriteRenderer.sprite = levelManager.GetSpriteFromMemory(spriteOverride);
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
