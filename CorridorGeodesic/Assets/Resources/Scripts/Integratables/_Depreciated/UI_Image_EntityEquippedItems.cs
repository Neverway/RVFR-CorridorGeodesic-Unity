//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
/*
[RequireComponent(typeof(Image))]
public class UI_Image_PawnEquippedItems : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public Pawn targetPawn;
    public bool findPossessedPawn;
    public bool isActionShelf;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Image[] imageTargets;
    [SerializeField] private Sprite[] fallbackImage;
    [SerializeField] private Sprite[] images;


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
            UpdateImageTargets();
        }
        else
        {
            foreach (var image in imageTargets)
            {
                image.enabled = false;
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
    
    /// <summary>
    /// Updates the sprites on image targets based on equipped items.
    /// </summary>
    private void UpdateImageTargets()
    {
        Sprite[] images = new Sprite[imageTargets.Length];

        for (int i = 0; i < imageTargets.Length; i++)
        {
            images[i] = targetPawn.GetComponent<Pawn_Inventory>().equippedItems[i]?.icon;
        }

        if (isActionShelf)
        {
            int currentAction = targetPawn.GetComponent<Pawn_Inventory>().currentAction;

            if (targetPawn.isNearInteractable)
            {
                foreach (var image in imageTargets)
                {
                    image.color = new Color(0.1f,0.1f,0.1f, 0.5f);
                }
                imageTargets[1].color = new Color(0.25f,0.25f,0.25f, 0.5f);
            }
            else
            {
                foreach (var image in imageTargets)
                {
                    image.color = new Color(0.25f,0.25f,0.25f, 1f);
                }
                imageTargets[1].color = Color.white;
            }
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
}
*/