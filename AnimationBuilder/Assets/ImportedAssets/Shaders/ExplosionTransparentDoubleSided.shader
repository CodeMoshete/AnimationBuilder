Shader "Custom/Explosion Transparent (Double Sided)"
{
	Properties
	{
		_MainTex ("Color Ramp", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "black" {}
		_NoiseStrength("Noise Strength", Range(0,1)) = 0.1
		_RampRange("Ramp Range", Range(0.05, 1)) = 1
		_RampStart("Ramp Start", Range(-0.99, 0.99)) = 0.01
		_MasterAlpha("Master Alpha", Range(0,1)) = 1
		_ChannelFactor("ChannelFactor (r,g,b)", Vector) = (1,0,0)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Lighting Off
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			struct v2f
			{
				float2 ramp : TEXCOORD0;
				float2 disp : TEXCOORD1;
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
			};

			sampler2D _DisplacementTex;
			float4 _DisplacementTex_ST;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _RampRange;
			float _RampStart;
			float _NoiseStrength;
			float _MasterAlpha;
			float3 _ChannelFactor;
			
			v2f vert (appdata_full v)
			{
				float3 dcolor = tex2Dlod(_DisplacementTex, float4(v.texcoord.xy, 0, 0));
				float d = (dcolor.r*_ChannelFactor.r + dcolor.g*_ChannelFactor.g + dcolor.b*_ChannelFactor.b);
				v.vertex.xyz += v.normal * d * _NoiseStrength;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.normal * 0.5 + 0.5;
				o.ramp = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.disp = TRANSFORM_TEX(v.texcoord1, _DisplacementTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 dcolor = tex2D(_DisplacementTex, i.disp);
				float d = clamp((dcolor.r*_ChannelFactor.r + dcolor.g*_ChannelFactor.g + dcolor.b*_ChannelFactor.b) * _RampRange + _RampStart, 0.01, 0.99);

				float rampAmt = clamp(dcolor.r * _RampRange + _RampStart, 0.01, 0.99);
				fixed4 col = tex2D(_MainTex, float2(d,1)).rgba;
				col.a *= _MasterAlpha;
				return col;
			}
			ENDCG
		}
	}
}
