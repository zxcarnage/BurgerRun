Shader "Custom/OceanDepthFade" {
	Properties {
		_Fog ("Fog", Vector) = (0,0.8470589,1,1)
		_FogDistanceFar ("FogDistanceFar", Float) = 0
		_FogDistanceNear ("FogDistanceNear", Float) = 0
		_FogPower ("FogPower", Float) = 1
		_Ampltude ("Ampltude", Range(0, 1)) = 0
		_Speed ("Speed", Range(0, 3)) = 0
		_Noise ("Noise", 2D) = "white" {}
		_MainTex ("MainTex", 2D) = "white" {}
		_Brightness ("Brightness", Range(0, 1)) = 0
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}