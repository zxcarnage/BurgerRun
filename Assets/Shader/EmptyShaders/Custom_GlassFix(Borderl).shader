Shader "Custom/GlassFix(Borderl)" {
	Properties {
		_Color ("Color", Vector) = (0.572549,0.9882354,0.9921569,1)
		_SinColor ("Sin Color", Vector) = (0.572549,0.9882354,0.9921569,1)
		_Transparent ("Transparent", Range(0, 1)) = 0.5
		_Sin ("Sin", Float) = 0
		_Emmision ("Emmision", Range(0, 1)) = 0
		_SinPower ("SinPower", Range(0, 1)) = 0
		_ShadowColor ("Shadow Color", Vector) = (0.5471698,0.6199778,1,0)
		_BorderX ("BorderX", Float) = 2
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}