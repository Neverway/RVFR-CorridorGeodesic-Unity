Shader "Soulex/Surface/Standard Toon Oil"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
        [KeywordEnum(TruePBR, StylizedPBR)] _SpecularMode ("Specular Mode", Float) = 0
        _AlphaClip ("Alpha Clip", Range(0, 1)) = 0.5

        _Color ("Color", Color) = (1,1,1,1)

        _RampSmoothness ("Ramp Smoothness", Range(0.1, 1.0)) = 0.1

        _OilAmount ("Oil Amount", Range(0.0, 1.0)) = 0.0
        _OilSmoothing ("Oil Smoothing", Range(0.0, 1.0)) = 0.4

        _OilColor ("Oil Color", Color) = (0.05,0.01,0.07,1)

        [NoScaleOffset] _OilTex ("Oil Placement Texture", 2D) = "black" {}

        [NoScaleOffset] [Normal] _OilNormal ("Oil Normal", 2D) = "bump" {}

        [NoScaleOffset] _MainTex ("Albedo", 2D) = "white" {}

        _Roughness ("Roughness Power", Range(0.0, 1.0)) = 0.5
        [NoScaleOffset] _RoughnessMap ("Roughness Map", 2D) = "white" {}

        _Metallic ("Metallic Power", Range(0.0, 1.0)) = 0.0
        [NoScaleOffset] _MetallicMap ("Metallic Map", 2D) = "white" {}

        _NormalPower ("Normal Power", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset][Normal] _Normal ("Normal Map", 2D) = "bump" {}

        _HeightScale ("Height Scale", Range(0, 0.08)) = 0
        [NoScaleOffset] _HeightMap ("Height Map", 2D) = "black" {}

        [NoScaleOffset] _Occlusion ("Occlusion", 2D) = "white" {}

        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionTex ("Emission", 2D) = "white" {}

        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
        _Offset ("Offset", Vector) = (0, 0, 0, 0)

        [Toggle] _UseSlice ("Use Slice", Float) = 0
        [HideInInspector] _SliceCenterOne ("Slice Center One", Vector) = (0, 0, 0, 0)
        [HideInInspector] _SliceCenterTwo ("Slice Center Two", Vector) = (0, 0, 0, 0)

        [HideInInspector] _SliceNormalOne ("Slice Normal One", Vector) = (0, 0, 0, 0)
        [HideInInspector] _SliceNormalTwo ("Slice Normal Two", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull [_CullMode]

        CGPROGRAM

        #pragma surface surf Ramp fullforwardshadows addshadow
        #pragma multi_compile _SPECULARMODE_TRUEPBR _SPECULARMODE_STYLIZEDPBR

        #pragma target 3.0

        #include "UnityCG.cginc"
        #include "UnityPBSLighting.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"
        #include "UnityStandardBRDF.cginc"
        #include "UnityStandardUtils.cginc"

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Normal;
            float3 viewDir;
            float3 worldPos;
        };

        half _RampSmoothness;
        half _NormalPower;

        half _OilAmount;
        half _OilSmoothing;
        float4 _OilColor;
        sampler2D _OilTex;
        sampler2D _OilNormal;

        float _AlphaClip;

        sampler2D _MainTex;

        half _Roughness;
        sampler2D _RoughnessMap;

        half _Metallic;
        sampler2D _MetallicMap;

        sampler2D _Normal;

        half _HeightScale;
        sampler2D _HeightMap;

        sampler2D _Occlusion;

        float4 _EmissionColor;
        sampler2D _EmissionTex;
        
        float2 _Tiling;
        float2 _Offset;

        fixed4 _Color;

        float _UseSlice;

        float3 _SliceCenterOne;
        float3 _SliceCenterTwo;

        float3 _SliceNormalOne;
        float3 _SliceNormalTwo;

        struct SurfaceOutputToon
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 worldPos;
            fixed3 viewDir;
            half3 Emission;
            half Metallic;
            half Roughness;
            half Occlusion;
            fixed Alpha;
        };
        half3 BRDFToon(half3 diffColor, half3 specColor, half oneMinusReflectivity, half smoothness, 
        float3 normal, float3 viewDir, UnityLight light, UnityIndirect gi)
        {
            float perceptualRoughness = SmoothnessToPerceptualRoughness(smoothness);
            float roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

            roughness = max(roughness, 0.002);

            float3 lightDirection = normalize(light.dir);
            float3 viewDirection = normalize(viewDir);
            float3 halfDirection = Unity_SafeNormalize(viewDirection + lightDirection);
            float3 lightReflectDirection = normalize(reflect(-lightDirection, normal));
            
            half NdotL = smoothstep(0, _RampSmoothness, DotClamped(normal, lightDirection));
            half NdotH = DotClamped(normal, halfDirection);
            half NdotV = abs(dot(normal, viewDirection));
            half LdotH = DotClamped(lightDirection, halfDirection);

            half diffuseTerm = DisneyDiffuse(NdotV, NdotL, LdotH, perceptualRoughness) * NdotL;

            float specularTerm;
            half steps;

            #ifdef _SPECULARMODE_TRUEPBR
                steps = _RampSmoothness * _RampSmoothness * 100;
                float V = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
                float D = GGXTerm(NdotH, roughness);
                specularTerm = V * D * UNITY_PI;
                specularTerm = lerp(round(specularTerm * steps) / steps, specularTerm, 1 - smoothness);
            #elif _SPECULARMODE_STYLIZEDPBR
                steps = _RampSmoothness * _RampSmoothness * 300;
                specularTerm = round(pow(NdotH * NdotL, smoothness * 10) * steps) / steps;
            #endif

            specularTerm = max(0, specularTerm * NdotL);

            half surfaceReduction = 1.0 / (roughness * roughness + 1.0);

            specularTerm *= any(specColor) ? 1.0 : 0.0;

            half grazingTerm = saturate(smoothness + (1 - oneMinusReflectivity));
            half3 color = diffColor * (gi.diffuse + light.color * diffuseTerm)
                    + specularTerm * light.color * FresnelTerm(specColor, LdotH)
                    + surfaceReduction * gi.specular * FresnelLerp(specColor, grazingTerm, NdotV);

            return color;
        }
        inline void LightingRamp_GI(SurfaceOutputToon s, UnityGIInput data, inout UnityGI gi)
        {
            #if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
                gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
            #else
                Unity_GlossyEnvironmentData g = UnityGlossyEnvironmentSetup(1 - s.Roughness, data.worldViewDir, s.Normal, lerp(unity_ColorSpaceDielectricSpec.rgb, s.Albedo, s.Metallic));
                gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal, g);
            #endif
        }
        half4 LightingRamp(SurfaceOutputToon s, float3 viewDir, UnityGI gi)
        {
            float3 normal = normalize(s.Normal);
            float3 viewDirection = normalize(viewDir);
            float3 viewReflectDirection = normalize(reflect(-viewDirection, normal));

            half oneMinusReflectivity;
            half3 specColor;

            s.Albedo = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, specColor, oneMinusReflectivity);

            half3 c = BRDFToon(s.Albedo, specColor, oneMinusReflectivity, 1 - s.Roughness, normal, viewDirection, gi.light, gi.indirect);

            half4 emission = half4(s.Emission + c, s.Alpha);

            return emission;
        }
        half HistogramScan(half input, half position, half smoothing)
        {
            smoothing *= 0.5;
            float low = saturate(position - smoothing);
            float high = saturate(position + smoothing);

            position = position * 2 - 1;

            return smoothstep(low, high, input + position);
        }

        void surf (Input IN, inout SurfaceOutputToon o)
        {
            float2 uv = IN.uv_MainTex * _Tiling + _Offset;
            float2 parallaxOffset = ParallaxOffset (tex2D(_HeightMap, uv).r, _HeightScale, IN.viewDir);

            uv += parallaxOffset;

            fixed4 col = tex2D(_MainTex, uv) * _Color;

            half oilLerp = HistogramScan(tex2D(_OilTex, uv).r, _OilAmount, _OilSmoothing);

            o.viewDir = IN.viewDir;
            o.worldPos = IN.worldPos;

            o.Albedo = lerp(col.rgb, _OilColor, oilLerp);

            o.Normal = lerp(UnpackScaleNormal(tex2D(_Normal, uv), _NormalPower), 
            UnpackScaleNormal(tex2D(_OilNormal, uv), _NormalPower * pow(oilLerp, 2)), oilLerp);

            o.Metallic = lerp(tex2D(_MetallicMap, uv).r * _Metallic, 0, oilLerp);

            o.Roughness = lerp(tex2D(_RoughnessMap, uv).r * _Roughness, 0.1, oilLerp);

            o.Occlusion = lerp(tex2D(_Occlusion, uv).r, 1, oilLerp);

            o.Emission = tex2D(_EmissionTex, uv) * _EmissionColor;

            o.Alpha = col.a;

            float sliceA = dot(_SliceNormalOne, IN.worldPos - _SliceCenterOne);
            float sliceB = dot(_SliceNormalTwo, IN.worldPos - _SliceCenterTwo);

            clip(col.a - _AlphaClip);

            if(_UseSlice == 1)
            {
                clip(sliceA);
                clip(sliceB);
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
