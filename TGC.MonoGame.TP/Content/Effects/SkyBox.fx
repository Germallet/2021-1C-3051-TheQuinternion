﻿#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 ViewProjection;

float3 CameraPosition;

texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state
{
    texture = <SkyBoxTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = Mirror;
    AddressV = Mirror;
};

// ===== Skybox =====

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 ScreenSpacePosition : TEXCOORD1;
    float3 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, ViewProjection);

    float4 VertexPosition = mul(input.Position, World);
    output.TextureCoordinate = VertexPosition.xyz - CameraPosition;

    output.ScreenSpacePosition = output.Position;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float depth = input.ScreenSpacePosition.z / input.ScreenSpacePosition.w;
    return float4(texCUBE(SkyBoxSampler, normalize(input.TextureCoordinate)).rgb, depth);
}

// ===== Techniques =====

technique Skybox
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}