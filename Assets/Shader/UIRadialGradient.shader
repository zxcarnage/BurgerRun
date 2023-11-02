Shader "Custom/UIRadialGradient" {
	Properties {
		_Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (0,0,0,0)
		_Center ("Gradient Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Gradient Radius", Range(0, 1)) = 0.5
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float4 _Center;
            float _Radius;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float dist = distance(i.uv, center) / _Radius;
                dist = saturate(1.0 - dist);
                
                half4 interpolatedColor = lerp(_Color2, _Color1, dist);

                return interpolatedColor;
            }
            ENDCG
        }
	}
	FallBack "Diffuse"
}