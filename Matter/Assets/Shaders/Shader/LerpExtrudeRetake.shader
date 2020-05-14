// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "LerpExtrudeRetake"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[NoScaleOffset]_MainTextExtrude("MainText Extrude", 2D) = "white" {}
		[NoScaleOffset]_Noise1Text("Noise 1 Text", 2D) = "white" {}
		_depthDistance("depthDistance", Float) = 0.1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[NoScaleOffset]_Noise2Text("Noise 2 Text", 2D) = "white" {}
		[HDR]_ColorAlbedo("Color Albedo", Color) = (0.3672215,0.3499151,0.303131,1)
		_AnimatedTextAlbedo("Animated Text Albedo", Range( 0 , 1)) = 1
		_DissolveExtrude("Dissolve Extrude", Float) = -0.49
		_SizeOutline("Size Outline", Float) = 22.8
		_NoiseScale("Noise Scale", Float) = 0.041
		_AlphaNoise("Alpha Noise", Float) = 2.18
		_AlphaExtrude("Alpha Extrude", Float) = 0.64
		[HDR]_Color1("Color 1", Color) = (1.059274,0.8152525,0.8152525,1)
		[HDR]_Color2("Color 2", Color) = (1.924528,0.8045684,0.04538981,1)
		[HDR]_Color3("Color 3", Color) = (1.843137,0.05490196,0,1)
		[HDR]_ColorOutine("Color Outine", Color) = (6.368628,0.4334669,0,0)
		_Origin("Origin", Float) = 0
		_Spread("Spread", Range( 0 , 10)) = 0
		_AlphaDarkenRedColor("Alpha Darken Red Color", Float) = 0
		_AlphaDarkenYellowColor("Alpha Darken Yellow Color", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPosition87;
		};

		uniform float4 _Color3;
		uniform float _AlphaDarkenRedColor;
		uniform float4 _Color2;
		uniform float _AlphaDarkenYellowColor;
		uniform float _Origin;
		uniform float _Spread;
		uniform float4 _Color1;
		uniform sampler2D _MainTextExtrude;
		uniform float4 _ColorAlbedo;
		uniform float _AnimatedTextAlbedo;
		uniform float _DissolveExtrude;
		uniform float4 _ColorOutine;
		uniform float _SizeOutline;
		uniform sampler2D _Noise2Text;
		uniform sampler2D _Noise1Text;
		uniform float _NoiseScale;
		uniform float _AlphaNoise;
		uniform float _AlphaExtrude;
		uniform float _Metallic;
		uniform float _Smoothness;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _depthDistance;
		uniform float _Cutoff = 0.5;


		float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }

		float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }

		float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }

		float snoise( float3 v )
		{
			const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
			float3 i = floor( v + dot( v, C.yyy ) );
			float3 x0 = v - i + dot( i, C.xxx );
			float3 g = step( x0.yzx, x0.xyz );
			float3 l = 1.0 - g;
			float3 i1 = min( g.xyz, l.zxy );
			float3 i2 = max( g.xyz, l.zxy );
			float3 x1 = x0 - i1 + C.xxx;
			float3 x2 = x0 - i2 + C.yyy;
			float3 x3 = x0 - 0.5;
			i = mod3D289( i);
			float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
			float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
			float4 x_ = floor( j / 7.0 );
			float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
			float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
			float4 h = 1.0 - abs( x ) - abs( y );
			float4 b0 = float4( x.xy, y.xy );
			float4 b1 = float4( x.zw, y.zw );
			float4 s0 = floor( b0 ) * 2.0 + 1.0;
			float4 s1 = floor( b1 ) * 2.0 + 1.0;
			float4 sh = -step( h, 0.0 );
			float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
			float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
			float3 g0 = float3( a0.xy, h.x );
			float3 g1 = float3( a0.zw, h.y );
			float3 g2 = float3( a1.xy, h.z );
			float3 g3 = float3( a1.zw, h.w );
			float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
			g0 *= norm.x;
			g1 *= norm.y;
			g2 *= norm.z;
			g3 *= norm.w;
			float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
			m = m* m;
			m = m* m;
			float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
			return 42.0 * dot( m, px);
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos87 = ase_vertex3Pos;
			float4 ase_screenPos87 = ComputeScreenPos( UnityObjectToClipPos( vertexPos87 ) );
			o.screenPosition87 = ase_screenPos87;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 blendOpSrc30 = _Color3;
			float4 blendOpDest30 = float4( 1,1,1,0 );
			float4 lerpBlendMode30 = lerp(blendOpDest30,( blendOpSrc30 * blendOpDest30 ),_AlphaDarkenRedColor);
			float4 blendOpSrc32 = _Color2;
			float4 blendOpDest32 = float4( 1,1,1,0 );
			float4 lerpBlendMode32 = lerp(blendOpDest32,( blendOpSrc32 * blendOpDest32 ),_AlphaDarkenYellowColor);
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 temp_output_9_0 = ase_vertex3Pos;
			float clampResult16 = clamp( _Spread , 0.1 , 10.0 );
			float clampResult4 = clamp( ( ( temp_output_9_0.y - _Origin ) / clampResult16 ) , 0.0 , 1.0 );
			float4 lerpResult3 = lerp( ( saturate( lerpBlendMode30 )) , lerpBlendMode32 , clampResult4);
			float clampResult22 = clamp( clampResult4 , 0.25 , 1.0 );
			float4 lerpResult21 = lerp( lerpResult3 , _Color1 , clampResult22);
			float2 uv_MainTextExtrude38 = i.uv_texcoord;
			float4 lerpResult25 = lerp( lerpResult21 , ( tex2D( _MainTextExtrude, uv_MainTextExtrude38 ) * _ColorAlbedo ) , saturate( _AnimatedTextAlbedo ));
			float temp_output_62_0 = step( ase_vertex3Pos.y , _DissolveExtrude );
			half3 gammaToLinear34 = ( lerpResult25 * temp_output_62_0 ).rgb;
			gammaToLinear34 = half3( GammaToLinearSpaceExact(gammaToLinear34.r), GammaToLinearSpaceExact(gammaToLinear34.g), GammaToLinearSpaceExact(gammaToLinear34.b) );
			o.Albedo = gammaToLinear34;
			float smoothstepResult58 = smoothstep( _DissolveExtrude , ( (0.0 + (_SizeOutline - 0.0) * (-0.4 - 0.0) / (1.0 - 0.0)) + _DissolveExtrude ) , ase_vertex3Pos.y);
			float temp_output_60_0 = ( 1.0 - smoothstepResult58 );
			float2 uv_Noise2Text49 = i.uv_texcoord;
			float simplePerlin3D56 = snoise( float3( i.uv_texcoord ,  0.0 )*( _NoiseScale * 1 ) );
			simplePerlin3D56 = simplePerlin3D56*0.5 + 0.5;
			float blendOpSrc64 = temp_output_60_0;
			float blendOpDest64 = ( ( 1.0 - ( tex2D( _Noise2Text, uv_Noise2Text49 ).b - tex2D( _Noise1Text, i.uv_texcoord ).b ) ) * (-25.0 + (simplePerlin3D56 - -1.0) * (10.0 - -25.0) / (1.0 - -1.0)) );
			float lerpBlendMode64 = lerp(blendOpDest64,abs( blendOpSrc64 - blendOpDest64 ),_AlphaNoise);
			float blendOpSrc68 = -temp_output_62_0;
			float blendOpDest68 = ( saturate( lerpBlendMode64 ));
			float lerpBlendMode68 = lerp(blendOpDest68,(( blendOpDest68 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest68 ) * ( 1.0 - blendOpSrc68 ) ) : ( 2.0 * blendOpDest68 * blendOpSrc68 ) ),_AlphaExtrude);
			float temp_output_69_0 = (-8.4 + (lerpBlendMode68 - -1.0) * (5.47 - -8.4) / (1.0 - -1.0));
			float4 temp_cast_2 = (0.0).xxxx;
			float4 temp_cast_3 = (4.0).xxxx;
			float4 clampResult77 = clamp( ( lerpResult25 * _ColorOutine * temp_output_60_0 * temp_output_69_0 ) , temp_cast_2 , temp_cast_3 );
			half3 gammaToLinear75 = clampResult77.rgb;
			gammaToLinear75 = half3( GammaToLinearSpaceExact(gammaToLinear75.r), GammaToLinearSpaceExact(gammaToLinear75.g), GammaToLinearSpaceExact(gammaToLinear75.b) );
			o.Emission = gammaToLinear75;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float4 ase_screenPos87 = i.screenPosition87;
			float4 ase_screenPosNorm87 = ase_screenPos87 / ase_screenPos87.w;
			ase_screenPosNorm87.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm87.z : ase_screenPosNorm87.z * 0.5 + 0.5;
			float screenDepth87 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm87.xy ));
			float distanceDepth87 = abs( ( screenDepth87 - LinearEyeDepth( ase_screenPosNorm87.z ) ) / ( _depthDistance ) );
			float clampResult89 = clamp( distanceDepth87 , 0.0 , 1.0 );
			o.Alpha = clampResult89;
			clip( temp_output_69_0 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.screenPosition87;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.screenPosition87 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;0;1536;803;1157.66;-325.2638;2.95444;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;6;-2600.647,149.9695;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-1478.253,3360.655;Inherit;False;Property;_NoiseScale;Noise Scale;12;0;Create;True;0;0;False;0;0.041;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;43;-2241.683,2589.051;Inherit;True;Property;_Noise2Text;Noise 2 Text;7;1;[NoScaleOffset];Create;True;0;0;False;0;7c37fe289ea7e1141a3324b4f62e1df9;dc2ef1c1daf6ed745a7813ef508a8257;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1475.585,1975.556;Inherit;False;Property;_SizeOutline;Size Outline;11;0;Create;True;0;0;False;0;22.8;22.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;44;-2217.99,2857.326;Inherit;True;Property;_Noise1Text;Noise 1 Text;2;1;[NoScaleOffset];Create;True;0;0;False;0;dc2ef1c1daf6ed745a7813ef508a8257;7c37fe289ea7e1141a3324b4f62e1df9;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;46;-2201.373,3436.203;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;9;-2312.647,150.9695;Inherit;True;Object;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;14;-1985.416,475.3562;Inherit;False;Property;_Origin;Origin;19;0;Create;True;0;0;False;0;0;-3.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;12;-1960.893,198.0192;Inherit;True;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;49;-1920.217,2811.355;Inherit;True;Property;_Noise2;Noise 2;9;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-1192.5,2667.108;Inherit;False;Property;_DissolveExtrude;Dissolve Extrude;10;0;Create;True;0;0;False;0;-0.49;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1876.335,753.8834;Inherit;False;Property;_Spread;Spread;20;0;Create;True;0;0;False;0;0;5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-1229.254,1978.776;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;47;-1921.848,3095.782;Inherit;True;Property;_Noise;Noise;9;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;50;-1655.899,2335.868;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1743.024,864.2184;Inherit;False;Constant;_min;min;4;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;52;-1253.853,3366.755;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1736.648,953.4821;Inherit;False;Constant;_max;max;4;0;Create;True;0;0;False;0;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;-1506.068,323.5493;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1301.456,-260.7968;Inherit;False;Property;_AlphaDarkenRedColor;Alpha Darken Red Color;21;0;Create;True;0;0;False;0;0;3.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;55;-1320.5,2381.108;Inherit;True;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.NoiseGeneratorNode;56;-1077.737,3156.917;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;53;-1502.038,2959.676;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;20;-1294.251,-454.0473;Inherit;False;Property;_Color3;Color 3;17;1;[HDR];Create;True;0;0;False;0;1.843137,0.05490196,0,1;0.8021093,0.8586389,0.9716981,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;16;-1417.847,733.5099;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-931.404,2054.446;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-462.0799,2081.42;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;19;-1104.482,395.17;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-963.6284,89.6402;Inherit;True;Property;_AlphaDarkenYellowColor;Alpha Darken Yellow Color;22;0;Create;True;0;0;False;0;0;3.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-949.9297,-123.8928;Inherit;False;Property;_Color2;Color 2;16;1;[HDR];Create;True;0;0;False;0;1.924528,0.8045684,0.04538981,1;0.8216463,1.584294,3.706149,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;57;-726.5207,3162.298;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-25;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;59;-631.8663,3033.41;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;30;-866.6078,-394.1241;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-281.0988,3171.496;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;35;-385.1431,-129.64;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;4;-543,373;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;32;-654.2469,67.10247;Inherit;True;Multiply;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;62;-675.0698,2509.028;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;60;-109.5284,2070.708;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-148.5228,2996.003;Inherit;False;Property;_AlphaNoise;Alpha Noise;13;0;Create;True;0;0;False;0;2.18;9.21;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;39;277.5171,914.5133;Inherit;True;Property;_MainTextExtrude;MainText Extrude;1;1;[NoScaleOffset];Create;True;0;0;False;0;06af96540a2e1804b9ad80ac8f192fcc;62be4d3b0c28ee741a76ee265799e4d9;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;64;216.9657,2930.264;Inherit;True;Difference;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;660.7628,1163.412;Inherit;False;Property;_ColorAlbedo;Color Albedo;8;1;[HDR];Create;True;0;0;False;0;0.3672215,0.3499151,0.303131,1;0.4216803,0.6189672,0.8679245,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;22;56.09569,476.749;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;65;-171.7443,2589.047;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;686.7648,1459.045;Inherit;False;Property;_AnimatedTextAlbedo;Animated Text Albedo;9;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;609.3279,3040.736;Inherit;False;Property;_AlphaExtrude;Alpha Extrude;14;0;Create;True;0;0;False;0;0.64;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-216.7999,126.5;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;614.1618,938.0483;Inherit;True;Property;_Extrude;Extrude;9;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;84.41675,239.5397;Inherit;False;Property;_Color1;Color 1;15;1;[HDR];Create;True;0;0;False;0;1.059274,0.8152525,0.8152525,1;1.414214,1.414214,1.414214,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;1075.377,1114.288;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;477.2826,197.6365;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;37;1025.067,1436.899;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;68;860.7137,2839.72;Inherit;True;Overlay;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;1397.791,997.4641;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;71;1256.809,1652.021;Inherit;False;Property;_ColorOutine;Color Outine;18;1;[HDR];Create;True;0;0;False;0;6.368628,0.4334669,0,0;5.036642,2.350433,7.125916,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;67;-257.8333,2476.183;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;69;1337.191,2803.15;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-8.4;False;4;FLOAT;5.47;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;1156.044,3706.935;Inherit;False;Property;_depthDistance;depthDistance;3;0;Create;True;0;0;False;0;0.1;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;85;1084.044,3532.553;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;70;520.8772,1969.893;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;1752.73,1663.579;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;1843.049,1925.579;Inherit;False;Constant;_minLinearHDR;min Linear HDR;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;1885.441,2050.636;Inherit;False;Constant;_maxLinearHDR;max Linear HDR;19;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;77;2110.124,1732.689;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;2,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;1766.897,1019.421;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;87;1369.712,3621.31;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;1029.32,3297.577;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;88;1689.668,3492.027;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;75;2358.122,1728.448;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;86;1337.327,3289.902;Inherit;True;Property;_TEXT_Noise_Water;TEXT_Noise_Water;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;80;2291.469,1450.266;Inherit;True;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GammaToLinearNode;34;2241.36,999.864;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;89;1890.988,3506.204;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;2295.048,1293.77;Inherit;False;Property;_Metallic;Metallic;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;82;791.2891,3336.544;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.25;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3254.32,2224.138;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;LerpExtrudeRetake;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;6;0
WireConnection;12;0;9;2
WireConnection;49;0;43;0
WireConnection;51;0;45;0
WireConnection;47;0;44;0
WireConnection;47;1;46;0
WireConnection;52;0;42;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;55;0;50;2
WireConnection;56;0;46;0
WireConnection;56;1;52;0
WireConnection;53;0;49;3
WireConnection;53;1;47;3
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;16;2;18;0
WireConnection;54;0;51;0
WireConnection;54;1;48;0
WireConnection;58;0;55;0
WireConnection;58;1;48;0
WireConnection;58;2;54;0
WireConnection;19;0;13;0
WireConnection;19;1;16;0
WireConnection;57;0;56;0
WireConnection;59;0;53;0
WireConnection;30;0;20;0
WireConnection;30;2;31;0
WireConnection;61;0;59;0
WireConnection;61;1;57;0
WireConnection;35;0;30;0
WireConnection;4;0;19;0
WireConnection;32;0;2;0
WireConnection;32;2;33;0
WireConnection;62;0;55;0
WireConnection;62;1;48;0
WireConnection;60;0;58;0
WireConnection;64;0;60;0
WireConnection;64;1;61;0
WireConnection;64;2;63;0
WireConnection;22;0;4;0
WireConnection;65;0;62;0
WireConnection;3;0;35;0
WireConnection;3;1;32;0
WireConnection;3;2;4;0
WireConnection;38;0;39;0
WireConnection;92;0;38;0
WireConnection;92;1;23;0
WireConnection;21;0;3;0
WireConnection;21;1;1;0
WireConnection;21;2;22;0
WireConnection;37;0;36;0
WireConnection;68;0;65;0
WireConnection;68;1;64;0
WireConnection;68;2;66;0
WireConnection;25;0;21;0
WireConnection;25;1;92;0
WireConnection;25;2;37;0
WireConnection;67;0;62;0
WireConnection;69;0;68;0
WireConnection;70;0;67;0
WireConnection;73;0;25;0
WireConnection;73;1;71;0
WireConnection;73;2;60;0
WireConnection;73;3;69;0
WireConnection;77;0;73;0
WireConnection;77;1;79;0
WireConnection;77;2;78;0
WireConnection;74;0;25;0
WireConnection;74;1;70;0
WireConnection;87;1;85;0
WireConnection;87;0;84;0
WireConnection;83;1;82;0
WireConnection;88;0;86;1
WireConnection;88;1;87;0
WireConnection;75;0;77;0
WireConnection;86;1;83;0
WireConnection;34;0;74;0
WireConnection;89;0;87;0
WireConnection;0;0;34;0
WireConnection;0;2;75;0
WireConnection;0;3;81;0
WireConnection;0;4;80;0
WireConnection;0;9;89;0
WireConnection;0;10;69;0
ASEEND*/
//CHKSM=52B08B8ADBA5D721B82EE78CF396CB740396DB81