Shader "Custom/Backplate" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Base Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "Queue" = "Transparent-1" "Rendertype" = "Transparent" }
		
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				return tex2D(_MainTex, i.uv) * _Color;
			}

			ENDCG
		}
	}
}