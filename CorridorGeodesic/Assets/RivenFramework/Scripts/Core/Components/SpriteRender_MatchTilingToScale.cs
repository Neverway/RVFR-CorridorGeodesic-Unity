//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Used to make 2D sprites tile dynamically when scaled for things like
//  water volumes
// Notes:
//
//=============================================================================

using UnityEngine;

namespace Neverway.Framework
{
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
}