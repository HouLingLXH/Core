Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("speed",float) = 1
		_ToNum("toNum",float) = 1
        _Step("step",float) = 0.9 //每个数字的高度
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100


		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Speed;
			float _ToNum;
			float _Step;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex );
				//_Speed *= _ToNum + (_ToNum- v.uv.y );
				//v.uv += fixed2(0,_Time.y * _Speed );
				
				float aimUV_y = _ToNum * _Step; 

				v.uv.y += aimUV_y;
				o.uv = TRANSFORM_TEX(v.uv , _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}


		
	}
}
