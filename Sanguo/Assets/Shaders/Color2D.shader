Shader "Custom/Color2D" {
    Properties 
    {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
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
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
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
	
			uniform fixed4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
				
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
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
				fixed4 ret = fixed4(_Color.x, _Color.y, _Color.z, col.a);
				return ret;
			}
			ENDCG
		}
	}
	/*
	CGINCLUDE
    #include "UnityCG.cginc"
	sampler2D _MainTex;
	struct v2f_img_f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		fixed4 color: TEXCOORD1;	
	};
	v2f_img_f vert (appdata_full v) {
		v2f_img_f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
		o.color = v.color;
		return o;
	}
	float4 frag (v2f_img_f i) : COLOR {
		
	    fixed4 addcolor = tex2D(_MainTex, i.uv.xy) * i.color;
		fixed4 ret = fixed4(0,0,0,1);
		
	    if(addcolor.a > 0.8){
	    	ret = addcolor;
	    } else {
			float angle = 3.14159 / 12;
			float sum = 0;
			for (int n=0; n<12; n++)
			{
				fixed4 temp = tex2D(_MainTex, i.uv.xy + float2(0.02 * sin(n * angle), 0.02 * cos(n * angle)));
				sum += temp.a;
			}
			
			ret = float4(0, 1, 0, 1);
			ret.a = sum / 3;
		}

	    return ret;
		
	}
	ENDCG
	*/


    Fallback "Diffuse"
}
