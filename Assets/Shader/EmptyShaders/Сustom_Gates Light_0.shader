Shader "Сustom/Gates Light" {
	Properties {
		_TextureSample1 ("Texture Sample 1", 2D) = "white" {}
		_Color0 ("Color 0", Vector) = (0,0,0,0)
		_Color2 ("Color2", Vector) = (0,0,0,0)
		_Opacity ("Opacity", Range(0, 1)) = 0.5
		_Emmision ("Emmision", Range(0, 1)) = 0.5
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}