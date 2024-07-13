using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_Volume3D_Kill : MonoBehaviour
{
    protected void OnTriggerEnter(Collider _other)
    {
        if (_other.GetComponent<Pawn>())
        {
            Destroy(_other.gameObject);
        }
    }
}
