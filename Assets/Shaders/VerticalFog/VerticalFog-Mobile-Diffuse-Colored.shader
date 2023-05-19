Shader "Environment/Vertical Fog Diffuse Colored" 
{
	Properties 
	{
		_Color ("Tint Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MinFogValue ("Min Fog Value", Range(0, 1)) = 0.2
		_FogStart("Fog Start", Float) = 5.0
		_FogEnd("Fog End", Float) = 35.0
		[Toggle] _UseWorldPos("Use World Pos", Float) = 0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert finalcolor:finalcolor vertex:vert

		sampler2D _MainTex;
		float4 _Color;
		float _MinFogValue;
		float _FogStart;
		float _FogEnd;
		half _UseWorldPos;

		uniform half4 unity_FogStart;
		uniform half4 unity_FogEnd;

		struct Input 
		{
			float2 uv_MainTex;
			float4 pos;
			half fog;
		};

		void vert(inout appdata_full v, out Input o)
		{
			float4 hpos = UnityObjectToClipPos(v.vertex);
			o.pos = mul(unity_ObjectToWorld, v.vertex);
			o.uv_MainTex = v.texcoord.xy;

			float pos = length(UnityObjectToViewPos(v.vertex).xyz);
			float diff = unity_FogEnd.z - unity_FogStart.x;
			float invDiff = 1.0f / diff;
			o.fog = clamp(1.0 - (unity_FogEnd.z - pos) * invDiff, 0.0, 1.0);
			o.fog = max(o.fog, _MinFogValue);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a;
		}

		void finalcolor(Input IN, SurfaceOutput o, inout fixed4 color)
		{
			#ifndef UNITY_PASS_FORWARDADD
				float distance = _UseWorldPos > 0.5 ? IN.pos.y : IN.pos.y - _WorldSpaceCameraPos.y;
				float lerpValue = clamp((distance - _FogStart) / (_FogEnd - _FogStart), 0, 1);
				lerpValue = max(lerpValue, IN.fog);
				color.rgb = lerp(color.rgb, unity_FogColor.rgb, lerpValue);
			#endif
		}
		ENDCG
	}

	Fallback "Mobile/VertexLit"
}
