﻿Shader "Pixelate"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
			_ScreenWidth("ScreenWidth", float) = 512
		_ScreenHeight("ScreenHeight", float) = 512
		_xPixels("xPixels", Float) = 15
		_yPixels("yPixels", Float) = 10
	}
		SubShader
		{
			Pass
			{
				Tags
				{
					"LightMode" = "ForwardBase"
					"PassFlags" = "OnlyDirectional"
				}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase

				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float4 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 worldNormal : NORMAL;
					float3 viewDir : TEXCOORD1;
					SHADOW_COORDS(2)
				};


				sampler2D _MainTex;
				float4 _MainTex_ST;
				half _ScreenWidth;
				half _xPixels;
				half _yPixels;
				half _ScreenHeight;

				v2f vert(appdata v)
				{

					v2f o;

					

					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.viewDir = WorldSpaceViewDir(v.vertex);
					TRANSFER_SHADOW(o);
					return o;
				}

				float4 _Color;

				float4 frag(v2f i) : SV_Target
				{

					

					float3 normal = normalize(i.worldNormal);

					float NdotL = dot(_WorldSpaceLightPos0, normal);
					float lightintensity;
					float4 sample = tex2D(_MainTex, i.uv);
					if (NdotL > 0)
					{
						if (NdotL > 0.5)
						{
							lightintensity = 1;
						}
						else
						{

							lightintensity = 0.66;

						}

					}
					else
					{
						/*if (NdotL < -0.5)
						{
							lightintensity = 0.5;
						}
						else
						{*/
							lightintensity = .33;
						//}

					}

					return _Color * sample *lightintensity;//(_AmbientColor + light + specular);
				}
				ENDCG
			}
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}
//Just rimDot could be good for a key item or frozen object