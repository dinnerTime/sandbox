Shader "Custom/Surface Shader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf NoLighting

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			half4 color : COLOR;
		};

		fixed4 LightingNoLighting(SurfaceOutput IN, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = IN.Albedo;
			c.a = IN.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * IN.color.rgb;
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}