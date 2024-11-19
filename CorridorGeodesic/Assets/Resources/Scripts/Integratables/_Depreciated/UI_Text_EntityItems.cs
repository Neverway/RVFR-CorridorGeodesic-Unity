//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
[RequireComponent(typeof(Image))]
public class UI_Text_PawnItems : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Pawn targetPawn;
    public bool findPossessedPawn;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    public TMP_Text[] textTargets;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        if (findPossessedPawn)
        {
            targetPawn = FindPossessedPawn();
        }
        if (targetPawn && targetPawn.GetComponent<Pawn_Inventory>())
        {
            for (int i = 0; i < textTargets.Length; i++)
            {
                if (!targetPawn.GetComponent<Pawn_Inventory>().items[i])
                {
                    textTargets[i].text = "---";
                    continue;
                }
                textTargets[i].text = targetPawn.GetComponent<Pawn_Inventory>().items[i].name;
            }
        }
        else
        {
            foreach (var text in textTargets)
            {
                text.text = "---";
            }
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
    /*
    /// <summary>
    /// Updates the sprites on image targets based on equipped items.
    /// </summary>
    private void UpdateImageTargets()
    {
        Sprite[] images = new Sprite[imageTargets.Length];

        for (int i = 0; i < imageTargets.Length; i++)
        {
            images[i] = targetPawn.GetComponent<Pawn_Inventory>().equippedItems[i]?.inventoryIcon;
        }

        if (isActionShelf)
        {
            int currentAction = targetPawn.GetComponent<Pawn_Inventory>().currentAction;

            switch (currentAction)
            {
                case 0:
                    SetSprite(imageTargets[0], images, 2);
                    SetSprite(imageTargets[1], images, 0);
                    SetSprite(imageTargets[2], images, 1);
                    break;
                case 1:
                    SetSprite(imageTargets[0], images, 0);
                    SetSprite(imageTargets[1], images, 1);
                    SetSprite(imageTargets[2], images, 2);
                    break;
                case 2:
                    SetSprite(imageTargets[0], images, 1);
                    SetSprite(imageTargets[1], images, 2);
                    SetSprite(imageTargets[2], images, 0);
                    break;
            }
        }
        else
        {
            for (int i = 0; i < imageTargets.Length; i++)
            {
                imageTargets[i].sprite = images[i];
                imageTargets[i].enabled = images[i] != null;
            }
        }
    }

    /// <summary>
    /// Sets the sprite on the image target based on the equipped item or fallback.
    /// </summary>
    /// <param name="_imageTarget">The Image target to update.</param>
    /// <param name="_images">Array of equipped item sprites.</param>
    /// <param name="_index">Index of the equipped item sprite.</param>
    private void SetSprite(Image _imageTarget, Sprite[] _images, int _index)
    {
        _imageTarget.sprite = _images[_index] != null ? _images[_index] : fallbackImage[_index];
    }


    //=-----------------=
    // External Functions
    //=-----------------=
    public void SetTextHighlighting(int _highlightedIndex)
    {
        foreach (var text in textTargets)
        {
            text.color = new Color(1, 1, 1, 0.5f);
            text.fontStyle &= ~FontStyles.Underline; // Disable text underline
        }

        textTargets[_highlightedIndex].color = new Color(1, 1, 1, 1);
        textTargets[_highlightedIndex].fontStyle |= FontStyles.Underline; // Enable text underline
    }
    
}
*/