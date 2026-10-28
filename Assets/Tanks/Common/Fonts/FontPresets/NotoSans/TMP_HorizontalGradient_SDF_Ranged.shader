
Shader "Custom/TMP_HorizontalGradient_SDF_Ranged"
{
    Properties
    {
        _ColorA ("Left Color", Color) = (0,1,0,1)
        _ColorB ("Right Color", Color) = (0,0,1,1)
        _MainTex ("Font Atlas", 2D) = "white" {}
        _MinX ("Min X", Float) = -0.5
        _MaxX ("Max X", Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Lighting Off
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ColorA;
            float4 _ColorB;
            float _MinX;
            float _MaxX;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 objPos : TEXCOORD1;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.objPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float sdf = tex2D(_MainTex, i.uv).a;
                float width = fwidth(sdf);
                float alpha = smoothstep(0.5 - width, 0.5 + width, sdf);

                float gradient = saturate((i.objPos.x - _MinX) / (_MaxX - _MinX));
                float4 gradColor = lerp(_ColorA, _ColorB, gradient);

                return float4(gradColor.rgb, alpha * gradColor.a);
            }
            ENDCG
        }
    }
    FallBack "TextMeshPro/Distance Field"
}
