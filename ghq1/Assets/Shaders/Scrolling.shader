Shader "Shader/scence01/Scrolling" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ScrollingXSpeed("Scrolling X Speed",Range(-10,10)) = 2
		_ScrollingYSpeed("Scrolling Y Speed",Range(-10,10)) = 2
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};
		
		fixed _ScrollingXSpeed;
		fixed _ScrollingYSpeed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed2 uv = IN.uv_MainTex;
			uv += fixed2(_ScrollingXSpeed, _ScrollingYSpeed) * _Time;
			half4 c = tex2D (_MainTex, uv);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			// Metallic and smoothness come from slider variables
		}
		ENDCG
	}
	FallBack "Diffuse"
}
