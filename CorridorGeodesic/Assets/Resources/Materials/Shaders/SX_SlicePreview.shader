Shader "Unlit/SX_SlicePreview"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Overlay Tex", 2D) = "white" {}
        [NoScaleOffset] _EdgeTex ("Edge Tex", 2d) = "white" {}

        _EdgeStrength ("Edge Strength", Float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 position : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float4 screenPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _EdgeTex;

            half _EdgeStrength;

            v2f vert (appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.screenPos = ComputeScreenPos(o.position);

                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float depth = GetDepth(i.screenPos, _EdgeStrength).r;

                UVMod mainMod;
                mainMod.uvScale = 0.25;
                mainMod.uvOffset = 0;



                fixed4 col = tex2D(_MainTex, i.worldPos.xy);
                //fixed4 col = fixed4(i.screenPos.xyz, 1);

                col *= depth;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
