Shader "Soulex/Surface/Standard Toon2"
{
    Properties
    {
        [Header(Material Settings)][Space]
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
        [KeywordEnum(TruePBR, StylizedPBR)] _SpecularMode ("Specular Mode", Float) = 0
        _AlphaClip ("Alpha Clip", Range(0, 1)) = 0.5

        _Color ("Color", Color) = (1,1,1,1)

        [Header(Main Texture Properties)][Space]
        _RampSmoothness ("Ramp Smoothness", Range(0.1, 1.0)) = 0.1

        [NoScaleOffset] _MainTex ("Albedo", 2D) = "white" {}

        _Glossiness ("Roughness Power", Range(0.0, 1.0)) = 0.5
        [NoScaleOffset] _SpecGlossMap ("Roughness Map", 2D) = "white" {}

        _Metallic ("Metallic Power", Range(0.0, 1.0)) = 0.0
        [NoScaleOffset] _MetallicGlossMap ("Metallic Map", 2D) = "white" {}

        _BumpScale ("Normal Power", Range(0.0, 1.0)) = 1.0
        [NoScaleOffset][Normal] _BumpMap ("Normal Map", 2D) = "bump" {}

        _Parallax ("Height Scale", Range(0, 0.08)) = 0
        [NoScaleOffset] _ParallaxMap ("Height Map", 2D) = "black" {}

        [NoScaleOffset] _OcclusionMap ("Occlusion", 2D) = "white" {}
        
        _Tiling ("Tiling", Vector) = (1, 1, 0, 0)
        _Offset ("Offset", Vector) = (0, 0, 0, 0)

        [Header(Emission Properties)][Space]
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset] _EmissionMap ("Emission", 2D) = "white" {}

        [Header(Detail Layer)][Space]
        _DetailAlbedoMap ("Detail Texture", 2D) = "black" {}
        _DetailProminence ("Detail Prominence", Range(0, 1)) = 0.2
        _DetailColor ("Detail Color", Color) = (0, 0, 0, 0)

        [Header(Debug Parameters)][Space]
        [Toggle] _UseSlice ("Use Slice", Float) = 0
        [Toggle] _ColorOnly ("Color Only", Float) = 0
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
        #include "UnityGBuffer.cginc"
        #include "SX_Helpers.cginc"

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DetailAlbedoMap;
            float2 uv_Normal;
            float3 viewDir;
            float3 worldPos;
            float4 screenPos;
        };

        half _RampSmoothness;

        float _AlphaClip;

        sampler2D _MainTex;

        half _Glossiness;
        sampler2D _SpecGlossMap;

        half _Metallic;
        sampler2D _MetallicGlossMap;

        half _BumpScale;
        sampler2D _BumpMap;

        half _Parallax;
        sampler2D _ParallaxMap;

        sampler2D _OcclusionMap;

        half4 _EmissionColor;
        sampler2D _EmissionMap;

        sampler2D _DetailAlbedoMap;
        half _DetailProminence;
        half3 _DetailColor;
        
        float2 _Tiling;
        float2 _Offset;

        fixed4 _Color;

        float _UseSlice;
        float _ColorOnly;

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
            fixed2 screenUv;
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
            
            half preNdotL = DotClamped(normal, lightDirection);
            half NdotL = smoothstep(0, _RampSmoothness, preNdotL);
            half NdotH = DotClamped(normal, halfDirection);
            half NdotV = abs(dot(normal, viewDirection));
            half LdotH = DotClamped(lightDirection, halfDirection);

            //half shadowMix = lerp(1 - darkShadow * 0.95, 1 - lightShadow * -0.2, NdotL);
            //shadowMix = lerp(shadowMix, 1, saturate(preNdotL + 0.5));
            //NdotL *= shadowMix;

            half diffuseTerm = DisneyDiffuse(NdotV, NdotL, LdotH, perceptualRoughness) * NdotL;

            float specularTerm;
            half steps;

            #ifdef _SPECULARMODE_TRUEPBR
                steps = _RampSmoothness * _RampSmoothness * 100 * 64;
    
                //steps = 64;
                
                //float rNdotL = round(NdotL * steps) / steps;
                //float rNdotV = round(NdotV * steps) / steps;
    
                float rNdotH = round(NdotH * steps) / steps;
                
                float V = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
                float D = GGXTerm(rNdotH, roughness);
                specularTerm = V * D * UNITY_PI;
                //specularTerm = lerp(round(specularTerm * steps) / steps, specularTerm, 1 - smoothness);
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
            half oneMinusReflectivity;
            half3 specColor;

            s.Albedo = DiffuseAndSpecularFromMetallic(s.Albedo, s.Metallic, specColor, oneMinusReflectivity);

            //half darkShadow = tex2D(_DarkShadowTex, s.screenUv).r;
            //half lightShadow = tex2D(_LightShadowTex, s.screenUv).r;

            half3 c = BRDFToon(s.Albedo, specColor, oneMinusReflectivity, 1 - s.Roughness, normal, viewDirection, gi.light, gi.indirect);

            half4 emission = half4(s.Emission + c, s.Alpha);

            return emission;
        }

        void surf (Input IN, inout SurfaceOutputToon o)
        {
            float2 uv = IN.uv_MainTex * _Tiling + _Offset;
            float2 parallaxOffset = ParallaxOffset (tex2D(_ParallaxMap, uv).r, _Parallax, IN.viewDir);

            uv += parallaxOffset;

            fixed4 col = tex2D(_MainTex, uv) * _Color;
            half4 detailCol = tex2D(_DetailAlbedoMap, IN.uv_DetailAlbedoMap);
            half detailMask = luminance(detailCol.rgb) * detailCol.a * _DetailProminence;

            o.viewDir = IN.viewDir;
            o.worldPos = IN.worldPos;
            //o.screenUv = IN.screenPos.xy / IN.screenPos.w;
            //o.screenUv.x *= (_ScreenParams.x / _ScreenParams.y);

            //o.screenUv *= 8;

            o.Albedo = lerp(col.rgb, detailCol.rgb * _DetailColor, detailMask);

            if (_ColorOnly == 0)
            {
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, uv), _BumpScale);

                o.Metallic = tex2D(_MetallicGlossMap, uv).r * _Metallic;

                o.Roughness = tex2D(_SpecGlossMap, uv).r * _Glossiness;

                o.Occlusion = tex2D(_OcclusionMap, uv).r;

                o.Emission = tex2D(_EmissionMap, uv) * _EmissionColor;

                o.Alpha = col.a;
            }
            else
            {
                o.Roughness = tex2D(_SpecGlossMap, uv).r * _Glossiness;
            }

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
