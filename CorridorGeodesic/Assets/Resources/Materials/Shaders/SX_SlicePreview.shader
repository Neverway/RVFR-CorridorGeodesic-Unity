Shader "Soulex/SX_SlicePreview"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Overlay Tex", 2D) = "white" {}
        [NoScaleOffset] _EdgeTex ("Edge Tex", 2D) = "white" {}
        [NoScaleOffset] _DistortionTex ("Distortion Tex", 2D) = "white" {}

        _EdgeStrength ("Edge Strength", Float) = 1
        _Distortion ("Distortion", Float) = 0.5
        _EmissionStrength ("Emission Strength", Float) = 4
        _ScalingSize ("Scaling Size", Float) = 100

        _ShallowColor ("Shallow Color", Color) = (1, 1, 1, 1)
        _DeepColor ("Deep Color", Color) = (0, 0, 0, 0)

        _EffectTime ("Effect Time", Range(0, 1)) = 0
        _SphereSize ("Sphere Size", Float) = 0
        _BulbsCenter ("Bulbs Center", Vector) = (0, 0, 0, 0)
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

                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o, o.position);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float lerpTimer = saturate(_EffectTime - 0.5) * 2;
                float bulbDistance = saturate(length(i.worldPos - _BulbsCenter.xyz) - (_SphereSize + pow(_ScalingSize, 1 + _EffectTime) * _EffectTime));
                float effectFactor = 1 - lerp(bulbDistance, 0, lerpTimer);

                half depth = GetDepth(i.screenPos, _EdgeStrength) * effectFactor;

                half oneMinusDepth = 1 - depth;

                UVMod distortionMod;
                distortionMod.uvScale = 0.1;
                distortionMod.uvOffset = _Time.x * 0.2;

                TriplanarUV distortionUVs = GetTriplanarUVs(i.worldPos, i.normal, 1, distortionMod, _MainTex, _MainTex_ST);

                half4 distortion = GetTriplanarTexture(_DistortionTex, distortionUVs, distortionMod) * _Distortion;

                float4 overlayCol = SampleTriplanarTexture(_MainTex, _MainTex, _MainTex_ST, i.worldPos, i.normal, 
                0.25, (distortion - _Distortion * 0.5) * oneMinusDepth, 64) * depth;

                float4 edgeCol = SampleTriplanarTexture(_EdgeTex, _MainTex, _MainTex_ST, i.worldPos, i.normal,
                0.25, (distortion - _Distortion * 0.5) * oneMinusDepth, 64) * oneMinusDepth;

                half alpha = overlayCol.a + edgeCol.a;

                half depthMask = pow(oneMinusDepth, 10);

                half4 depthCol = lerp(_ShallowColor, half4(_DeepColor.rgb, lerp(0.2, _DeepColor.a, lerpTimer)), depth);
                
                alpha = max(alpha, depthMask);

                alpha *= depthCol.a;

                alpha = min(alpha, 1 - depthMask);

                half3 color = (overlayCol.rgb + edgeCol.rgb) * depthCol.rgb * _EmissionStrength;

                color = lerp(color, depthCol.rgb, depthMask);

                fixed4 col = fixed4(color, alpha);

                //col = fixed4(bulbDistance, bulbDistance, bulbDistance, 1);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
