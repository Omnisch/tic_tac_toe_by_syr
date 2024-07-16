Shader "Custom/FrameDither"
{
    Properties
    {
        _MainTex ("BaseMap", 2D) = "white" {}
        _PostTex ("SubBaseMap", 2D) = "white" {}
        _NoiseTex ("NoiseMap", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Range(-0.5, 0.5)) = 0.01
        _Frequency("Frequency", Range(0.1, 1)) = 0.33
        _FrameCount("Loop Frame Count", Int) = 3
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "PreviewType"="Plane"
        }
        LOD 200

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert_img
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _PostTex;
            sampler2D _NoiseTex;

            float _NoiseScale;
            float _Frequency;
            int _FrameCount;

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 frag(v2f i) : SV_Target
            {
                float distort = (uint)(_Time.y / _Frequency) % _FrameCount;
                float2 cycle = float2(cos(distort), sin(distort + 3.14));
                float4 cNoise = tex2D(_NoiseTex, i.uv + cycle);
                float4 cMain = tex2D(_MainTex, i.uv + _NoiseScale * cNoise.rg);
                float4 cPost = tex2D(_PostTex, i.uv);
                float4 c = cMain * cPost;

                return c;
            }
            ENDCG
        }
    }
}
