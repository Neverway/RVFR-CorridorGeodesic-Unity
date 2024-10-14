Shader "Soulex/Effects/FogPlane"
{
	Properties
	{
		_ShallowColor("Shallow Color", Color) = (1, 1, 1, .5)
		_DeepColor("Deep Color", Color) = (0, 0, 0, .5)
		_Strength("Fog Strength", Range(0,3)) = 0.5
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque"  "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Cull Off

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
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD2;
			};

			float4 _ShallowColor;
			float4 _DeepColor;
			//uniform sampler2D _CameraDepthTexture;
			float _Strength;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half4 col = lerp(_ShallowColor, _DeepColor, GetDepth(i.scrPos, _Strength));

				col = saturate(col);

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}