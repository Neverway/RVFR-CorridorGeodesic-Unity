//===================== (Neverway 2024) Written by Andre Blunt =====================
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

    private Color originalColor;
    private float originalFloat;

    //=-----------------=
    // Reference Variables
    //=-----------------=
    public Material material;
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

        switch (propertyType)
        {
            case PropertyType.Color:
                originalColor = instanceMat.GetColor(propertyName);
                break;
            case PropertyType.Float:
                originalFloat = instanceMat.GetFloat(propertyName);
                break;
            default:
                break;
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    public void ChangePropertyToggle(bool state)
    {
        if (state)
            SetProperty();
        else
            ResetMaterial();
    }
    public void ChangeProperty(float duration)
    {
        StopAllCoroutines();
        ResetMaterial();
        StartCoroutine(WaitChangeProperty(useAnimationLengthForDuration ? animClip.length * animationLengthMultiplier : duration));
    }
    public void ChangePropertyManual(object value)
    {
        switch (propertyType)
        {
            case PropertyType.Color:
                instanceMat.SetColor(propertyName, (Color)value);
                break;
            case PropertyType.Float:
                instanceMat.SetFloat(propertyName, (float)value);
                break;
            default:
                break;
        }
    }
    IEnumerator WaitChangeProperty(float duration)
    {
        SetProperty();

        yield return new WaitForSeconds(duration);

        ResetMaterial();
    }
    void SetProperty()
    {
        switch (propertyType)
        {
            case PropertyType.Color:
                instanceMat.SetColor(propertyName, changeToColor);
                break;
            case PropertyType.Float:
                instanceMat.SetFloat(propertyName, changeToFloat);
                break;
            default:
                break;
        }
    }
    void ResetMaterial()
    {
        switch (propertyType)
        {
            case PropertyType.Color:
                instanceMat.SetColor(propertyName, originalColor);
                break;
            case PropertyType.Float:
                instanceMat.SetFloat(propertyName, originalFloat);
                break;
            default:
                break;
        }
    }
	
    //=-----------------=
    // External Functions
    //=-----------------=
}