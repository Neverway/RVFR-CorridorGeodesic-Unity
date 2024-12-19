Shader "Soulex/PostProcessing/DitheringFilter"
{
    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

        float _EffectStrength;
        float _Spread;

        static const int bayer2[2 * 2] = {
            0, 2,
            3, 1
        };
        static const int bayer4[4 * 4] = {
            0, 8, 2, 10,
            12, 4, 14, 6, 
            3, 11, 1, 9,
            15, 7, 13, 5
        };

        float GetBayer4(int x, int y)
        {
            return float(bayer4[(x % 4) + (y % 4) * 4] / 16.0) - 0.5;
        }
        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

            int x = i.texcoord.x * _ScreenParams.x;
            int y = i.texcoord.y * _ScreenParams.y;

            float bayerValue = GetBayer4(x, y);

            return lerp(col, col + _Spread * bayerValue, _EffectStrength);
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