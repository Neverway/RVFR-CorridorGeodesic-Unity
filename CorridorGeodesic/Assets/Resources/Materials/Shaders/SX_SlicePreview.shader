Shader "Soulex/Effects/SlicePreview"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Overlay Tex", 2D) = "white" {}
        [NoScaleOffset] _EdgeTex ("Edge Tex", 2D) = "white" {}
        [NoScaleOffset] _DistortionTex ("Distortion Tex", 2D) = "white" {}

        _OverlayScale ("Overlay Scale", Float) = 0.25
        _EdgeScale ("Edge Scale", Float) = 0.25
        _DistortionScale ("Distortion Scale", Float) = 0.1
        
        _EdgeStrength ("Edge Strength", Float) = 1
        _Distortion ("Distortion", Float) = 0.5
        _EmissionStrength ("Emission Strength", Float) = 4
        _ScalingSize ("Scaling Size", Float) = 100

        _ShallowColor ("Shallow Color", Color) = (1, 1, 1, 1)
        _DeepColor ("Deep Color", Color) = (0, 0, 0, 0)

        [HideInInspector] _EffectTime ("Effect Time", Range(0, 1)) = 0
        [HideInInspector] _SphereSize ("Sphere Size", Float) = 0
        [HideInInspector] _BulbsCenter ("Bulbs Center", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "SX_Helpers.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 position : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float4 screenPos : TEXCOORD2;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _EdgeTex;
            sampler2D _DistortionTex;

            half _OverlayScale;
            half _EdgeScale;
            half _DistortionScale;

            half _EdgeStrength;
            half _Distortion;
            half _EmissionStrength;
            half _ScalingSize;

            float _EffectTime;
            half _SphereSize;
            half3 _BulbsCenter;

            half4 _ShallowColor;
            half4 _DeepColor;

            v2f vert (appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.screenPos = ComputeScreenPos(o.position);

                o.normal = UnityObjectToWorldNormal(v.normal);

                UNITY_TRANSFER_FOG(o, o.position);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float lerpTimer = saturate(_EffectTime - 0.5) * 2;
                float bulbDistance = saturate(length(i.worldPos - _BulbsCenter.xyz) - 
                (_SphereSize + pow(_ScalingSize, 1 + _EffectTime) * _EffectTime));
                float effectFactor = 1 - lerp(bulbDistance, 0, lerpTimer);

                half depth = GetDepth(i.screenPos, _EdgeStrength) * effectFactor;

                half oneMinusDepth = 1 - depth;

                UVMod mod;
                mod.uvScale = 0.25;
                mod.uvOffset = 0;

                //Distortion
                TriplanarUV UVs = GetTriplanarUVs(i.worldPos, i.normal, 64, mod, _MainTex, _MainTex_ST);

                //PixelizeTriplanarUV(UVs, 128);

                mod.uvScale = _DistortionScale;
                mod.uvOffset = _Time.x * 0.2;

                half4 distortion = GetTriplanarTexture(_DistortionTex, UVs, mod) * _Distortion;

                //Color Setup
                mod.uvOffset = (distortion - _Distortion * 0.5) * oneMinusDepth;

                //Overlay Color
                mod.uvScale = _OverlayScale;
                float4 overlayCol = GetTriplanarTexture(_MainTex, UVs, mod) * depth;

                //Edge Color
                mod.uvScale = _EdgeScale;
                float4 edgeCol = GetTriplanarTexture(_EdgeTex, UVs, mod) * oneMinusDepth;

                //Combine Effects
                half alpha = overlayCol.a + edgeCol.a;

                half depthMask = pow(oneMinusDepth, 2);

                half4 depthCol = lerp(_ShallowColor, half4(_DeepColor.rgb, lerp(0.025, _DeepColor.a, lerpTimer)), depth);
                
                alpha = max(alpha, depthMask);

                alpha *= depthCol.a;

                alpha = min(alpha, 1 - pow(depthMask, 5));

                half3 color = (overlayCol.rgb + edgeCol.rgb) * depthCol.rgb;

                color = lerp(color, depthCol.rgb, depthMask) * (_EmissionStrength * lerp(2, 1, lerpTimer));

                fixed4 col = fixed4(color, alpha);

                //col = fixed4(bulbDistance, bulbDistance, bulbDistance, 1);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
