Shader "Soulex/Effects/FogSkybox"
{
    Properties
    {
        [NoScaleOffset] _Tex ("Cubemap", Cube) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            samplerCUBE _Tex;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                UNITY_TRANSFER_FOG(o, o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = -normalize(_WorldSpaceCameraPos.xyz - i.worldPos);

                fixed4 col = texCUBE(_Tex, viewDir);

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
