struct pixelData
{
	float4 position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 worldposition : TEXCOORD1;
    int transp : TEXCOORD2;
    int selected : TEXCOORD3;
};
Texture2D texture0 : register(t0);
SamplerState sampler0 : register(s0);

float4 pixelShader(pixelData input) : SV_Target
{
    float4 tex = texture0.Sample(sampler0, input.TexCoord);
    if(input.selected==1)
        tex.xyz = tex.xyz / 1.5;
    if(input.transp ==1)
        tex.a = 0.5;
    return tex;
    
}