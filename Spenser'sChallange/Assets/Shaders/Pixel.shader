// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Pixel"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScreenWidth("ScreenWidth", float) = 512
		_ScreenHeight("ScreenHeight", float) = 512
		_xPixels("xPixels", float) = 15
		_yPixels("yPixels", float) = 10
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
	
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _ScreenWidth;
			half _xPixels;
			half _yPixels;
			half _ScreenHeight;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
	
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				half dx = _xPixels * (1. / _ScreenWidth);
				half dy = _yPixels * (1. / _ScreenHeight);
				half2 coord = half2(dx*floor(i.uv.x / dx), dy * floor(i.uv.y / dy));
				col = tex2D(_MainTex, coord);
				return col;
			}
			ENDCG
		}
	}
}
