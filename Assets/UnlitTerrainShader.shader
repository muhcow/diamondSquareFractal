Shader "Unlit/UnlitTerrainShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "blue" {}
        MaxHeight ("maximum height", Float) = 0.73723
        SnowHeight ("snow height", Float) = 0.90
        DirtHeight ("dirt height", Float) = 0.60
        GrassHeight ("grass height", Float) = 0.50
        HeightBelowOrigin ("height below origin", Float) = 0.50
       
       
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
            
            uniform float MaxHeight;
			uniform float SnowHeight;
			uniform float DirtHeight;
			uniform float GrassHeight;
            uniform float HeightBelowOrigin;
            
            

			struct vertIn
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 colour : COLOR;
				float2 distance : TEXCOORD0;
			};

			//vertex shader
			vertOut vert(vertIn v)
			{   
                //SnowHeight = (MaxHeight * 0.9) /*- HeightBelowOrigin */;
                //DirtHeight = (MaxHeight * 0.7) /*- HeightBelowOrigin */;
                //GrassHeight = (MaxHeight * 0.5)/* - HeightBelowOrigin */;
                
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// Colour each vertex based on height
				float4 col;
				if (v.vertex.y > SnowHeight) {
					// white
					col = float4(1, 1, 1, 1);
				}              
				if (SnowHeight > v.vertex.y > DirtHeight) {
					// brown
					col = float4(0.57254, 0.38431, 0.22353, 1);
				}
				if (DirtHeight > v.vertex.y > GrassHeight) {
					// green
					col = float4(0.09804, 0.54902, 0.09804, 1);
				}
				if (GrassHeight > v.vertex.y){
					// yellow
					col = float4(0.96863, 0.9451, 0.7451, 1);
				}
                
                
                

				o.colour = col;
				return o;
			}
			fixed4 frag(vertOut v) : SV_Target
			{
				float4 col = v.colour;
				return col;
			}
			ENDCG
		}
	}
}
