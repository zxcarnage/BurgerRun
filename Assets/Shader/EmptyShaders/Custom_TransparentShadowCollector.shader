Shader "Custom/TransparentShadowCollector" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,0.5)
		_ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 0.6
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
	Fallback "VertexLit"
}