Shader "Custom/ClipGradient" {
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
				fixed4 c = tex2D (_MainTex, i.texcoord.xy) * i.color * _Color;
				fixed4 temp = fixed4 (0,0,0,0);

				if (i.texcoord.x >= _Left && i.texcoord.x <= _Right && i.texcoord.y <= _Top && i.texcoord.y >= _Bottom)	 
				{
					return temp;
				}
				else 
				{
					float x = 0;
					float y = 0;
					float angle30 = 3.14159 / 24;
					float width = _Right - _Left;
					float yOffset = width * tan(angle30);
					float value = width / yOffset;
					
					if (i.texcoord.x >= _Left && i.texcoord.x <= _Right)
					{
						if (i.texcoord.y > _Top && i.texcoord.y <= _Top + yOffset)
						{
							x = _Right - i.texcoord.x;
							y = i.texcoord.y - _Top;
							if (x / y > value)
							{
								c = temp;
							}
						}
						else if (i.texcoord.y < _Bottom && i.texcoord.y >= _Bottom - yOffset)
						{	
							x = i.texcoord.x - _Left;
							y = _Bottom - i.texcoord.y;
							if (x / y > value)
							{
								c = temp;
							}
						}
					}
					
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
