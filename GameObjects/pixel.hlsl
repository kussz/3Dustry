struct pixelData
{
	float4 position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 worldposition : TEXCOORD1;
    float transp : TEXCOORD2;
    int selected : TEXCOORD3;
    int buildify : TEXCOORD4;
};
Texture2D texture0 : register(t0);
SamplerState sampler0 : register(s0);

float4 pixelShader(pixelData input) : SV_Target
{
    float4 tex = texture0.Sample(sampler0, input.TexCoord);
    if(input.selected==1)
        tex.xyz = tex.xyz / 1.5;
    if(input.buildify==1)
    {
        float asp = input.transp - input.worldposition.y;
        if(input.transp >-0.5)
            if(input.transp+0.1<input.worldposition.y)
                tex.a = 0.2;
            else if (asp+0.1 < 0.1)
                tex.xyz += float4(1-(asp + 0.1) * 10, 1-(asp + 0.1) * 10, 0, 1);
    }
    
    return tex;
    
}