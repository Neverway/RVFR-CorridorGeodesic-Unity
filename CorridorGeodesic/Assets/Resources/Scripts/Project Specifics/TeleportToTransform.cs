using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToTransform : MonoBehaviour
{
    public Transform toTeleportTo;
    void Update()
    {
        transform.position = toTeleportTo.position;
    }
}
