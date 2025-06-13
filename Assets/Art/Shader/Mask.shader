Shader "Custom/test" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _MaskTex("_MaskTex", 2D) = "white" {}
    }

        SubShader{
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "IgnoreProjector" = "True"
            }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float2 uv_A : TEXCOORD2;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float2 uv_A : TEXCOORD2;
                    float4 vertex : SV_POSITION;
                    float3 worldPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _MaskTex;
                float4 _MaskTex_ST;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    //o.uv = v.uv;
                    o.uv_A = TRANSFORM_TEX(v.uv_A, _MaskTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // 计算基于世界坐标的UV
                    float2 worldUV = i.worldPos.xy;
                    fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

                    // 计算重复UV
                    float2 repeatedUV = frac(worldUV);

                    // 采样纹理
                    fixed4 col = tex2D(_MaskTex, worldUV);
                    fixed4 Finalcol = tex2D(_MainTex, i.uv_A);

                    return col;
                }
                ENDCG
            }
    }
}