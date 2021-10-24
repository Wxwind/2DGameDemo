﻿Shader "Custom/Blinnphong"{
    
    Properties{
        _MainTex("MainTex", 2D) = "white" {}
        _Color("Fog Color", color) = (1,1,1,1)
        _FogPower("雾强度", Range(0,1)) = 0.5
        _Octaves("分型次数",int) = 4
        _Frequency("采样频率",float) = 1.0
        _Amplitude("幅度",float) = 0.5
        _Lacunarity("频率增加倍数",float) = 2.0
        _Gain("幅度增加倍数",float) = 0.5
    }
    SubShader{
       
        Pass{

            Tags { "LightMode" = "UniversalForward"} 
            HLSLPROGRAM
     
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float _FogPower;
            float4 _Color;
            int _Octaves;
            float _Frequency;
            float _Amplitude;
            float _Lacunarity;
            float _Gain;
            CBUFFER_END
            TEXTURE2D (_MainTex);
            SAMPLER(sampler_MainTex);
            //sampler2D _MainTex;
            

            struct a2v {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f{
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
        
            float rand(float2 p){
				return frac(sin(dot(p ,float2(12.9898,78.233))) * 43758.5453);
			}

			float perlinNoise(float2 x)
			{
				float2 i = floor(x);
				float2 f = frac(x);

				float a = rand(i);
				float b = rand(i + float2(1.0, 0.0));
				float c = rand(i + float2(0.0, 1.0));
				float d = rand(i + float2(1.0, 1.0));
				float2 u = f * f * f * (f * (f * 6 - 15) + 10);

				float x1 = lerp(a,b,u.x);
				float x2 = lerp(c,d,u.x);
				return lerp(x1,x2,u.y);
			}

			float fbm(float2 x)
			{
				float scale = 1;
				float value = 0;
                float weight = 0;
				float frequency = _Frequency;
                float amplitude = _Amplitude;
				for(int i=0;i<_Octaves;++i)//i为叠加的层数，对应噪声的密度
				{
					value += perlinNoise(x * frequency) * amplitude;
					frequency *= _Lacunarity;
                    weight +=amplitude;
                    amplitude *= _Gain;
				}
				return value/weight*scale;
			}

            v2f vert(a2v v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv= TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
           
            half4 frag(v2f i) : SV_Target
            {
                //float3 mainColor = tex2D(_MainTex,i.uv).rgb;
                float3 mainColor = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).rgb;
                float noise = perlinNoise(i.uv+_Time.x);//噪声函数
                float3 col_fog = _Color.rgb*noise*_FogPower;
                float3 col_out = lerp(mainColor,col_fog,0.2);
                return float4(col_out,0.5f);
            }           
            ENDHLSL
        }
    }
}