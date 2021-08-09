Shader "Custom/HideMask"
{

    Properties{}

    SubShader{

        Tags { 
            "RenderType" = "Opaque" 
        }
        
        Pass{
            ZWrite Off
        }
    }
}