Shader "Soulex/Triplanar"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _Sharpness ("Triplanar Blend", Range(1,64)) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 worldPos;
            INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        half _Sharpness;
        fixed4 _Color;

        float4 _MainTex_ST;

        sampler2D _Normal;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 weights = abs(IN.worldNormal);

            weights = pow(weights, _Sharpness);
            weights /= (weights.x + weights.y + weights.z);

            float2 uv_front = TRANSFORM_TEX(IN.worldPos.xy, _MainTex);
            float2 uv_side = TRANSFORM_TEX(IN.worldPos.zy, _MainTex);
            float2 uv_top = TRANSFORM_TEX(IN.worldPos.xz, _MainTex);

            float4 col_front = tex2D(_MainTex, uv_front) * weights.z;
            float4 col_side = tex2D(_MainTex, uv_side) * weights.x;
            float4 col_top = tex2D(_MainTex, uv_top) * weights.y;

            float3 norm_front = UnpackNormal(tex2D(_Normal, uv_front)) * weights.z;
            float3 norm_side = UnpackNormal(tex2D(_Normal, uv_side)) * weights.x;
            float3 norm_top = UnpackNormal(tex2D(_Normal, uv_top)) * weights.y;

            fixed4 c = col_front + col_side + col_top;
            float3 n = norm_front + norm_side + norm_top;

            o.Albedo = c.rgb;
            //o.Normal = n;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
