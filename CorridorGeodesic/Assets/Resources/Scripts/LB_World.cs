//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes: 
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Neverway.Framework.PawnManagement;

namespace Neverway
{
    public class LB_World : MonoBehaviour
    {
        //=-----------------=
        // Public Variables
        //=-----------------=
        public bool shouldHaveGeoGun = true;
        public bool shouldHaveUpgradedGeoGun = false;
        public bool debugMarkerPlacementAnywhere = false;


        //=-----------------=
        // Private Variables
        //=-----------------=


        //=-----------------=
        // Reference Variables
        //=-----------------=
        private GameInstance gameInstance;
        private Pawn_WeaponInventory weaponInventory;
        private Alt_Item_Geodesic_Utility_GeoGun geoGun;

        //=-----------------=
        // Mono Functions
        //=-----------------=
        private void Start()
        {
            gameInstance = FindObjectOfType<GameInstance>();
            gameInstance.UI_ShowHUD();
            UpdateGeoGunUpgrade();
            StartCoroutine(FindReferences());
        }

        private void Update()
        {
            UpdateGeoGunUpgrade();
        }

        public void UpdateGeoGunUpgrade()
        {
            if (!weaponInventory) return;

            if (shouldHaveGeoGun) weaponInventory.GiveGeoGun();

            if (shouldHaveUpgradedGeoGun) weaponInventory.UpgradeGeoGun();
            
            if (geoGun) geoGun.allowMarkerPlacementAnywhere = debugMarkerPlacementAnywhere;
        }


        //=-----------------=
        // Internal Functions
        //=-----------------=
        private IEnumerator FindReferences()
        {
            if (!weaponInventory)
            {
                weaponInventory = FindObjectOfType<Pawn_WeaponInventory>();
            }
            if (!geoGun)
            {
                geoGun = FindObjectOfType<Alt_Item_Geodesic_Utility_GeoGun>();
            }
            yield return new WaitForEndOfFrame();
            
            if (!weaponInventory || !geoGun)
            {
                StartCoroutine(FindReferences());
            }
        }


        //=-----------------=
        // External Functions
        //=-----------------=
    }
}