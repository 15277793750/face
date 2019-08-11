Shader "Unlit/Move"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	    _ScrollingXSpeed("Scrolling X Speed",Range(-10,10)) = 2
		_ScrollingYSpeed("Scrolling Y Speed",Range(-10,10)) = -2
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque"  "Queue" = "Transparent" }
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _ScrollingXSpeed;
			fixed _ScrollingYSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed2 uv = i.uv;
			    fixed x = _ScrollingXSpeed * _Time;
			    fixed y = _ScrollingYSpeed * _Time;
			    uv += fixed2(x, y);
				fixed4 c = tex2D(_MainTex, uv);
				return c;
			}
			ENDCG
		}
	}
}
