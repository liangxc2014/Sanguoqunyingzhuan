Shader "Custom/ClipSimple" {
	Properties {
		_Color ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Left ("Left ", Float) = 0.1
		_Right("Right", Float) = 0.9
		_Bottom("Bottom", Float) = 0.1
		_Top ("Top ", Float) = 0.9
	}
	SubShader 
	{
		LOD 100

		Tags 
		{ 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }

        Cull Off 
        Lighting Off 
        ZWrite Off 
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass 
		{
        	CGPROGRAM
        	#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			fixed4 _Color;
			sampler2D _MainTex;	
			uniform float _Left;
			uniform float _Right;
			uniform float _Top;
			uniform float _Bottom;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

            fixed4 frag (v2f i) : COLOR
            {
				float4 c = tex2D (_MainTex, i.texcoord.xy) * i.color * _Color;

				if (i.texcoord.x >= _Left && i.texcoord.x <= _Right && i.texcoord.y <= _Top && i.texcoord.y >= _Bottom)	 {
					return float4 (0,0,0,0);
				} else {
					return c;
				}
			}
			ENDCG
        }
    }

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
