Shader "Custom/Fog/Fog_Perlin"{
    
    Properties{
        _MainTex("主纹理", 2D) = "white" {}
        _Color("雾颜色", color) = (1,1,1,1)
        _FogPower("雾强度", Range(0,1)) = 0.5
        _FogVisibility("雾密度/可见性", Range(0,1)) = 0.2
        _Octaves("分型次数",int) = 4
        _Frequency("采样频率",float) = 1.0
        _Amplitude("幅度",float) = 0.5
        _Lacunarity("频率增加倍数",float) = 2.0
        _Gain("幅度增加倍数",float) = 0.5
        _SpeedX("X速度",float) = 2
        _SpeedY("Y速度",float) = 2
        _NoiseUnitCount("方格个数/噪声密度",float) = 5.0
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
            float _FogVisibility;
            float _SpeedX;
            float _SpeedY;
            float _NoiseUnitCount;
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
        
            half2 rand(float2 p){
                float theta = 52 * sin(p.x * 6+ p.y * 5);//随便编一个
                //float theta = sin(666 + p.x * 5678 + p.y * 1234) * 4321;
                return half2(cos(theta),sin(theta));//单位向量
                
            }

            float2 smoothLerp(float2 x){
               return x * x * x * (x * (x * 6 - 15) + 10);
             
            }

            float perlinNoise(float2 x)
            {
                float2 i = floor(x);
                float2 uv = frac(x);

                float2 posa = uv;
                float2 posb = uv - float2(1.0, 0.0);
                float2 posc = uv - float2(0.0, 1.0);
                float2 posd = uv - float2(1.0, 1.0);

                float2 grada = rand(i);
                float2 gradb = rand(i + float2(1.0, 0.0));
                float2 gradc = rand(i + float2(0.0, 1.0));
                float2 gradd = rand(i + float2(1.0, 1.0));

                float dot1=dot(posa,grada) / 2 + 0.5f;
                float dot2=dot(posb,gradb) / 2 + 0.5f;
                float dot3=dot(posc,gradc) / 2 + 0.5f;
                float dot4=dot(posd,gradd) / 2 + 0.5f;
      
                float2 u = smoothLerp(uv);

                float x1 = lerp(dot1,dot2,u.x);
                float x2 = lerp(dot3,dot4,u.x);
                return lerp(x1,x2,u.y);
            }



            float fbm(float2 x)
            {
                float value = 0;
                float weight = 0;
                float frequency = _Frequency;
                float amplitude = _Amplitude;
                for(int i=0;i<_Octaves;++i)
                {
                    value += perlinNoise(x * frequency) * amplitude;
                    frequency *= _Lacunarity;
                    weight +=amplitude;
                    amplitude *= _Gain;
                }
                return value/weight;
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
                float noise = perlinNoise(i.uv*_NoiseUnitCount+_Time.x*float2(_SpeedX,_SpeedY));//噪声函数
                float3 fogColor = _Color.rgb*noise*_FogPower;
                float3 final = lerp(mainColor,fogColor,_FogVisibility);
                return float4(final,1.0f);
            }           
            ENDHLSL
        }
    }
}