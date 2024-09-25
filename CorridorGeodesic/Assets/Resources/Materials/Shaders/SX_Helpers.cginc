#include "SX_SGHelpers.cginc"
sampler2D _CameraDepthTexture;

struct TriplanarUV
{
	float2 uv_front;
	float2 uv_side;
	float2 uv_top;

	float3 weights;
};

struct UVMod
{
	float2 uvScale;
	float2 uvOffset;
};

inline half4 GetDepth(float4 screenPos, float strength)
{
	half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(screenPos)));
	return saturate(strength * (depth - screenPos.w));
}
inline TriplanarUV GetTriplanarUVs(float3 worldPos, float3 normal, float sharpness, sampler2D _MainTex, float4 _MainTex_ST)
{
	TriplanarUV uv;

	uv.uv_front = TRANSFORM_TEX(worldPos.xy, _MainTex);
    uv.uv_side = TRANSFORM_TEX(worldPos.zy, _MainTex);
    uv.uv_top = TRANSFORM_TEX(worldPos.xz, _MainTex);

	float3 weights = abs(normalize(normal));
	weights = pow(weights, sharpness);
    weights /= (weights.x + weights.y + weights.z);

	uv.weights = weights;

	return uv;
}

inline TriplanarUV GetTriplanarUVs(float3 worldPos, float3 normal, float sharpness, UVMod mod, sampler2D _MainTex, float4 _MainTex_ST)
{
	TriplanarUV uv;

	uv.uv_front = TRANSFORM_TEX(worldPos.xy, _MainTex) * mod.uvScale + mod.uvOffset;
    uv.uv_side = TRANSFORM_TEX(worldPos.zy, _MainTex) * mod.uvScale + mod.uvOffset;
    uv.uv_top = TRANSFORM_TEX(worldPos.xz, _MainTex) * mod.uvScale + mod.uvOffset;

	GetTriplanarUVChannels_float(normal, sharpness, uv.weights);

	return uv;
}

inline float4 GetTriplanarTexture(sampler2D _Tex, TriplanarUV uv, UVMod frontMod, UVMod sideMod, UVMod topMod)
{
	float4 tex_front = tex2D(_Tex, uv.uv_front * frontMod.uvScale + frontMod.uvOffset) * uv.weights.z;
	float4 tex_side = tex2D(_Tex, uv.uv_side * sideMod.uvScale + sideMod.uvOffset) * uv.weights.x;
	float4 tex_top = tex2D(_Tex, uv.uv_top * topMod.uvScale + topMod.uvOffset) * uv.weights.y;

	return tex_front + tex_side + tex_top;
}

inline float4 GetTriplanarTexture(sampler2D _Tex, TriplanarUV uv, UVMod mod)
{
	float4 tex_front = tex2D(_Tex, uv.uv_front * mod.uvScale + mod.uvOffset) * uv.weights.z;
	float4 tex_side = tex2D(_Tex, uv.uv_side * mod.uvScale + mod.uvOffset) * uv.weights.x;
	float4 tex_top = tex2D(_Tex, uv.uv_top * mod.uvScale + mod.uvOffset) * uv.weights.y;

	return tex_front + tex_side + tex_top;
}

inline float4 GetTriplanarTexture(sampler2D _Tex, TriplanarUV uv)
{
	float4 tex_front = tex2D(_Tex, uv.uv_front) * uv.weights.z;
	float4 tex_side = tex2D(_Tex, uv.uv_side) * uv.weights.x;
	float4 tex_top = tex2D(_Tex, uv.uv_top) * uv.weights.y;

	return tex_front + tex_side + tex_top;
}