Shader "Spakment/Intersect/TestShaderTwoPass"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
        [HDR] _ColorTint("ColorTint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Geometry+10" }
        LOD 100
        ZWrite Off
 
        Pass
        {
            Cull Front
            ZTest Greater
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
 
 
            float4 _Color;
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
 
         
            ENDCG
        }
 
 
        Pass
        {
            Cull Back
            ZTest Greater
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
         
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
 
 
            float4 _ColorTint;
 
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                //discard;
                return _ColorTint;
            }
 
         
            ENDCG
        }
    }
}