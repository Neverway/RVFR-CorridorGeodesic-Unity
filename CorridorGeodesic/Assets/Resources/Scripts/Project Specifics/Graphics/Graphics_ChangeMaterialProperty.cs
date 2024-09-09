//===================== (Neverway 2024) Written by _____ =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics_ChangeMaterialProperty: MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=


    //=-----------------=
    // Private Variables
    //=-----------------=
    [SerializeField] private PropertyType propertyType;
    [SerializeField] private string propertyName;
    [SerializeField] private Color changeToColor;
    [SerializeField] private float changeToFloat;

    private Material instanceMat;

    private Color prevColor;
    private float prevFloat;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    [SerializeField] private Material material;
    [SerializeField] private Renderer meshRenderer;

    [Header("Animation")]
    [SerializeField] private bool useAnimationLengthForDuration = false;
    [SerializeField] private AnimationClip animClip;
    [SerializeField] private float animationLengthMultiplier = 1;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    private void Start()
    {
        List<Material> materials = new List<Material>();
        materials.AddRange(meshRenderer.sharedMaterials);

        int materialIndex = materials.IndexOf(material);

        instanceMat = new Material(meshRenderer.sharedMaterials[materialIndex]);

        materials[materialIndex] = instanceMat;

        meshRenderer.sharedMaterials = materials.ToArray();
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void ChangeProperty(float duration)
    {
        StopAllCoroutines();
        ResetMaterial();
        StartCoroutine(WaitChangeProperty(useAnimationLengthForDuration ? animClip.length * animationLengthMultiplier : duration));
    }
    IEnumerator WaitChangeProperty(float duration)
    {
        switch (propertyType)
        {
            case PropertyType.Color:
                prevColor = instanceMat.GetColor(propertyName);
                instanceMat.SetColor(propertyName, changeToColor);
                print($"changed? {instanceMat} {changeToColor}");
                break;
            case PropertyType.Float:
                prevFloat = instanceMat.GetFloat(propertyName);
                instanceMat.SetFloat(propertyName, changeToFloat);
                break;
            default:
                yield break;
        }
        yield return new WaitForSeconds(duration);

        ResetMaterial();
    }
    void ResetMaterial()
    {
        switch (propertyType)
        {
            case PropertyType.Color:
                instanceMat.SetColor(propertyName, prevColor);
                break;
            case PropertyType.Float:
                instanceMat.SetFloat(propertyName, prevFloat);
                break;
            default:
                break;
        }
    }
	
    //=-----------------=
    // External Functions
    //=-----------------=
}