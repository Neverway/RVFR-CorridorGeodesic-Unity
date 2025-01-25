void GetTriplanarUVChannels_float(float3 normal, float sharpness, out float3 weights)
{
	weights = abs(normal);
	weights = pow(weights, sharpness);
    weights /= (weights.x + weights.y + weights.z);
}