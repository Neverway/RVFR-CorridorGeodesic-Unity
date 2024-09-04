//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose: Clears active rifts from geo-gun
// Notes:
//
//=============================================================================

using UnityEngine;

[RequireComponent(typeof(NEW_LogicProcessor))]
public class Func_GeoGunFizzler: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public NEW_LogicProcessor inputSignal;
	
	
    //=-----------------=
    // Private Variables
    //=-----------------=
	
	
    //=-----------------=
    // Reference Variables
    //=-----------------=
    private NEW_LogicProcessor logicProcessor;
    private Item_Geodesic_Utility_NixieCross nixieCross;
	
	
    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
	    logicProcessor = GetComponent<NEW_LogicProcessor>();
    }

    private void Update()
    {
	    if (!inputSignal) return;
	    logicProcessor.isPowered = inputSignal.isPowered;
	    if (logicProcessor.hasPowerStateChanged)
	    {
		    if (logicProcessor.isPowered)
		    {
			    if (!nixieCross)
			    {
				    nixieCross = FindObjectOfType<Item_Geodesic_Utility_NixieCross>();
			    }

			    if (nixieCross)
			    {
				    nixieCross.UseSpecial();
			    }
		    }
	    }
    }
	
	
    //=-----------------=
    // Internal Functions
    //=-----------------=
	
	
    //=-----------------=
    // External Functions
    //=-----------------=
}
