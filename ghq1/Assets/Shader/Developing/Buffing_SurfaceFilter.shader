// 磨皮效果, 使用表面滤波方式, 计算量很大不建议移动设备上使用 //

Shader "MJ/UI/Buffing_SurfaceFilter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius("Radius", Range(0, 20)) = 1
		_Threshold("Threshold", Range(0,0.3)) = 0
		_Brightness("Brightness", Range(0,10)) = 0
	}

	CGINCLUDE
	#include "UnityCG.cginc"			

	// #include "../CommonLib/CommonLib.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;								
	};

	sampler2D _MainTex;
	float2 _MainTex_TexelSize;
	float4 _MainTex_ST;
	float _Threshold;
	int _Radius;
	float _Brightness;

	float3 CalculateWeight(float3 xi, float3 x1)
	{
		return 1-abs(xi-x1)/(2.5*_Threshold);
	}

	float4 SurfaceFilter(float2 uv)
	{
		float3 x1 = tex2D(_MainTex, uv).rgb;

		float3 sum_up;				// 分子 //
		float3 sum_down;			// 分母 //

		// 对 (2*_Radius+1)*(2*_Radius+1) 大小的矩形区域内所有像素采样 //
		for(int i=-_Radius; i<=_Radius; i++)
		{
			for(int j=-_Radius; j<=_Radius; j++)
			{
				float2 uv_new = uv + float2(j,i) * _MainTex_TexelSize.xy;
				float3 xi = tex2D(_MainTex, uv_new).rgb;

				sum_up += CalculateWeight(xi, x1)*xi;
				sum_down += CalculateWeight(xi, x1);
			}
		}

		float3 rgb = sum_up/sum_down;
		return float4(rgb,1);
	}

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	float4 frag (v2f i) : SV_Target
	{
		float4 col = SurfaceFilter(i.uv)*_Brightness;
		return col;
	}

	ENDCG

	SubShader
	{
		Tags {
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			}
		LOD 100

		Pass
		{
			Name "Buffing"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}