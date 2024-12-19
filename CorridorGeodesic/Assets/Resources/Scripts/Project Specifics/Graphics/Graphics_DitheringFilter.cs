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
[PostProcess(typeof(DitheringFilterRenderer), PostProcessEvent.AfterStack, "Soulex/Dithering", true)]
public sealed class DitheringFilter : PostProcessEffectSettings
{
    [Range(0, 1)]
    public FloatParameter effectStrength = new FloatParameter { value = 1 };

    public FloatParameter spread = new FloatParameter { value = 1 };
}
public sealed class DitheringFilterRenderer : PostProcessEffectRenderer<DitheringFilter>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Soulex/PostProcessing/DitheringFilter"));

        sheet.properties.SetFloat("_EffectStrength", settings.effectStrength);
        sheet.properties.SetFloat("_Spread", settings.spread);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}