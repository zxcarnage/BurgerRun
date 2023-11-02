Shader "Custom/Gates Light" {
    Properties {
        _TextureSample1 ("Texture Sample 1", 2D) = "white" {}
        _Color0 ("Color 0", Color) = (0,0,0,0)
        _Color2 ("Color2", Vector) = (0,0,0,0)
        _Opacity ("Opacity", Range(0, 1)) = 0.5
        _Emission ("Emission", Range(0, 1)) = 0.5
        [HideInInspector] _texcoord ("", 2D) = "white" {}
        [HideInInspector] __dirty ("", Float) = 1
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TextureSample1;
            float4 _Color0;
            float4 _Color2;
            float _Opacity;
            float _Emission;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                half4 texColor = tex2D(_TextureSample1, i.uv);

                // Calculate transparency based on the inverted red channel (1 - texColor.r)
                // and apply the _Opacity parameter
                half4 finalColor;
                finalColor.rgb = _Color0.rgb;
                finalColor.a = (1 - texColor.r) * _Opacity;

                return finalColor;
            }
            ENDCG
        }
    }
}



