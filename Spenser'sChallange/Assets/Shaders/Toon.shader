Shader "Toon"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_SpecularPower("Specular Power", Float) = 32
			[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
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
				float _SpecularPower;
				float4 _SpecularColor;
				float4 _AmbientColor;
				float4 _RimColor;
				float _RimAmount;
				float _RimThreshold;

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

							lightintensity = 0.75;

						}

					}
					else
					{
						if (NdotL < -0.5)
						{
							lightintensity = 0.5;
						}
						else
						{
							lightintensity = 0.625;
						}

					}
					float shadow = SHADOW_ATTENUATION(i);

					float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
					//smooths out divisions between colors
					//lightintensity = smoothstep(0, 0.1, NdotL);
					float light = lightintensity * _LightColor0;

					// In the fragment shader, above the existing lightIntensity declaration.

					float3 viewDir = normalize(i.viewDir);
					float4 rimDot = 1 - dot(viewDir, normal);

					float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
					rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
					//float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
					float4 rim = rimIntensity * _RimColor;

					float3 reflectedLight = normalize(_WorldSpaceLightPos0 + viewDir);
					//float3 reflectedLight = reflect(-posToLight, normal);
					//reflectedLight = normalize(reflectedLight);
					float dp = dot(normal, reflectedLight);
					//dp = max(dp, 0);
					//ambient += //1 * dot(normal, posToLight) * falloff;
					float specular = pow(dp * lightintensity, _SpecularPower * _SpecularPower);//* falloff;
					float specularIntensitySmooth = smoothstep(0.005, 0.01, specular);
					specular = specularIntensitySmooth * _SpecularColor;

					return _Color * sample * (_AmbientColor + light + specular + rim);
				}
				ENDCG
			}
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}
//Just rimDot could be good for a key item or frozen object