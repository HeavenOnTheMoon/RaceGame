Shader "Unlit/UnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("_Color",color) = (1,1,1,1)
        _Alpha("_Alpha",float) =1 
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }
        LOD 100
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
       
        struct a2v
        {
            float4 positionOS:POSITION;
            float2 texcoord:TEXCOORD;
        };

        struct v2f
        {
            float4 positionCS:SV_POSITION;
            float2 texcoord:TEXCOORD;
        };

        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _Color;
        float _Alpha;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        ENDHLSL

        Pass
        {
         Tags{ "LightMode" = "UniversalForward" }
           Blend SrcAlpha OneMinusSrcAlpha 
           HLSLPROGRAM
           #pragma vertex Vert
           #pragma fragment Frag

           v2f Vert(a2v i)
           {
               v2f o;
               o.positionCS = TransformObjectToHClip(i.positionOS.xyz);
               o.texcoord = TRANSFORM_TEX(i.texcoord,_MainTex);
               
               return o;

           }

           half4 Frag(v2f i):SV_TARGET
           {
                i.texcoord+=_Time.y/8;
               half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,i.texcoord)*_Color;
               tex.a *= _Alpha;
               return tex;
           }
           ENDHLSL
        }
    }
}