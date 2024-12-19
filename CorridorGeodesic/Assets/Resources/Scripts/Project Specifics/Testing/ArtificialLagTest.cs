using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialLagTest : MonoBehaviour
{
    [DebugReadOnly] public int findObjectPasses = 0;
    [HideInInspector] public string someValue;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKeyDown(KeyCode.L))
                findObjectPasses += 25;

            if (Input.GetKeyDown(KeyCode.K))
                findObjectPasses -= 25;

            if (findObjectPasses < 0)
                findObjectPasses = 0;
        }
        for (int i = 0; i < findObjectPasses; i++)
        {
            ArtificialLagTest[] lags = FindObjectsOfType<ArtificialLagTest>();
            foreach(ArtificialLagTest lag in lags)
            {
                someValue = Guid.NewGuid().ToString();
            }
        }
    }
}
