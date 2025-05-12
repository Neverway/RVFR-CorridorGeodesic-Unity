//===================== (Neverway 2024) Written by Liz M. =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int something = 5;
    public GameObject someObject;

    private NewBehaviourScript RAHHH;

    private void Start()
    {
        RAHHH = someObject.GetComponent<NewBehaviourScript>();
    }

    private void Update()
    {
        Time.timeScale = 2f;
    }
}