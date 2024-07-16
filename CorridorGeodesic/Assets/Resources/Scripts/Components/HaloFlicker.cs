using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HaloFlicker : MonoBehaviour
{

    [SerializeField] private float size = .2f;
    [SerializeField] private float variance = 0.1f;

    private void Update ()
    {
        SerializedObject halo = new SerializedObject (GetComponent ("Halo"));
        halo.FindProperty ("m_Size").floatValue = Random.Range (size - variance, size + variance);
        halo.ApplyModifiedProperties();
    }

}
