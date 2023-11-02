Shader "Custom/SpriteEdgeTransparency"
{
    Properties
    {
        _MainTex ("Source Image", 2D) = "white" {}
        _EdgeTransparency ("Edge Transparency", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _EdgeTransparency;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.texcoord);
                half2 uv = i.texcoord;
                half2 center = 0.5;
                half dist = distance(uv, center);
                half transparency = 1.0 - lerp(0, _EdgeTransparency, dist);
                col.a *= transparency;
                return col;
            }
            ENDCG
        }
    }
}
