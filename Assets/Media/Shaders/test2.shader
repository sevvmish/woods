
Shader "MY/test2" {
 
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        

    }
 
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}

        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Lambert vertex:vert addshadow

            
            sampler2D _MainTex;
            

                //Structs
                struct Input {
                    float2 uv_MainTex;
                };

                // Vertex Manipulation Function
                void vert (inout appdata_full i) {

                }

                // Surface Shader
                void surf (Input IN, inout SurfaceOutput o) {
                    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }

        ENDCG
        }
     
    Fallback "Diffuse"
} 
