//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2D_Pickup : Volume2D
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public string itemID;
    public Item item;
    public bool useItemIDOverride;


    //=-----------------=
    // Private Variables
    //=-----------------=


    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private SpriteRenderer spriteRenderer;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
    
    }

    private void Update()
    {
        if (useItemIDOverride) item = FindObjectOfType<LevelManager>().GetItemFromMemory(itemID);
        if (spriteRenderer && item) spriteRenderer.sprite = item.icon;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void activate()
    {
        if (GetPlayerInTrigger().GetComponent<Pawn_Inventory>().AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}
