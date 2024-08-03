//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Flashes an image by raising it's alpha color channel gradually when an entity has been hurt
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UI_Image_EntityPain : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Pawn targetPawn;
    public bool findPossessedPawn;
    public float fadeSpeed=1;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private bool isInPain; // Used to keep track of if we are currently fading the image in or out (Fadeout: Underground reference?)


    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    }

    private void Update()
    {
        if (findPossessedPawn)
        {
            targetPawn = FindPossessedPawn();
        }
        if (targetPawn)
        {
            targetPawn.OnPawnHurt += () => { OnHurt(); };
        }
        else
        {
            var color = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private Pawn FindPossessedPawn()
    {
        foreach (var entity in FindObjectsByType<Pawn>(FindObjectsSortMode.None))
        {
            if (entity.isPossessed)
            {
                return entity;
            }
        }
        return null;
    }

    private IEnumerator FadeInPain()
    {
        isInPain = true;
        GetComponent<Animator>().Play("PainFlash");
        yield return new WaitForSeconds(1);
        isInPain = false;
    }

    private void OnHurt()
    {
        if (!isInPain)
        {
            StartCoroutine(FadeInPain());
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
