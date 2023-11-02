Shader "Custom/OutlineShader"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _FillColor ("Fill Color", Color) = (0, 0, 1, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _OutlineColor;
            float4 _FillColor;
            float _OutlineWidth;
            float _Visibility;
            

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                return o;
            }
            
             half4 frag (v2f i) : SV_Target
            {
                half distance = ddx(i.pos.x) + ddy(i.pos.y);
                half insideOutline = smoothstep(0.5 - _OutlineWidth, 0.5 + _OutlineWidth, distance);
                half4 finalColor = lerp(_OutlineColor, _FillColor, insideOutline);

                return finalColor;
            }
            ENDCG
        }
    }
}
