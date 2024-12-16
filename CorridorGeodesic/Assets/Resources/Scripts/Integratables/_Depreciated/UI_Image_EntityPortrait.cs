//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Neverway.Framework.PawnManagement
{
    [RequireComponent(typeof(Image))]
    public class UI_Image_PawnPortrait : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public Pawn targetPawn;
        public bool findPossessedPawn;
        public Sprite fallbackImage;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=


        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Update()
        {
            if (findPossessedPawn)
            {
                targetPawn = FindPossessedPawn();
            }
            if (targetPawn && targetPawn.GetComponent<SpriteRenderer>())
            {
                GetComponent<Image>().sprite = targetPawn.GetComponent<SpriteRenderer>().sprite;
            }
            else if (fallbackImage)
            {
                GetComponent<Image>().sprite = fallbackImage;
            }
        }

        //=-----------------=
        // Internal Functions
        //=-----------------=
        private Pawn FindPossessedPawn()
        {
            foreach (var entity in FindObjectsByType<Pawn>(FindObjectsSortMode.None))
            {
                if (entity.isPossessed) return entity;
            }
            return null;
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}