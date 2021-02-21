Shader "Wrapped/Specular Wrapped"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _LambertWrapAmount("Lambert Wrapping", Range (0.01, 1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        #pragma surface surf WrapLambert
        
        fixed _LambertWrapAmount;
        fixed4 LightingWrapLambert(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten) {
            //Diffuse term
            fixed NdotL = dot(s.Normal, lightDir) + _LambertWrapAmount;
            fixed diff = max(NdotL, 0) / (1.0 + _LambertWrapAmount);
            fixed3 diffColor = s.Albedo * _LightColor0.rgb;

            //Specular term
            half3 h = normalize(lightDir + viewDir);
            float NdotH = dot(s.Normal, h);
            float spec = pow(max(NdotH, 0), s.Specular * 128.0) * s.Gloss;
            fixed3 specColor = _SpecColor.rgb * _LightColor0.rgb;

            //Sum
            fixed4 c;
            c.rgb = ((diffColor * diff) + (specColor * spec)) * (atten * 2);
            c.a = s.Alpha + (_LightColor0.a * _SpecColor.a * spec * atten);
            return c;
        }
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        float4 _Color;
        sampler2D _MainTex;
        half _Shininess;
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            
            o.Albedo = tex.rgb * _Color.rgb;
            o.Gloss = tex.a;
            o.Alpha = tex.a * _Color.a;
            o.Specular = _Shininess;
        }
        ENDCG
    }
    FallBack "Specular"
}
