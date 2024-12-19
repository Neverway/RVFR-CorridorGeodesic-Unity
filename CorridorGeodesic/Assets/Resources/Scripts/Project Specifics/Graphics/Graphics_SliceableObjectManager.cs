//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Graphics_SliceableObjectManager : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
	public static Graphics_SliceableObjectManager Instance;

    //=-----------------=
    // Private Variables
    //=-----------------=
    private List<Graphics_SliceableObject> sliceableObjects = new List<Graphics_SliceableObject>();

    //=-----------------=
    // Reference Variables
    //=-----------------=


    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Awake()
    {
        Instance = this;
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=


    //=-----------------=
    // External Functions
    //=-----------------=
    public void AddToList(Graphics_SliceableObject obj)
    {
        sliceableObjects.Add(obj);
    }
    public void ClearList()
    {
        sliceableObjects.Clear();
    }
    public void SliceObjects()
    {
        sliceableObjects.ForEach(s => s.StartSlicing());
    }
}