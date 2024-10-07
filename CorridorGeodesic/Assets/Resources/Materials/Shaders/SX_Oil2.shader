Shader "Soulex/SX_Oil2"
{
    Properties
    {
        _Color ("Color", Vector) = (1, 1, 1, 1)
        [NoScaleOffset] _MainTex ("Mask", 2D) = "white" {}
        [NoScaleOffset] _IrridesenceTex ("irridesence Tex", 2D) = "white" {}
        [NoScaleOffset] [Normal] _Normal ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 200

        //BlendOp Max
        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite Off

        //#include "SX_Helpers.cginc"

        CGPROGRAM
        #pragma surface surf Standard addshadow alpha:fade

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _IrridesenceTex;
        sampler2D _Normal;

        half4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            //float3 normal = WorldNormalVector (IN, o.Normal);

            half fresnel = saturate(1 - (dot(o.Normal, IN.viewDir)));
            fresnel = pow(fresnel, 5);

            o.Albedo = lerp(c.rgb, tex2D(_IrridesenceTex, fresnel).rgb, fresnel);

            o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));

            //o.Smoothness = 1;

            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}