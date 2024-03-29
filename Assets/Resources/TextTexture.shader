﻿Shader "Custom/TextTexture"
{
Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius px", Float) = 8
        _Width ("Width px", Float) = 100
        _Height ("Height px", Float) = 100

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        // ---
    }
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Width;
            float _Height;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 newUVInPixels = float2((i.uv.x - 0.5) * _Width + .5, (i.uv.y - 0.5) * _Height + .5);
                float2 muv = abs(newUVInPixels * 2 - 1) - float2(_Width, _Height) + _Radius * 2;
                float d = length(max(0, muv)) / (_Radius * 2);
                float alpha = saturate((1 - d) / fwidth(d));
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.a = min(col.a, alpha);
                return col;
            }
            ENDCG
        }
    }
}