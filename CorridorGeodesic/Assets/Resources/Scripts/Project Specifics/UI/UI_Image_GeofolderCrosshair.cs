//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose: Flashes an image by raising it's alpha color channel gradually when an entity has been hurt
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Image_GeofolderCrosshair : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    public bool hasInitializedCrosshairBars;


    //=-----------------=
    // Reference Variables
    //=-----------------=
    private ALTItem_Geodesic_Utility_GeoFolder geoFolder;
    [SerializeField] private Image sineA, markerA, sineB, markerB, badDogNoBiscuit;
    [SerializeField] private GameObject crosshair;


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Update()
    {
        // Locate the player and see if they have the geofolder
        if (!geoFolder)
        {
            var pawn = FindPossessedPawn();
            if (pawn)
            {
                geoFolder = pawn.GetComponentInChildren<ALTItem_Geodesic_Utility_GeoFolder>();
            }
            // Hide the crosshair if the player is not holding the geofolder
            crosshair.SetActive(false);
            return;
        }
        crosshair.SetActive(true);
        
        
        // Enable the marker indicators accordingly
        switch (geoFolder.deployedInfinityMarkers.Count)
        {
            case 0:
                markerA.enabled = false;
                markerB.enabled = false;
                hasInitializedCrosshairBars = false;
                sineA.fillAmount = 0;
                sineB.fillAmount = 0;
                break;
            case 1:
                markerA.enabled = true;
                markerB.enabled = false;
                hasInitializedCrosshairBars = false;
                sineA.fillAmount = 0;
                sineB.fillAmount = 0;
                break;
            case 2:
                markerA.enabled = true;
                markerB.enabled = true;
                InitializeCrosshairBars();
                break;
            default:
                print("other");
                break;
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

    private void InitializeCrosshairBars()
    {
        if (hasInitializedCrosshairBars) return;
        StartCoroutine(LerpFunction());
    }

    
    IEnumerator LerpFunction()
    {
        float time = 0;
        float duration = 0.25f;

        while (time < duration)
        {
            var charge = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            sineA.fillAmount = charge;
            sineB.fillAmount = charge;
            hasInitializedCrosshairBars = true;
            yield return null;
        }
    }


    //=-----------------=
    // External Functions
    //=-----------------=
}
