Shader "NPR Sketch Effect/Sketch" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_Sketch0Tex ("Sketch 0 Tex", 2D) = "white" {}
		_Sketch1Tex ("Sketch 1 Tex", 2D) = "white" {}
		_Sketch2Tex ("Sketch 2 Tex", 2D) = "white" {}
		_Sketch3Tex ("Sketch 3 Tex", 2D) = "white" {}
		_Sketch4Tex ("Sketch 4 Tex", 2D) = "white" {}
		_Sketch5Tex ("Sketch 5 Tex", 2D) = "white" {}
		_Intensity ("Main Tex Intensity", Range(0.5, 2)) = 1
		_Tile ("Sketch Tile", Range(1, 10)) = 5
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
			#pragma vertex vert
			#pragma fragment frag

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _Sketch0Tex;
			uniform sampler2D _Sketch1Tex;
			uniform sampler2D _Sketch2Tex;
			uniform sampler2D _Sketch3Tex;
			uniform sampler2D _Sketch4Tex;
			uniform sampler2D _Sketch5Tex;
			uniform float _Intensity;
			uniform float _Tile;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				float4 scrpos : TEXCOORD1;
				float3 wldnor : TEXCOORD2;
				float3 wldlit : TEXCOORD3;
				LIGHTING_COORDS(4, 5)
			};
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tex = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.scrpos = ComputeScreenPos(o.pos);
				o.wldnor = mul((float3x3)_Object2World, v.normal);
				o.wldlit = WorldSpaceLightDir(v.vertex);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			float4 frag (v2f i) : SV_Target
			{
				float3 N = normalize(i.wldnor);
				float3 L = normalize(i.wldlit);
				float2 scrpos = i.scrpos.xy / i.scrpos.w * _Tile;
				float atten = LIGHT_ATTENUATION(i);

				float diff = (dot(N, L) * 0.5 + 0.5) * atten * 6.0;
				float3 c;
				if (diff < 1.0)
					c = tex2D(_Sketch5Tex, scrpos).rgb;
				else if (diff < 2.0)
					c = tex2D(_Sketch4Tex, scrpos).rgb;
				else if (diff < 3.0)
					c = tex2D(_Sketch3Tex, scrpos).rgb;
				else if (diff < 4.0)
					c = tex2D(_Sketch2Tex, scrpos).rgb;
				else if (diff < 5.0)
					c = tex2D(_Sketch1Tex, scrpos).rgb;
				else
					c = tex2D(_Sketch0Tex, scrpos).rgb;

				float4 albedo = tex2D(_MainTex, i.tex) * _Intensity;
				return float4(c * albedo.rgb, 1.0) * _LightColor0;
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
			float4 frag (v2f i) : SV_Target
			{
				return _OutlineColor;
            }
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
