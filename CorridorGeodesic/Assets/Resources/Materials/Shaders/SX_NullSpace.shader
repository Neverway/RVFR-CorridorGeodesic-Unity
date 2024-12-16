Shader "Soulex/Effects/NullSpace"
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
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "SX_Helpers.cginc"

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
                float4 screenPos : TEXCOORD3;
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

                UNITY_TRANSFER_FOG(o,o.position);
                return o;
            }
            float3 GetInternalProjectionUVs(float3 viewDir){
                return floor(reflect(viewDir * -1, float3(0, 0, 1)) * 64) / 64;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //float3 weights = abs(i.normal);
                //weights /= (weights.x + weights.y + weights.z);

                float2 timeUV = float2(1, 1) * _Time.x;
                float2 screenUV = (i.screenPos / i.screenPos.w);
                screenUV.y *= (_ScreenParams.y/_ScreenParams.x);

                PixelizeUV(screenUV, 256);

                UVMod mod;
                mod.uvScale = 0.25;
                mod.uvOffset = 0;

                TriplanarUV UVs = GetTriplanarUVs(i.worldPos, i.normal, 1, mod, _MainTex, _MainTex_ST);
                
                PixelizeTriplanarUV(UVs, 128);

                //float2 uv_front = TRANSFORM_TEX(i.worldPos.xy, _MainTex) * 0.1 + timeUV;
                //float2 uv_side = TRANSFORM_TEX(i.worldPos.zy, _MainTex) * 0.1 + timeUV;
                //float2 uv_top = TRANSFORM_TEX(i.worldPos.xz, _MainTex) * 0.1 + timeUV;

                //float3 distort_front = tex2D(_DistortTex, uv_front * 0.25).rgb * _DistortIntensity;
                //float3 distort_side = tex2D(_DistortTex, uv_side * 0.25).rgb * _DistortIntensity;
                //float3 distort_top = tex2D(_DistortTex, uv_top * 0.25).rgb * _DistortIntensity;

                //float3 distort = distort_front * weights.z + distort_side * weights.x + distort_top * weights.y;

                mod.uvScale = 0.25;
                mod.uvOffset = timeUV * 0.25;

                float3 distort = GetTriplanarTexture(_DistortTex, UVs, mod).rgb;

                UVMod colTop;
                UVMod colSide;
                UVMod colBottom;

                colTop.uvScale = 1;
                colTop.uvOffset = distort.rg;

                colSide.uvScale = 1;
                colSide.uvOffset = distort.bg;

                colBottom.uvScale = 1;
                colBottom.uvOffset = distort.rb;

                float4 col = GetTriplanarTexture(_MainTex, UVs, colTop, colSide, colBottom) * _Color * 0.5;

                //float4 col_front = tex2D(_MainTex, uv_front + distort.rg);
                //float4 col_side = tex2D(_MainTex, uv_side + distort.bg);
                //float4 col_top = tex2D(_MainTex, uv_top + distort.rb);

                //float4 col = (col_front * weights.z + col_side * weights.x + col_top * weights.y) * _Color * 0.5;

                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);

                float skyWeight = 0.5/3;

                float stars = tex2D(_Stars, screenUV * 10 - timeUV).r;

                stars = pow(stars, 5);

                col += texCUBE(_CubeMap, GetInternalProjectionUVs(viewDir)) * skyWeight;
                col += texCUBE(_CubeMap_1, GetInternalProjectionUVs(viewDir)) * skyWeight;
                col += texCUBE(_CubeMap_2, GetInternalProjectionUVs(viewDir)) * skyWeight;

                col += float4(stars, stars, stars, 0);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
