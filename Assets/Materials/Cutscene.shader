Shader "Custom/Cutscene"
{
    Properties
    {
        _MainTex("BaseMap", 2D) = "white" {}
        _NoiseTex("NoiseMap", 2D) = "white" {}
        _ShiftColor ("Shift Color", Color) = (1,1,1,1)
        _BlendColor ("Blend Color", Color) = (1,1,1,1)
        _DisplayPercentage ("Display Percentage", Range(0, 1)) = 1
        _Scale ("Height Scale", Range(-1, 0.5)) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
        }

        Pass
        {
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "DispersedDither.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _ShiftColor;
            fixed4 _BlendColor;
            float _DisplayPercentage;
            float _Scale;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 frag(v2f i) : SV_Target
            {
                // transparent if being clamped
                if (i.uv.y < _Scale || i.uv.y > 1 - _Scale)
                    return fixed4(1, 1, 1, 0);
                // transparent if gt display percentage
                if (abs(2 * i.uv.y - 1) > _DisplayPercentage)
                    return fixed4(1, 1, 1, 0);
                // if has noise texture then apply dithering
                float4 cNoise = tex2D(_NoiseTex, i.uv + DispersedDither(0.33, 3));
                cNoise = saturate(cNoise) - 0.5;
                // straightforward
                float aspectRatio = 1 / (1.0 - 2.0 * _Scale);
                float2 mainUv = i.uv + 0.005 * cNoise.rg;
                mainUv.y = mainUv.y - _Scale * aspectRatio;
                fixed4 c = tex2D(_MainTex, mainUv);
                c = fixed4(1, 1, 1, 1) - (fixed4(1, 1, 1, 1) - c) * (fixed4(1, 1, 1, 1) - _ShiftColor);
                c *= _BlendColor;
                return c;
            }

            ENDCG
        }
    }
    FallBack Off
}
