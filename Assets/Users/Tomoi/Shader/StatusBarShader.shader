//https://qiita.com/Putinu/items/5c250704405bf0244385

Shader "Hidden/StatusBarShader"
{
   Properties // プロパティを置ける
    {
        // 追加
        _FillAmount("FillAmount", Range(0.0, 1.0)) = 1.0 // 値を増減させるためのプロパティ
        _Color ("Color", Color) = (1, 1, 1, 1) // 色を変更するためのプロパティ
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha // 追加

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // 追加
            float4 _MainTex_TexelSize; // テクスチャのサイズに関する情報を持つ
            float _FillAmount;
            fixed4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                // 追加
                fixed4 color = _Color; // 処理中の座標の色を_Colorに
                float h = _MainTex_TexelSize.w; // テクスチャの縦のピクセル数
                float w = _MainTex_TexelSize.z; // テクスチャの横のピクセル数

                // 左端を斜めにする処理
                if (i.uv.y > i.uv.x * w / h) {
                    color.a = 0.0; // 色のアルファ値を0(透明)にする
                }
                // 右端を斜めにする処理
                else if (i.uv.y < i.uv.x * w / h - (w - h) / h * _FillAmount) {
                    color.a = 0.0; // 色のアルファ値を0(透明)にする
                }

                return color;
            }
            ENDCG
        }
    }
}
