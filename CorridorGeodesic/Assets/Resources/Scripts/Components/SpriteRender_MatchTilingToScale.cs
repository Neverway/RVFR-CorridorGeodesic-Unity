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
public class SpriteRender_MatchTilingToScale : MonoBehaviour
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
    private SpriteRenderer spriteRender;
    [SerializeField] private GameObject targetObject;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRender.size = targetObject.transform.localScale;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
}
