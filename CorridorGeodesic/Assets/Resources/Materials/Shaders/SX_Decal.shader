Shader "Soulex/Effects/Decal"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        //[KeywordEnum(None, Add, Multiply)] _ColorMode ("Color Mode", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent-400" "DisableBatching"="True" }
        LOD 100

        ZWrite Off

        Pass
        {
            Blend SrcColor OneMinusSrcColor

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            //#pragma multi_compile _COLORMODE_NONE _COLORMODE_ADD _COLORMODE_MULTIPLY

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
                float4 screenPos : TEXCOORD0;
                float3 ray : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);

                o.position = UnityObjectToClipPos(v.vertex);

                o.ray = worldPos - _WorldSpaceCameraPos;

                o.screenPos = ComputeScreenPos(o.position);

                UNITY_TRANSFER_FOG(o, o.position);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenUv = i.screenPos.xy / i.screenPos.w;

                float2 uv = GetProjectedObjectPos(screenUv, i.ray).xz;

                fixed4 col = tex2D(_MainTex, uv);

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
