//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: If an object or volume has a sprite renderer as part of it, this
//  will tile the sprite renderer according to the objects scale
// Notes:
//
//=============================================================================

using UnityEngine;

/// <summary>
/// If an object or volume has a sprite renderer as part of it, this will tile the sprite renderer according to the objects scale
/// </summary>
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
