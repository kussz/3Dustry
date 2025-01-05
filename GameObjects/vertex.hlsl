﻿struct vertexData
{
	float4 position : POSITION;
    float2 texCoord : TEXCOORD0;
};

struct pixelData
{
	float4 position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 worldposition : TEXCOORD1;
    int transp : TEXCOORD2;
    int2 lookingCell : TEXCOORD3;
};

cbuffer perObjectData : register(b0) {
	float4x4 worldViewProjectionMatrix;
    int isTransparent;
    int lookingCellX;
    int lookingCellY;
	float   _padding;
}

pixelData vertexShader(vertexData input) {
	pixelData output = (pixelData)0;
    output.worldposition = input.position;
    output.transp = isTransparent;
    float4 position = input.position;
    output.lookingCell = int2(lookingCellX, lookingCellY);
	output.position =
		mul(position, worldViewProjectionMatrix);
    output.TexCoord = input.texCoord;
	return output;
}