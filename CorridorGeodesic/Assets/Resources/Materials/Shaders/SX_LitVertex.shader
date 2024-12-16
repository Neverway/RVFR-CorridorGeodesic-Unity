Shader "Soulex/Particles/LitVertex"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 vertexColor : COLOR;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 color = tex2D(_MainTex, IN.uv_MainTex) * IN.vertexColor;
            o.Emission = color.rgb;
            //o.Albedo = color.rgb;
            o.Alpha = color.a;

            clip(o.Alpha - 0.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
