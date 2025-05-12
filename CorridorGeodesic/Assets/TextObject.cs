using System;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TextObject : MonoBehaviour
{
    public string text;
    public TextMeshPro physicalText;

    void Update()
    {
        if (physicalText != null)
            physicalText.text = text;
    }

}