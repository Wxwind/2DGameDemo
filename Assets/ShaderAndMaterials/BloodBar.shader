Shader "Custom/BloodBar"{
    
    Properties{
        _RampTex("渐变纹理", 2D) = "white" {}
        _health("生命值", Range(0,1)) = 0.5

    }
    SubShader{
        
        Pass{
            Tags { 
                "LightMode" = "UniversalForward"
                "RenderType" = "Transparent"
            } 
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _RampTex_ST;
            float _health;
            CBUFFER_END
            TEXTURE2D (_RampTex);
            SAMPLER(sampler_RampTex);
            //sampler2D _MainTex;
            

            struct a2v {
                float4 positionOS : POSITION;              
                float2 uv : TEXCOORD0;
            };
            struct v2f{
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            

            float2 InverseLerp(float min,float max,float x){
                return (x-min)/(max-min);
                
            }


            v2f vert(a2v v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv= TRANSFORM_TEX(v.uv, _RampTex);
                return o;
            }
            
            half4 frag(v2f i) : SV_Target
            {
                float3 mainColor = SAMPLE_TEXTURE2D(_RampTex,sampler_RampTex,float2(_health,0.5)).rgb;
                float healthMask = _health > i.uv.x;
                return float4(mainColor,healthMask);
            }           
            ENDHLSL
        }
    }
}