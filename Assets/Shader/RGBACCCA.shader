// lays a texture with RGB to custom color mapping over a normal texture
Shader "Sprites/RGBACCCA"
{
	Properties
	{
		[PerRendererData] _ColorR ("R Color", Color) = (1,1,1,1)
		[PerRendererData] _ColorG ("G Color", Color) = (1,1,1,1)
		[PerRendererData] _ColorB ("B Color", Color) = (1,1,1,1)
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Over", 2D) = "white" {}
	}

	SubShader
	{
		Tags 
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		 }
		
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
			 
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 pos : POSITION;
					float2 texcoord : TEXCOORD0;
					float4 color : COLOR;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float4 color : COLOR;
				};

				fixed4 _ColorR;
				fixed4 _ColorG;
				fixed4 _ColorB;
				sampler2D _MainTex;
				sampler2D _MaskTex;
				fixed4 _MainTex_ST;
			
				v2f vert (appdata_t IN)
				{
					v2f OUT;
					OUT.pos = mul(UNITY_MATRIX_MVP, IN.pos);
					OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
					OUT.color = IN.color;
					return OUT;
				}
			
				fixed4 frag (v2f IN) : SV_Target
				{
					fixed4 col  = tex2D(_MainTex, IN.uv);
					fixed4 over = tex2D(_MaskTex, IN.uv);

					fixed4 ocol = fixed4(_ColorR.rgb * over.r +
							      		 _ColorG.rgb * over.g +
							      		 _ColorB.rgb * over.b, over.a);

					fixed3 B = col.rgb * col.a;
					fixed3 A = ocol.rgb * ocol.a;

					fixed3 color = A.rgb + B.rgb * (1 - ocol.a) * IN.color.rgb * IN.color.a;
					fixed alpha = ocol.a + col.a * (1 - ocol.a) * IN.color.a;
					return fixed4(color, alpha);
				}
			ENDCG
		}
	}
}
