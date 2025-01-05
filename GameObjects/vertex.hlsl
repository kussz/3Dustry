struct vertexData
{
	float4 position : POSITION;
    float2 texCoord : TEXCOORD0;
};

struct pixelData
{
	float4 position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 worldposition : TEXCOORD1;
};

cbuffer perObjectData : register(b0) {
	float4x4 worldViewProjectionMatrix;
	float    time;
	int      timeScaling;
	float2   _padding;
}

pixelData vertexShader(vertexData input) {
	pixelData output = (pixelData)0;
    output.worldposition = input.position;
    float4 position = input.position;

	float scale = 0.5f * sin(time * 0.785f) + 1.0f;
	if (timeScaling > 0)
		position.xyz = mul(scale, position.xyz);

	output.position =
		mul(position, worldViewProjectionMatrix);
    output.TexCoord = input.texCoord;

	return output;
}