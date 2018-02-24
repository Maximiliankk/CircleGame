Shader "Custom/SwirlShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius ("Radius", Float) = 1
		_Angle("Angle", Float) = 1
		_Center("Center", Vector) = (.5, .5, 0, 0)
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
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Radius;
			float _Angle;
			float4 _Center;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv - _Center.xy;
				float len = length(uv * float2(_ScreenParams.x / _ScreenParams.y, 1));
				float angle = atan2(uv.y, uv.x) + _Angle * smoothstep(_Radius, 0, len);
				float radius = length(uv);

				fixed4 col = tex2D(_MainTex, float2(radius * cos(angle), radius * sin(angle)) + _Center.xy);
				return col;
			}
			ENDCG
		}
	}
}
