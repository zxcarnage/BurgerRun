Shader "Pong" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,0)
		_ColorBlink ("Color Blink", Vector) = (1,1,1,0)
		_LightStrenght ("Light Strenght", Range(0, 1)) = 0
		_Highlight ("Highlight ", Range(-1, -0.5)) = -0.95
		_HighlightSmooth ("Highlight Smooth", Range(0.001, 1)) = 1
		[Toggle(_STATICHIGHLIGHTS_ON)] _StaticHighLights ("Static HighLights", Float) = 0
		_Float1 ("Float 1", Float) = 0
		_F3 ("F3", Float) = 0
		_F2 ("F2", Float) = 0
		_F1 ("F1", Float) = 0
		_Color0 ("Color 0", Vector) = (0,0,0,0)
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