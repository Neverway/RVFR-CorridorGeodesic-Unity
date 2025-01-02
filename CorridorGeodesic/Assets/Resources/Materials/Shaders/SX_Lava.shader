Shader "Soulex/Effects/Lava"
{
    Properties
    {
        [Header(Colors)]
        _HotColor ("Hot Color", Color) = (1, 1, 1, 1)
        _ColdColor ("Cold Color", Color) = (0, 0, 0, 1)
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)

        [Header(Textures)]
        [NoScaleOffset] _MainTex ("Lava Cracks", 2D) = "white" {}
        [NoScaleOffset] _DistortTex ("Distortion Texture", 2D) = "white" {}

        [Header(Color Parameters)]
        _Offset ("Color Offset", Float) = 1.0
        _StrengthUnder ("Under Lava Strength", Range(0.0, 5.0)) = 1.0
        _StrengthOver ("Over Lava Strength", Range(0.0, 5.0)) = 1.0

        [Header(Edge Parameters)]
        _Edge ("Edge", Range(0.0, 1.0)) = 0.1
        _EdgeSmooth ("Edge Smoothness", Range(0.0, 1.0)) = 0.8
        _Cutoff("Cutoff Top", Range(0,1)) = 0.9
        _TopBlur("Top Blur", Range(0,1)) = 0.1

        [Header(UV Scales)]
        _Scale ("UV Scale", Vector) = (1, 1, 0, 0)
        _DistScale ("Distortion UV Scale", Vector) = (1, 1, 0, 0)
        _ScrollSpeed ("Scroll Speed", Vector) = (0.1, 0.1, 0, 0)
        _Distortion ("Distortion", Float) = 1.0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
            "Queue"="Transparent"
        }
        LOD 100
        //Blend SrcAlpha OneMinusSrcAlpha

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
                //float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                //float2 uv : TEXCOORD0;
                float4 scrPos : TEXCOORD2;
                float3 worldPos : TEXCOORD4;

                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _HotColor;
            float4 _ColdColor;
            float4 _EdgeColor;
            float4 _TopColor;

            float _Offset;
            float _StrengthUnder;
            float _StrengthOver;

            float _Edge;
            float _EdgeSmooth;
            float _Cutoff;
            float _TopBlur;

            float2 _Scale;
            float2 _DistScale;
            float2 _ScrollSpeed;
            float _Distortion;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _DistortTex;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.scrPos = ComputeScreenPos(o.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 scrollUV = _Time.x * _ScrollSpeed;

                float2 uvMain = i.worldPos.xz * _Scale;
                float2 uvDistort = i.worldPos.xz * _DistScale;

                //PixelizeUV(uvMain, 128);
                //PixelizeUV(uvDistort, 1280);

                float distort1 = tex2D(_DistortTex, uvDistort + scrollUV).r;
                float distort2 = tex2D(_DistortTex, uvDistort + scrollUV * 0.5).g;

                float distort = saturate((distort1 + distort2) * 0.5);

                float col = tex2D(_MainTex, uvMain + distort * _Distortion + scrollUV * 2).r;

                col += distort;

                float4 output = lerp(_ColdColor, _HotColor, col * _Offset) * _StrengthUnder;

                //half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
                //half4 edgeLine = 1 - saturate(_Edge * (depth - i.scrPos.w));

                half depth = GetDepth(i.scrPos, _Edge);
                half4 edgeLine = 1 - depth;

                //half4 fog = (depth - i.scrPos.w);

                float edge = smoothstep(1 - col, 1 - col + _EdgeSmooth, edgeLine);

                float top = smoothstep(_Cutoff, _Cutoff + _TopBlur, output);

                output *= (1 - edge);
                output *= (1 - top);

                output += (edge * _EdgeColor) * _StrengthOver;
                
                output += top * _TopColor * _StrengthOver;

                UNITY_APPLY_FOG(i.fogCoord, output);

                return output;
            }
            ENDCG
        }
    }
}
