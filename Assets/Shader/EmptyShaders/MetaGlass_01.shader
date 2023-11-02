Shader "MetaGlass_01" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,0)
		_Opacity ("Opacity", Range(0, 1)) = 0.4
		_Emmision ("Emmision", Range(0, 1)) = 0
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
	//CustomEditor "ASEMaterialInspector"
}