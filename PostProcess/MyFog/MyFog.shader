Shader "Unlit/CUbe"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogStart("fogStart",float) = 0
		_FogEnd("fogEnd",float) = 1
		 
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD3;
				float fog2 : TEXCOORD2;
 			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _FogStart;
			float _FogEnd;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.fog2 = saturate((_FogStart - o.worldPos.y )/(_FogStart - _FogEnd));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.rgb = lerp(fixed3(0.5,0.1,0.6),col.rgb, i.fog2); 
				
				
				return col;
			}
			ENDCG
		}
	}
}

