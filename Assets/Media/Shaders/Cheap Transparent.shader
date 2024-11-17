Shader "Cheap Transparent"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" }
		Lighting Off
		BindChannels
		{
		    Bind "Color", color
		    Bind "Vertex", vertex
		    Bind "TexCoord", texcoord
		}

		Blend SrcAlpha OneMinusSrcAlpha
	
		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					Combine texture * primary
				}
			}
		}
	}
}