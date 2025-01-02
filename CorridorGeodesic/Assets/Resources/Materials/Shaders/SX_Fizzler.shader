Shader "Soulex/Effects/Fizzler"
{
    Properties
    {
        [Header(Textures)]
        [NoScaleOffset] _MainTex ("Grid Texture", 2D) = "white" {}
        [NoScaleOffset] _Noise ("Noise Texture", 2D) = "white" {}

        [Header(Color)]
        _ShallowColor ("Shallow Color", Color) = (0, 0, 0, 0)
        _DeepColor ("Deep Color", Color) = (1, 1, 1, 1)

        [Header(Settings)]
        _TriplanarSharpness ("Triplanar Sharpness", Range(1.0, 64.0)) = 1.0
        _DistortStrength ("Distort Strength", Float) = 1.0
        _DepthStrength ("Depth Strength", Float) = 0.2
        _ScrollDirection ("Scroll Direction", Vector) = (1, 1, 0, 0)
        
        [Header(Transparency)]
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
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
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD2;
                float3 normal : NORMAL;
                float4 screenPos : TEXCOORD3;
                float4 position : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Noise;
            
            float4 _ShallowColor;
            float4 _DeepColor;

            float _TriplanarSharpness;
            float _DistortStrength;
            float _DepthStrength;
            float2 _ScrollDirection;

            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.normal = UnityObjectToWorldNormal(v.normal);

                o.screenPos = ComputeScreenPos(o.position);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.position);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UVMod mod;
                mod.uvScale = 0.25;
                mod.uvOffset = 0;

                TriplanarUV UVs = GetTriplanarUVs(i.worldPos, i.normal, _TriplanarSharpness, mod, _MainTex, _MainTex_ST);

                //PixelizeTriplanarUV(UVs, 128);

                mod.uvScale = 0.125;
                mod.uvOffset = _ScrollDirection * _Time.x * 0.25;

                float distortA = GetTriplanarTexture(_Noise, UVs, mod).r;

                mod.uvScale = 0.0625;
                float distortB = GetTriplanarTexture(_Noise, UVs, mod).g;

                float distort = saturate((distortA + distortB) * 0.5);

                mod.uvScale = 1;
                mod.uvOffset = distort * _DistortStrength - _DistortStrength * 0.5;
                float4 col = GetTriplanarTexture(_MainTex, UVs, mod);

                half4 fog = GetDepth(i.screenPos, _DepthStrength);

                half4 fogColor = lerp(_ShallowColor, _DeepColor, fog);

                col.rgb *= lerp(0.1, 2, col.a);

                col.rgb += distort;

                col.a = max(col.a, fogColor.a) * _Alpha;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4((col * fogColor).rgb, col.a);
            }
            ENDCG
        }
    }
}
