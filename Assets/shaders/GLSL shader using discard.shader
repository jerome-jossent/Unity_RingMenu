Shader "GLSL shader using discard" {
   SubShader {
      Pass {
         Cull Off // turn off triangle culling, alternatives are:
         // Cull Back (or nothing): cull only back faces 
         // Cull Front : cull only front faces
         
         GLSLPROGRAM
               
         varying vec4 position_in_object_coordinates;
 
         #ifdef VERTEX         
         
         void main()
         {
            position_in_object_coordinates= gl_Vertex;
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