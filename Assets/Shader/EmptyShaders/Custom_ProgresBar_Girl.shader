Shader "Custom/ProgresBar_Girl" {
	Properties {
		_Cutoff ("Mask Clip Value", Float) = 0.5
		_Color ("Color", Vector) = (0,0,0,0)
		_Texture ("Texture", 2D) = "white" {}
		_LightPower ("Light Power", Range(0, 1)) = 1
		_ContourLightPower ("Contour Light Power", Range(0, 1)) = 0
		_ContourLight ("Contour Light", Vector) = (0,0,0,0)
		_FrontLightPower ("Front Light Power", Range(0, 1)) = 0
		_FrontLight ("Front Light", Vector) = (0,0,0,0)
		_ShadowOpacity ("Shadow Opacity", Range(0, 1)) = 0
		_DiffuseLightStrength ("Diffuse Light Strength", Float) = 0
		_OutlineWidth ("OutlineWidth", Float) = 0
		_OutlineColor ("OutlineColor", Vector) = (1,1,1,1)
		_NotFilled ("NotFilled", Vector) = (1,1,1,1)
		_NotFilledOutline ("NotFilledOutline", Vector) = (1,1,1,1)
		_BlendMax ("BlendMax", Float) = 1
		_BlendMin ("BlendMin", Float) = 1
		_Blend ("Blend", Range(0, 1)) = 0.5
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