Shader "Custom/MaskShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WhiteFactor("_WhiteFactor", Range(0.0, 20.0)) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		LOD 100
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
			float _Radius = 0.45;
			float _WhiteFactor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

#define REVERSE_TRANSFORM_TEX(tex, name) (((tex) - name##_ST.zw) / name##_ST.xy)

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if (length(REVERSE_TRANSFORM_TEX(i.uv, _MainTex) - float2(0.5, 0.5)) > _Radius)
				{
					col.a = 0;
				}
				col.rgb *= _WhiteFactor;
				return col;
			}
			ENDCG
		}
	}
}
