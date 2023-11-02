Shader "Custom/Water"
{
    Properties
    {   
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture (R)", 2D) = "white" {}
        _Tiling ("Tiling", Vector) = (1, 1, 1, 1)
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}   
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
        _SurfaceNoiseScroll("Surface Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _WaterStandartColor("Water Standart Color", Color) = (1,1,1)
        _ColorOverlay("Color Overlay", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            fixed4 _WaterStandartColor;
            fixed4 _ColorOverlay;

            struct v2f
            {
                float2 distortUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float2 noiseUV : TEXCOORD0;
                float4 colorOverlay : COLOR;
            };

            sampler2D _SurfaceNoise;
            sampler2D _MaskTex;
            sampler2D _MainTex;
            float4 _SurfaceNoise_ST;
            float4 _Tiling;
            float2 _SurfaceNoiseScroll;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _SurfaceDistortionAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise) * _Tiling.xy;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                o.colorOverlay = _ColorOverlay;

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                half4 maskColor = tex2D(_MaskTex, noiseUV);
                half4 colorTex = tex2D(_MainTex, noiseUV);

                half4 finalColor = colorTex * _ColorOverlay;

                return finalColor;
            }
            ENDCG
        }
    }
}
