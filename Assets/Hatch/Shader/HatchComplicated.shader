Shader "NPR Hatch Effect/Hatch Complicated" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_Hatch0Tex ("Hatch 0 Tex", 2D) = "white" {}
		_Hatch1Tex ("Hatch 1 Tex", 2D) = "white" {}
		_Hatch2Tex ("Hatch 2 Tex", 2D) = "white" {}
		_Hatch3Tex ("Hatch 3 Tex", 2D) = "white" {}
		_Hatch4Tex ("Hatch 4 Tex", 2D) = "white" {}
		_Hatch5Tex ("Hatch 5 Tex", 2D) = "white" {}
		_Intensity ("Hatch Intensity", Range(0, 1)) = 1
		_RimPower ("Rim Power", Range(0.1, 16)) = 8
		_Shininess ("Shininess", Range(1, 256)) = 128
		_HatchColor ("Hatch Color", Color) = (0, 0, 0, 0)
		_OutlineColor ("Outline Color", Color) = (0, 0, 0, 0)
		_OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.005
	}
	SubShader {
		Pass {
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			Cull Back
			
			CGPROGRAM
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdbase
			#pragma multi_compile _ NHE_INVERSE_RIM
			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _Hatch0Tex;
			uniform sampler2D _Hatch1Tex;
			uniform sampler2D _Hatch2Tex;
			uniform sampler2D _Hatch3Tex;
			uniform sampler2D _Hatch4Tex;
			uniform sampler2D _Hatch5Tex;
			uniform float _Intensity;
			uniform float _RimPower;
			uniform float _Shininess;
			uniform float4 _HatchColor;
		
			struct v2f
			{
				float4 pos  : SV_POSITION;
				float4 tex  : TEXCOORD0;   // .xy is hatch uv, .zw is base uv
				float3 norm : TEXCOORD1;
				float3 lit  : TEXCOORD2;
				float3 view : TEXCOORD3;
				LIGHTING_COORDS(4, 5)
			};
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = float4(v.texcoord.xy, TRANSFORM_TEX(v.texcoord, _MainTex).xy);
				o.norm = mul((float3x3)_Object2World, SCALED_NORMAL);
				o.lit = WorldSpaceLightDir(v.vertex);
				o.view = WorldSpaceViewDir(v.vertex);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			float4 frag (v2f i) : SV_TARGET
			{
				float3 N = normalize(i.norm);
				float3 L = normalize(i.lit);
				float3 V = normalize(i.view);
				float3 H = normalize(L + V);
				
				float ndl = saturate(dot(N, L));
				float rim = saturate(dot(N, V));
				float ndh = saturate(dot(N, H));
				rim = pow(rim, _RimPower);
#if NHE_INVERSE_RIM
				rim = 1.0 - rim;
#endif
				float spec = pow(ndh, _Shininess);
				float shading = (ndl + rim + spec) * _LightColor0;

				float4 c;
                float step = 1.0 / 6.0;
				float2 uvHatch = i.tex.xy;
				
				float4 hatch5Color = tex2D(_Hatch5Tex, uvHatch);
				float4 hatch4Color = tex2D(_Hatch4Tex, uvHatch);
				float4 hatch3Color = tex2D(_Hatch3Tex, uvHatch);
				float4 hatch2Color = tex2D(_Hatch2Tex, uvHatch);
				float4 hatch1Color = tex2D(_Hatch1Tex, uvHatch);
				float4 hatch0Color = tex2D(_Hatch0Tex, uvHatch);
				
                if (shading <= step)
				{
					c = lerp(hatch5Color, hatch4Color, 6.0 * shading);
				}
				if (shading > step && shading <= 2.0 * step)
				{
					c = lerp(hatch4Color, hatch3Color, 6.0 * (shading - step));
				}
				if (shading > 2.0 * step && shading <= 3.0 * step)
				{
					c = lerp(hatch3Color, hatch2Color, 6.0 * (shading - 2.0 * step));
				}
				if (shading > 3.0 * step && shading <= 4.0 * step)
				{
					c = lerp(hatch2Color, hatch1Color, 6.0 * (shading - 3.0 * step));
				}
				if (shading > 4.0 * step && shading <= 5.0 * step)
				{
					c = lerp(hatch1Color, hatch0Color, 6.0 * (shading - 4.0 * step));
				}
				if (shading > 5.0 * step)
				{
					c = lerp(hatch0Color, 1.0, 6.0 * (shading - 5.0 * step));
				}
				float4 hatchColor = lerp(_HatchColor, 1.0, c.r);
				float4 baseColor = tex2D(_MainTex, i.tex.zw) * _Intensity;
				return hatchColor * baseColor * LIGHT_ATTENUATION(i);
			}
			ENDCG
		}
		Pass {
			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			Cull Front

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			uniform float4 _OutlineColor;
			uniform float _OutlineWidth;
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			v2f vert (appdata_base v)
			{
				v2f o;
				float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				norm = normalize(norm);
				float2 offset = TransformViewToProjection(norm.xy);
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.pos.xy += offset * o.pos.z * _OutlineWidth;
				return o;
			}
			float4 frag (v2f i) : SV_TARGET
			{
				return _OutlineColor;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
