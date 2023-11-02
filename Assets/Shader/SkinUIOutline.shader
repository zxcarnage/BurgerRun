Shader "Test/ClippingMask"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorClosed ("ColorClosed", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _ClippingCentre ("Clipping Centre", Vector) = (0,0,0,0)
        _Plane ("Plane", Vector) = (1,0.7,0.7,0)
        _invert("invert", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200
        Cull off
        Pass

        {
            Stencil
            {
                Ref 1
                Comp equal
            }

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

            float4 _Color;
            float4 _ColorClosed;
            float _Glossiness;
            float _Metallic;
            float _invert;
            uniform float _offset;

            uniform float3 _ClippingCentre;
            uniform float3 _Plane;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            half4 frag(v2f i) : SV_Target
            {
                float3 worldPos = i.vertex.xyz;
                half3 albedo;
                half alpha;
                if ((_offset - dot((worldPos - _ClippingCentre), _Plane)) * (1 - 2 * _invert) < 0)
                {
                    half4 c = tex2D(_MainTex, i.uv) * _Color;
                    albedo = c.rgb;
                    half metallic = _Metallic;
                    half smoothness = _Glossiness;
                    alpha = c.a;
                }
                else
                {
                    half4 c = _ColorClosed;
                    albedo = c.rgb;
                    half metallic = _Metallic;
                    half smoothness = _Glossiness;
                    alpha = c.a;
                }
                return half4(albedo, alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}