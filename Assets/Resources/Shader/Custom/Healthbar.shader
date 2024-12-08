Shader "Custom/Healthbar" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_HealthRatio ("Health Ratio", Range(0, 1)) = 1
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
			float _HealthRatio;
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

			half3 HSLToRGB(half h, half s, half l) {
				half c = (1 - abs(2 * l - 1)) * s;
				half x = c * (1 - abs(fmod(h * 6, 2) - 1));
				half m = l - c / 2;
				half3 rgb;

				if (h < 1.0 / 6.0) rgb = half3(c, x, 0);
				else if (h < 2.0 / 6.0) rgb = half3(x, c, 0);
				else if (h < 3.0 / 6.0) rgb = half3(0, c, x);
				else if (h < 4.0 / 6.0) rgb = half3(0, x, c);
				else if (h < 5.0 / 6.0) rgb = half3(x, 0, c);
				else rgb = half3(c, 0, x);

				return rgb + m;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, i.uv);

				if (i.uv.x > _HealthRatio) {
					discard;
				}

				half healthHue = _HealthRatio / 3.0 - 0.07;
				half3 rgbColor = HSLToRGB(healthHue, 1.0, 0.5);

				texColor.rgb *= rgbColor;
				texColor *= _Color;

				return texColor;
			}

			ENDCG
		}
	}
}
