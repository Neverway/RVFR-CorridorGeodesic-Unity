Shader "Soulex/Surface/Standard No Fog"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
        _AlphaClip ("Alpha Clip", Range(0, 1)) = 0.5

        _Color ("Color", Color) = (1,1,1,1)

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
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Cull [_CullMode]

        CGPROGRAM

        #pragma surface surf Standard addshadow nofog
        #include "UnityStandardUtils.cginc"

        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        float _AlphaClip;
        half _NormalPower;

        sampler2D _MainTex;

        float _Roughness;
        sampler2D _RoughnessMap;

        float _Metallic;
        sampler2D _MetallicMap;

        sampler2D _Normal;

        float _HeightScale;
        sampler2D _HeightMap;

        sampler2D _Occlusion;

        float4 _EmissionColor;
        sampler2D _EmissionTex;
        
        float2 _Tiling;
        float2 _Offset;

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex * _Tiling + _Offset;
            float2 parallaxOffset = ParallaxOffset (tex2D(_HeightMap, uv).r, _HeightScale, IN.viewDir);

            uv += parallaxOffset;

            fixed4 col = tex2D(_MainTex, uv) * _Color;

            o.Albedo = col.rgb;

            o.Normal = UnpackScaleNormal(tex2D(_Normal, uv), _NormalPower);

            o.Metallic = tex2D(_MetallicMap, uv).r * _Metallic;

            o.Smoothness = 1 - tex2D(_RoughnessMap, uv).r * _Roughness;

            o.Occlusion = tex2D(_Occlusion, uv).r;

            o.Emission = tex2D(_EmissionTex, uv) * _EmissionColor;

            o.Alpha = col.a;
            clip(col.a -_AlphaClip);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
