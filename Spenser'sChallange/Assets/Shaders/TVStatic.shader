// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TVStatic"
{
	//Properties
	//{
	//	_Color("Color", Color) = (0.5, 0.65, 1, 1)
	//	_MyArr("Tex", 2DArray) = "" {}
	//	_SliceRange("Slices", Range(0,16)) = 6
	//	_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
	//	_TextureScale("Texture Scale", Vector) = (1,1,1)
	//	_TextureRotate("Texture Rotate", Vector) = (0,0,0,0)
	//	_TextureTranslate("Texture Translate", Vector) = (0,0,0,0)

	//}
	//	//Tags { "RenderType"="Opaque" }
	//	//LOD 100

	//		SubShader
	//	{
	//		Pass
	//		{
	//			Tags
	//			{
	//				"LightMode" = "ForwardBase"
	//				"PassFlags" = "OnlyDirectional"
	//			}
	//			CGPROGRAM
	//			#pragma vertex vert
	//			#pragma fragment frag
	//			#pragma multi_compile_fwdbase
	//			#pragma require 2darray

	//			#include "UnityCG.cginc"
	//			#include "Lighting.cginc"
	//			#include "AutoLight.cginc"

	//			struct appdata
	//			{
	//				float4 vertex : POSITION;
	//				float3 uv : TEXCOORD0;
	//				float3 normal : NORMAL;
	//			};

	//			struct v2f
	//			{
	//				float4 pos : SV_POSITION;
	//				float4 scrPos : CAMERA;
	//				float3 uv : TEXCOORD0;
	//				float2 screenSpace : TEXCOORD2;
	//				float3 worldNormal : NORMAL;
	//				float3 viewDir : TEXCOORD1;

	//				SHADOW_COORDS(2)
	//			};

	//			UNITY_DECLARE_TEX2DARRAY(_MainTex);
	//			float4 _MainTex_ST;
	//			float4 _AmbientColor;
	//			float _SliceRange;

	//			v2f vert(appdata v)
	//			{

	//				v2f o;

	//				o.pos = UnityObjectToClipPos(v.vertex);
	//				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
	//				o.uv.z = (o.pos.z + 0.5) * _SliceRange;
	//				o.screenSpace = ComputeScreenPos(v.vertex);
	//				o.worldNormal = UnityObjectToWorldNormal(v.normal);
	//				o.viewDir = WorldSpaceViewDir(v.vertex);
	//				o.scrPos = ComputeScreenPos(o.pos);
	//				TRANSFER_SHADOW(o);
	//				return o;
	//			}

	//			float4 _Color;
	//			//fixed4 _ColorA, _ColorB;
	//			float _Slide;


	//			float4  frag(v2f i, UNITY_VPOS_TYPE vpos : SV_POSITION) : SV_Target
	//			{



	//				float3 normal = normalize(i.worldNormal);

	//				float NdotL = dot(_WorldSpaceLightPos0, normal);
	//				float lightintensity;
	//				float4 sample = UNITY_SAMPLE_TEX2DARRAY(_MainTex, i.uv);



	//				//SRT
	//				//float4 cameraPos = mul(unity_CameraProjection, i.pos);
	//				float3 screenPosition; screenPosition.xy = (i.scrPos.xy / i.scrPos.w);
	//				screenPosition.z = i.uv.z;
	//				//screenPosition.x -= 0.5;
	//				//screenPosition.y -= 0.5;
	//				float4 cameraTexture = UNITY_SAMPLE_TEX2DARRAY(_MainTex, screenPosition);


	//				//float t = length(i.uv - float2(0.5, 0.5)) * 1.41421356237; // 1.141... = sqrt(2)
	//				//cameraTexture *= lerp(_ColorA, _ColorB, t + (_Slide - 0.5) * 2);


	//				//float4 cameraTexture = tex2D(_MainTex, vpos.xy / _ScreenParams.xy);

	//				/*cameraTexture.x = cameraTexture.x % 1;
	//				cameraTexture.y = cameraTexture.y % 1;
	//				cameraTexture = tex2D(_MainTex, cameraTexture.xy);*/

	//				if (NdotL > 0)
	//				{
	//					if (NdotL > 0.5)
	//					{
	//						if (NdotL > .85)
	//						{
	//							lightintensity = 1;
	//						}
	//						else
	//						{
	//							lightintensity = .96;
	//						}
	//					}
	//					else
	//					{
	//						if (NdotL > .35)
	//						{
	//							lightintensity = 0.75;
	//						}
	//						else
	//						{
	//							lightintensity = 0.720;
	//						}

	//					}

	//				}
	//				else
	//				{
	//					if (NdotL < -0.5)
	//					{
	//						if (NdotL < -.85)
	//						{
	//							lightintensity = .211;//0.5;
	//						}
	//						else
	//						{
	//							lightintensity = .25; //.55;
	//						}
	//					}
	//					else
	//					{
	//						if (NdotL < -.35)
	//						{
	//							lightintensity = 0.5;//0.625-0.05;
	//						}
	//						else
	//						{
	//							lightintensity = .466;//0.625;
	//						}
	//					}

	//				}
	//				//return cameraTexture;
	//				return _Color * cameraTexture *lightintensity;
	//			}
	//			ENDCG
	//		}
	//		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	//	}
}
