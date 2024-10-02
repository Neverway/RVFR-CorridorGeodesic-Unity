Shader "Soulex/SX_Oil"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Mask", 2D) = "white" {}
        [NoScaleOffset] [Normal] _Normal ("Normal Map", 2D) = "bump" {}

        //_Alpha ("Alpha", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardAdd" }
        LOD 100

        
        BlendOp Max
        Blend One OneMinusSrcAlpha

        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdadd

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            #include "UnityShadowLibrary.cginc"
            #include "SX_Helpers.cginc"

            struct appdata
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                //SHADOW_COORDS(1)
                LIGHTING_COORDS(1, 3)
                float4 position : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Normal;

            //half _Alpha;

            v2f vert (appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.position);

                o.normal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = mul(unity_ObjectToWorld, v.position);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                TRANSFER_SHADOW(o);
                //UNITY_TRANSFER_FOG(o,o.position);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed shadow = LIGHT_ATTENUATION(i);
                fixed4 col = tex2D(_MainTex, i.uv);

                //UNITY_APPLY_FOG(i.fogCoord, col);

                col *= shadow;

                return float4(shadow, shadow, shadow, shadow);
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}
