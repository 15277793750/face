// 磨皮效果, 使用双边滤波方式, 计算量很大不建议移动设备上使用 //

Shader "MJ/UI/Buffing_BilateralFilter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SigmaS("Sigma Space", Range(0.1, 10)) = 1
		_SigmaR("Sigma Range", Range(0.01, 0.3)) = 1
		_Radius("Radius", Range(0, 10)) = 1
		_Brightness("Brightness", Range(0.1, 20)) = 0.5
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

	float _SigmaS;
	float _SigmaR;
	int _Radius;
	float _Brightness;

	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}
	
	float Luminance(float3 color)
	{
		return dot(color, float3(0.2125, 0.7154, 0.0721));
	}	

	float4 BilateralFilter(float2 uv)
	{
		float i = uv.x;
		float j = uv.y;
		float sigmaSSquareMult2 = (2*_SigmaS*_SigmaS);
		float sigmaRSquareMult2 = (2*_SigmaR*_SigmaR);

		float3 centerCol = tex2D(_MainTex, uv).rgb;					// 中心点像素的颜色 //
		float centerLum = Luminance(centerCol);						// 中心点像素的亮度 //

		float3 sum_up;												// 分子 //
		float3 sum_down;											// 分母 //
		for(int k=-_Radius; k<=_Radius; k++)
		{
			for(int l=-_Radius; l<=_Radius; l++)
			{
				float2 uv_new = uv+_MainTex_TexelSize.xy*float2(k,l);
				float3 curCol = tex2D(_MainTex, uv_new).rgb;		// 当前像素的颜色 //
				float curLum = Luminance(curCol);						// 当前像素的亮度 //
				float3 deltaColor = curCol-centerCol;
				float len = dot(deltaColor, deltaColor);
				// float exponent = -((i-k)*(i-k)+(j-l)*(j-l))/sigmaSSquareMult2 - (curLum-centerLum)*(curLum-centerLum)/sigmaRSquareMult2;
				float exponent = -((i-k)*(i-k)+(j-l)*(j-l))/sigmaSSquareMult2 - len/sigmaRSquareMult2;
				float weight = exp(exponent);
				sum_up += curCol*weight;
				sum_down += weight;
			}
		}

		float3 rgb = sum_up/sum_down;
		return float4(rgb*_Brightness, 1);
	}

	float4 frag (v2f i) : SV_Target
	{
		return BilateralFilter(i.uv);
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
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}