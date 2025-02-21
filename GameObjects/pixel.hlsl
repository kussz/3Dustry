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
Texture2D texture0 : register(t0);
SamplerState sampler0 : register(s0);

float4 pixelShader(pixelData input) : SV_Target
{
    float4 tex = texture0.Sample(sampler0, input.TexCoord);
    if(input.mainBuilding==1)
    {
        tex.a = 0.5;
    }
    else
    {
        if(input.selected==1)
            tex.xyz = tex.xyz / 1.5;
        if(input.buildify==1)
        {
            float asp = input.transp - input.worldposition.y;
            if(input.transp >-0.5)
                if(input.transp+0.1<input.worldposition.y)
                {
                    tex.a = 0.5;
                    tex.xyz *= 0.5;
                    
                }
                else if (asp+0.1 < 0.1)
                    tex.xyz += float4(1-(asp + 0.1) * 10, 1-(asp + 0.1) * 10, 0, 1);
        }
    }
    float3 lightDir1 = normalize(float3(3, 1, 5));
    float3 lcolor = float3(1, 1, 1);
    float lintensity = 0.8;
    float3 normal = normalize(input.normal);
    
    float diff = max(dot(normal, lightDir1), 0.0);
    float3 diffuseColor = tex.rgb * lcolor.xyz * diff * lintensity;
        
    float3 viewDir = normalize(input.campos.xyz - input.worldposition.xyz);
    float3 halfwayDir = normalize(lightDir1 + viewDir);
    float spec = pow(max(dot(normal, halfwayDir), 0.0), 20);
    float3 specularColor = lcolor.xyz * spec * lintensity;

    float3 ambientColor = tex.rgb * float3(0.01, 0.01, 0.01);

    tex.rgb = max(ambientColor + diffuseColor + specularColor, tex.rgb);
    
    return tex;
    
}