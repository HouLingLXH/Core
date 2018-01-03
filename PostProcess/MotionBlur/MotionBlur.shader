Shader "Unlit/MotionBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Blend("Blend",float) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "queue" = "Transparent" }
		LOD 100

		ZTest Always cull off ZWrite off


		CGINCLUDE
		
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
		float _Blend;

		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}
			
		fixed4 frag (v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
			col.a = _Blend;
			return col;
		}

		fixed4 frag2 (v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);

			return col;
		}

		ENDCG

		Pass
		{
			blend SrcAlpha OneMinusSrcAlpha
			ColorMask rgb
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			//利用透明度a进行混合
			ENDCG
		}

		Pass
		{
			blend one zero
			ColorMask a
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag2

			//本通道 为了 保护原有的a 值，不被_Blend覆盖掉
			ENDCG
		}
	}

	FallBack Off
}
