Shader "Soulex/PostProcessing/ComicFilter"
{
    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_DarkShadowTex, sampler_DarkShadowTex);
        TEXTURE2D_SAMPLER2D(_LightShadowTex, sampler_LightShadowTex);

        float _EffectStrength;
        float _DarkPoint;
        float _LightPoint;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

            half luminance = dot(col.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            luminance = smoothstep(0, 1, luminance);

            float2 uv = i.texcoord;
            uv.x *= (_ScreenParams.x / _ScreenParams.y);
            uv *= 16;

            half darkShadow = SAMPLE_TEXTURE2D(_DarkShadowTex, sampler_DarkShadowTex, uv).r;
            half lightShadow = SAMPLE_TEXTURE2D(_LightShadowTex, sampler_LightShadowTex, uv).r;

            half lumLerp = smoothstep(_DarkPoint - 0.05, _DarkPoint + 0.1, luminance);
            half shadow = lerp(darkShadow, lightShadow, lumLerp);
            shadow = 1 - (1 - shadow) * _EffectStrength;
            shadow = lerp(shadow, 1, smoothstep(0, _LightPoint, luminance));

            return col * shadow;
        }
    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
