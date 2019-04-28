Shader "Custom/TranswarpUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BrightRampTex("Brightness Ramp", 2D) = "black" {}
		_AlphaTex("Alpha (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

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
				float2 auv : TEXCOORD1;
				float2 buv : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 auv : TEXCOORD1;
				float2 buv : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _AlphaTex;
			float4 _AlphaTex_ST;
			sampler2D _BrightRampTex;
			float4 _BrightRampTex_ST;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.auv = TRANSFORM_TEX(v.auv, _AlphaTex);
				o.buv = TRANSFORM_TEX(v.buv, _BrightRampTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 alpha = tex2D(_AlphaTex, i.auv);
				fixed4 brightnessRamp = tex2D(_BrightRampTex, i.buv);
				col.rgb += brightnessRamp.rgb;
				col.a = max(col.a, brightnessRamp.r);
				col.a *= alpha.rgb;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
