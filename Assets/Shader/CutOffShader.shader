Shader "PokeShaders/CutOffShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TransitionTex ("Transition Texture", 2D) = "white" {}
		_Cutoff ("CutOff", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 0
		_Color("Screen Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _Cutoff;
			float _Fade;
			fixed4 _Color;
			sampler2D _TransitionTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 transit = tex2D(_TransitionTex, i.uv);
				fixed4 col = _Color;
				if (transit.b < _Cutoff){
					return col = lerp(col, _Color, _Fade);
				}
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}
