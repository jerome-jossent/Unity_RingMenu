//Shader "GLSL shader using discard"
Shader "Custom/jj"
{
    SubShader{
       Pass {
          Cull Off // turn off triangle culling, alternatives are:
          // Cull Back (or nothing): cull only back faces 
          // Cull Front : cull only front faces

          GLSLPROGRAM

          varying vec4 position_in_object_coordinates;

          #ifdef VERTEX         

          void main()
          {
             position_in_object_coordinates = gl_Vertex;
             gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
          }

          #endif

          #ifdef FRAGMENT

          void main()
          {
             if (position_in_object_coordinates.y > 0.0)
             {
                discard; // drop the fragment if y coordinate > 0
             }
             if (gl_FrontFacing) // are we looking at a front face?
             {
                gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0); // yes: green
             }
             else
             {
                gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0); // no: red
             }
          }

          #endif

          ENDGLSL
       }
    }
}

//Shader "Custom/jj"
//{
//    Properties
//    {
//        _Color ("Color", Color) = (1,1,1,1)
//        _MainTex ("Albedo (RGB)", 2D) = "white" {}
//        _Glossiness ("Smoothness", Range(0,1)) = 0.5
//        _Metallic ("Metallic", Range(0,1)) = 0.0
//    }
//    SubShader
//    {
//        Tags { "RenderType"="Opaque" }
//        LOD 200
//
//        CGPROGRAM
//        // Physically based Standard lighting model, and enable shadows on all light types
//        #pragma surface surf Standard fullforwardshadows
//
//        // Use shader model 3.0 target, to get nicer looking lighting
//        #pragma target 3.0
//
//        sampler2D _MainTex;
//
//        struct Input
//        {
//            float2 uv_MainTex;
//        };
//
//        half _Glossiness;
//        half _Metallic;
//        fixed4 _Color;
//
//        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
//        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
//        // #pragma instancing_options assumeuniformscaling
//        UNITY_INSTANCING_BUFFER_START(Props)
//            // put more per-instance properties here
//        UNITY_INSTANCING_BUFFER_END(Props)
//
//        void surf (Input IN, inout SurfaceOutputStandard o)
//        {
//            // Albedo comes from a texture tinted by color
//            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
//            o.Albedo = c.rgb;
//            // Metallic and smoothness come from slider variables
//            o.Metallic = _Metallic;
//            o.Smoothness = _Glossiness;
//            o.Alpha = c.a;
//        }
//        ENDCG
//    }
//    FallBack "Diffuse"
//}
