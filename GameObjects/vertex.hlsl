struct vertexData
{
	float4 position : POSITION;
    float2 texCoord : TEXCOORD0;
    float3 normal : NORMAL;
};

struct pixelData
{
	float4 position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 worldposition : TEXCOORD1;
    float3 normal : NORMAL;
    float3 campos : TEXCOORD5;
    float transp : TEXCOORD2;
    int selected : TEXCOORD3;
    int buildify : TEXCOORD4;
    int mainBuilding : TEXCOORD6;
};

cbuffer perObjectData : register(b0) {
    float4x4 worldMatrix;
	float4x4 worldViewProjectionMatrix;
    float isTransparent;
    float3 campos;
    int mainBuilding;
    int isSelected;
    int buildify;
}

pixelData vertexShader(vertexData input) {
	pixelData output = (pixelData)0;
    output.worldposition = mul(input.position, worldMatrix);
    output.transp = isTransparent;
    float4 position = input.position;
    output.selected = isSelected;
	output.position =
		mul(position, worldViewProjectionMatrix);
    output.TexCoord = input.texCoord;
    output.buildify = buildify;
    output.campos = campos;
    output.normal = input.normal;
    output.mainBuilding = mainBuilding;
	return output;
}