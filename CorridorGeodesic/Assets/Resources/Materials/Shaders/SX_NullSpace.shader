// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Soulex/SX_NullSpace"
{
    Properties
    {
        _Color ("Color", COLOR) = (0, 0, 0, 1)
        _MainTex ("Noise Texture", 2D) = "white" {}
        _Stars ("Star Texture", 2d) = "black" {}
        _DistortTex ("Distort Texture", 2D) = "white" {}
        _DistortIntensity ("Distort Intensity", Float) = 0.25
        _CubeMap ("Null Skybox", Cube) = "" {}
        _CubeMap_1 ("Null Skybox 1", Cube) = "" {}
        _CubeMap_2 ("Null Skybox 2", Cube) = "" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 screenPos : TEXCOORD1;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 position : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normal : NORMAL;
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            float _DistortIntensity;
            sampler2D _DistortTex;
            sampler2D _Stars;
            samplerCUBE _CubeMap;
            samplerCUBE _CubeMap_1;
            samplerCUBE _CubeMap_2;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.screenPos = ComputeScreenPos(o.position);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float3 GetInternalProjectionUVs(float3 viewDir, float intensity){
                float3 cubeSampleUVs = reflect(viewDir * -1, float3(0, 0, 1));

                cubeSampleUVs = lerp(cubeSampleUVs, _WorldSpaceCameraPos.xyz, 0.01);

                return cubeSampleUVs;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float3 weights = abs(i.normal);
                weights = weights / (weights.x, + weights.y + weights.z);

                float2 timeUV = float2(1, 1) * _Time.x;
                float2 screenUV = (i.screenPos / i.screenPos.w);
                screenUV.y *= (_ScreenParams.y/_ScreenParams.x);

                float2 uv_front = TRANSFORM_TEX(i.worldPos.xy, _MainTex) * 0.1 + timeUV;
                float2 uv_side = TRANSFORM_TEX(i.worldPos.zy, _MainTex) * 0.1 + timeUV;
                float2 uv_top = TRANSFORM_TEX(i.worldPos.xz, _MainTex) * 0.1 + timeUV;

                float3 distort = tex2D(_DistortTex, (screenUV + timeUV) * 0.25).rgb * _DistortIntensity;

                fixed4 col_front = tex2D(_MainTex, uv_front + distort.rg);
                fixed4 col_side = tex2D(_MainTex, uv_side + distort.bg);
                fixed4 col_top = tex2D(_MainTex, uv_top + distort.rb);

                fixed4 col = (col_front * weights.z + col_side * weights.x + col_top * weights.y) * _Color;

                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);

                col += texCUBE(_CubeMap, GetInternalProjectionUVs(viewDir, 0));
                col += texCUBE(_CubeMap_1, GetInternalProjectionUVs(viewDir, 0.02));
                col += texCUBE(_CubeMap_2, GetInternalProjectionUVs(viewDir, 0.05));

                col /= 4;

                float stars = tex2D(_Stars, screenUV * 10 - timeUV).r;

                stars = pow(stars, 5);

                col += float4(stars, stars, stars, 0);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
