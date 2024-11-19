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
[PostProcess(typeof(ComicFilterRenderer), PostProcessEvent.AfterStack, "Soulex/Comic", true)]
public sealed class ComicFilter : PostProcessEffectSettings
{
    public TextureParameter darkShadowTexture = new TextureParameter();
    public TextureParameter lightShadowTexture = new TextureParameter();

    [Range(0, 1)]
    public FloatParameter effectStrength = new FloatParameter { value = 0.1f };

    [Range(0, 1)]
    public FloatParameter darkPoint = new FloatParameter { value = 0.2f };

    [Range(0, 1)]
    public FloatParameter lightPoint = new FloatParameter { value = 0.35f };
}
public sealed class ComicFilterRenderer : PostProcessEffectRenderer<ComicFilter>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Soulex/PostProcessing/ComicFilter"));

        sheet.properties.SetTexture("_DarkShadowTex", settings.darkShadowTexture);
        sheet.properties.SetTexture("_LightShadowTex", settings.lightShadowTexture);
        sheet.properties.SetFloat("_EffectStrength", settings.effectStrength);
        sheet.properties.SetFloat("_DarkPoint", settings.darkPoint);
        sheet.properties.SetFloat("_LightPoint", settings.lightPoint);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}