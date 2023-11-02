Shader "Skybox Gradient" {
	Properties {
		_Texture ("Texture", 2D) = "white" {}
		_Top ("Top", Vector) = (1,1,1,0)
		_Bottom ("Bottom", Vector) = (0,0,0,0)
		_mult ("mult", Float) = 1
		_pwer ("pwer", Float) = 1
		[Toggle(_SCREENSPACE_ON)] _Screenspace ("Screen space", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
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