Shader "Сustom/ToonyLeshaCreativ" {
	Properties {
		_Texture ("Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (0,0,0,0)
		_Color0 ("Color 0", Vector) = (0,0,0,0)
		_Reflection ("Reflection", Range(0, 5)) = 0
		_Float5 ("Float 5", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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