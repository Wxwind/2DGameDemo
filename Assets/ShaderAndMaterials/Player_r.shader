Shader "Player/R"{
    
    Properties{
        [NoScaleOffset]_MainTex("主纹理", 2D) = "white" {}
        _Color("颜色", color) = (1,1,1,1)

    }
    SubShader{
        
        Pass{
            Tags { 
                "LightMode" = "UniversalForward"
                "RenderType" = "Transparent"
            } 

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite off
            //Blend  OneMinusSrcAlpha SrcAlpha

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            CBUFFER_END
            TEXTURE2D (_MainTex);
            SAMPLER(sampler_MainTex);
            

            struct a2v {
                float4 positionOS : POSITION;              
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };


            v2f vert(a2v v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv= v.uv;
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float3 mainColor = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv).rgb;
                float3 res=mainColor*_Color.rgb;
                return float4(mainColor,_Color.a);
            }           
            ENDHLSL
        }
    }
}