Shader "自定义/雾效"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _Color("Fog Color", color) = (1,1,1,1)
        _FogStrength("Fog 强度", Range(0,1)) = 0.5
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            sampler2D _MainTex;
            float _FogStrength;
            

            fixed4 _Color;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv: TEXCOORD1;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
            }

            float rand(float2 p){
				return frac(sin(dot(p ,float2(12.9898,78.233))) * 43758.5453);
			}

			float noise(float2 x)
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
				float res = 0;
				float w = 4;
				for(int i=0;i<4;++i)//i为叠加的层数，对应噪声的密度
				{
					res += noise(x * w);
					w *= 1.5;
				}
				return res*scale;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 col_src = tex2D(_MainTex, i.uv).rgb;
                float rd = fbm(i.uv+_Time.x);//噪声函数
                fixed3 col_fog = _Color.rgb*rd*_FogStrength;
                fixed3 col_out = lerp(col_src,col_fog,0.2);

                return fixed4(col_out,1);
            }
            ENDCG
        }
    }
}