//===================== (Neverway 2024) Written by Andre Blunt =====================
//
// Purpose:
// Notes:
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorQuantizationFilterRenderer), PostProcessEvent.AfterStack, "Soulex/ColorQuantization", true)]
public sealed class ColorQuantizationFilter : PostProcessEffectSettings
{
    [Range(0, 1)]
    public FloatParameter effectStrength = new FloatParameter { value = 1 };

    public IntParameter maxColors = new IntParameter { value = 256 };
}
public sealed class ColorQuantizationFilterRenderer : PostProcessEffectRenderer<ColorQuantizationFilter>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Soulex/PostProcessing/ColorQuantizationFilter"));

        sheet.properties.SetFloat("_EffectStrength", settings.effectStrength);
        sheet.properties.SetInt("_MaxColors", settings.maxColors);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}