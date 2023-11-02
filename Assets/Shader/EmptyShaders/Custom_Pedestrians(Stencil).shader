Shader "Custom/Pedestrians(Stencil)" {
	Properties {
		_TextureSample ("Texture Sample ", 2D) = "white" {}
		_Shading ("Shading", Range(0, 1)) = 0
		_Color ("Color", Vector) = (1,1,1,0)
		_Emmision ("Emmision", Range(0, 1)) = 0
		_DistanceColor ("Distance Color", Vector) = (1,1,1,0)
		_DistanceMax1 ("Distance Max", Float) = 0
		_DistanceMin1 ("Distance Min", Float) = 0
		_DistanceShading1 ("Distance Shading", Range(0, 1)) = 0
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