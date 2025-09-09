Shader "LF/FishingTrip/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _Tiling ("Tiling", Float) = 1
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 worldUV : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed3 _Color1;
            fixed3 _Color2;
            float _Tiling;

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldUV = worldPos.xz * _Tiling;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.worldUV;

                float  bottom1 = tex2D(_MainTex, uv * float2(2.5,1.7)).b;
                float  bottom2 = tex2D(_MainTex, uv * float2(4.5,2.3)).b;
                float  bottomMask = tex2D(_MainTex, uv * float2(2,1) + _Time.y * -0.05).r * 2.5 - 0.8;
                
                float3 bottomMaskColor = tex2D(_MainTex, uv * float2(2.5,1.25) + _Time.y * 0.025).r;
                
                float  bottom = bottom1 * saturate(bottomMask) + bottom2 * (1 - saturate(bottomMask));
                bottom *= tex2D(_MainTex, uv * float2(2.25,0.85) + _Time.y * 0.01 + bottomMask * 0.03).g * 1.3 - 0.5;
                bottom = saturate(bottom);
                fixed3 bColor = _Color1 + bottom * _Color1;

                float shimmers = 
                    tex2D(_MainTex, uv * float2(3,1.5) + _Time.y * float2(0.00,-0.00)).a *
                    tex2D(_MainTex, uv * float2(1.8,1) + _Time.y * float2(0.013,-0.02)).a;
                
                fixed3 col = lerp(bColor, _Color2 * bottomMaskColor, bottomMaskColor.r) + shimmers;

                return fixed4(col,1);
            }
            ENDCG
        }
    }
}