//===================== (Neverway 2024) Written by Liz M. and Connorses =====================
//
// Purpose: Displays health of the possessed pawn.
// Notes: Connorses modified this to use a tween.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class UI_Image_PawnHealth : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Pawn targetPawn;
    public bool findPossessedPawn;


    //=-----------------=
    // Private Variables
    //=-----------------=
    private float previousHealth;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private Image image;


    //=-----------------=
    // Mono Functions
    //=-----------------=

    private void Start ()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (findPossessedPawn && targetPawn == null)
        {
            targetPawn = FindPossessedPawn();
            if (targetPawn != null )
            {
                previousHealth = targetPawn.currentState.health;
                image.fillAmount = (targetPawn.currentState.health / targetPawn.defaultState.health) * 100 * 0.01f;
            }
        }
        if (targetPawn)
        {
            //If health has changed, animate health.
            if (targetPawn.currentState.health != previousHealth)
            {
                image.DOKill();
                image.DOFillAmount((targetPawn.currentState.health / targetPawn.defaultState.health) * 100 * 0.01f, 0.3f);
            }
            previousHealth = targetPawn.currentState.health;
        }
        else
        {
            image.fillAmount = 0;
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