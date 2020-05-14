// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Thibaut/Vfx/Shader_DisfractionEnergy01"
{
	Properties
	{
		_AlphaSubstract("AlphaSubstract", Float) = 0.85
		_lerpSwitchAlpha("lerpSwitchAlpha", Float) = 0
		_NormalAlpha("Normal Alpha", Float) = 0.75
		_NormalScale("Normal Scale", Float) = 1
		_Cutoff( "Mask Clip Value", Float ) = 0.17
		_lerp("lerp", Float) = 0
		_MaskValue("Mask Value", Range( -3 , 3)) = 0
		_MaskBias("Mask Bias", Range( 0 , 1)) = 0
		_EdgeThickness("Edge Thickness", Range( 0 , 0.15)) = 0.1377845
		_DissolveAmount("DissolveAmount", Range( -0.1 , 1)) = 0.4235094
		[HDR]_ColorHDROutline("Color HDR Outline", Color) = (0,8.314797,15.1544,0)
		_luminosity("luminosity", Range( 1 , 10)) = 1
		_contrast("contrast", Range( 0 , 2)) = 1
		[NoScaleOffset]_MainText("MainText", 2D) = "white" {}
		[NoScaleOffset]_StarField("StarField", 2D) = "white" {}
		[NoScaleOffset][Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset]_AlphaOutline("AlphaOutline", 2D) = "white" {}
		[HideInInspector]_TextNoise("Text Noise", 2D) = "white" {}
		[NoScaleOffset]_TextMask2("TextMask2", 2D) = "white" {}
		[NoScaleOffset]_TextNoiseMask("TextNoiseMask", 2D) = "white" {}
		_OpacityLayerBackground("Opacity Layer Background", Range( 0 , 5)) = 1
		_OpacityLayerStar1("Opacity Layer Star 1", Range( 0 , 8)) = 2
		_OpacityLayerStar2("Opacity Layer Star 2", Range( 0 , 8)) = 2
		_RotationUV("Rotation UV", Range( 0 , 8.727)) = 0
		_SpeedBackground("Speed Background", Range( 0 , 0.1)) = 0.005
		_SpeedStar1("Speed Star 1", Range( 0 , 0.1)) = 0.01
		_SpeedStar2("Speed Star 2", Range( 0 , 0.1)) = 0.03
		_ParallaxMapping("Parallax Mapping", Float) = 100
		_CtrlCloud("Ctrl Cloud", Range( 0 , 2)) = 1.312268
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform float _NormalScale;
		uniform sampler2D _NormalMap;
		uniform float _contrast;
		uniform sampler2D _MainText;
		uniform float _SpeedBackground;
		uniform float _ParallaxMapping;
		uniform float _RotationUV;
		uniform sampler2D _AlphaOutline;
		uniform float _lerpSwitchAlpha;
		uniform sampler2D _TextNoise;
		uniform float4 _TextNoise_ST;
		uniform float _CtrlCloud;
		uniform float _luminosity;
		uniform float _OpacityLayerBackground;
		uniform float4 _MainText_ST;
		uniform sampler2D _StarField;
		uniform float _SpeedStar1;
		uniform float _OpacityLayerStar1;
		uniform float _SpeedStar2;
		uniform float _OpacityLayerStar2;
		uniform float _lerp;
		uniform float _MaskValue;
		uniform float _MaskBias;
		uniform sampler2D _TextMask2;
		uniform sampler2D _TextNoiseMask;
		uniform float _AlphaSubstract;
		uniform float _NormalAlpha;
		uniform float _DissolveAmount;
		uniform float _EdgeThickness;
		uniform float4 _ColorHDROutline;
		uniform float _Cutoff = 0.17;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_cast_0 = (-0.5).xxxx;
			float4 temp_cast_1 = (0.97).xxxx;
			float clampResult257 = clamp( _contrast , 0.0 , 2.0 );
			float mulTime63 = _Time.y * ( _SpeedBackground * UNITY_PI );
			float2 valueSpeed86 = float2( 0,1 );
			float3 ase_worldPos = i.worldPos;
			float2 appendResult321 = (float2(ase_worldPos.xy));
			float2 uv_TexCoord56 = i.uv_texcoord + ( appendResult321 * 0.15 );
			float2 Offset129 = ( ( 0.99 - 1 ) * i.viewDir.xy * _ParallaxMapping ) + uv_TexCoord56;
			float2 uvParallax138 = Offset129;
			float2 anchor84 = float2( 0.5,0.5 );
			float rotation85 = ( ( _RotationUV * UNITY_PI ) * degrees( 0.001 ) * 4 );
			float cos54 = cos( rotation85 );
			float sin54 = sin( rotation85 );
			float2 rotator54 = mul( uvParallax138 - anchor84 , float2x2( cos54 , -sin54 , sin54 , cos54 )) + anchor84;
			float2 panner59 = ( mulTime63 * valueSpeed86 + rotator54);
			float2 mainText110 = panner59;
			float4 rotationNoise1171 = tex2D( _MainText, mainText110 );
			float mulTime240 = _Time.y * -0.2;
			float cos237 = cos( mulTime240 );
			float sin237 = sin( mulTime240 );
			float2 rotator237 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos237 , -sin237 , sin237 , cos237 )) + float2( 0.5,0.5 );
			float mulTime239 = _Time.y * 0.15;
			float cos238 = cos( mulTime239 );
			float sin238 = sin( mulTime239 );
			float2 rotator238 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos238 , -sin238 , sin238 , cos238 )) + float2( 0.5,0.5 );
			float2 lerpResult202 = lerp( rotator237 , rotator238 , ( _CosTime.y * _lerpSwitchAlpha ));
			float4 tex2DNode135 = tex2D( _AlphaOutline, lerpResult202 );
			float rotationNoise2177 = tex2DNode135.r;
			float4 temp_cast_3 = (rotationNoise2177).xxxx;
			float2 uv_TextNoise = i.uv_texcoord * _TextNoise_ST.xy + _TextNoise_ST.zw;
			float mulTime159 = _Time.y * 0.2;
			float cos156 = cos( mulTime159 );
			float sin156 = sin( mulTime159 );
			float2 rotator156 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos156 , -sin156 , sin156 , cos156 )) + float2( 0.5,0.5 );
			float mulTime160 = _Time.y * -0.1;
			float cos157 = cos( mulTime160 );
			float sin157 = sin( mulTime160 );
			float2 rotator157 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos157 , -sin157 , sin157 , cos157 )) + float2( 0.5,0.5 );
			float4 temp_cast_4 = (( tex2D( _MainText, rotator156 ).b + tex2D( _MainText, rotator157 ).b )).xxxx;
			float4 blendOpSrc162 = tex2D( _TextNoise, uv_TextNoise );
			float4 blendOpDest162 = temp_cast_4;
			float4 lerpResult3 = lerp( rotationNoise1171 , temp_cast_3 , ( ( 1.0 - ( blendOpDest162 - blendOpSrc162 ) ) * _CtrlCloud ));
			float4 noise168 = lerpResult3;
			float4 temp_output_259_0 = ( noise168 * _luminosity );
			float grayscale258 = Luminance(temp_output_259_0.rgb);
			float4 temp_output_12_0 = ( ( CalculateContrast(clampResult257,noise168) * grayscale258 ) * _OpacityLayerBackground );
			float2 uv0_MainText = i.uv_texcoord * _MainText_ST.xy + _MainText_ST.zw;
			float2 layeredBlendVar4 = uv0_MainText;
			float4 layeredBlend4 = ( lerp( lerp( temp_output_12_0 , temp_output_12_0 , layeredBlendVar4.x ) , temp_output_12_0 , layeredBlendVar4.y ) );
			float mulTime91 = _Time.y * ( _SpeedStar1 * UNITY_PI );
			float cos97 = cos( rotation85 );
			float sin97 = sin( rotation85 );
			float2 rotator97 = mul( uvParallax138 - anchor84 , float2x2( cos97 , -sin97 , sin97 , cos97 )) + anchor84;
			float2 panner94 = ( mulTime91 * valueSpeed86 + rotator97);
			float2 star1Text113 = panner94;
			float mulTime105 = _Time.y * ( _SpeedStar2 * UNITY_PI );
			float cos106 = cos( rotation85 );
			float sin106 = sin( rotation85 );
			float2 rotator106 = mul( uvParallax138 - anchor84 , float2x2( cos106 , -sin106 , sin106 , cos106 )) + anchor84;
			float2 panner107 = ( mulTime105 * valueSpeed86 + rotator106);
			float2 star2Text114 = panner107;
			float4 weightedBlendVar5 = layeredBlend4;
			float4 weightedAvg5 = ( ( weightedBlendVar5.x*( tex2D( _StarField, star1Text113 ) * ( _OpacityLayerStar1 * UNITY_PI ) ) + weightedBlendVar5.y*( tex2D( _StarField, star2Text114 ) * ( _OpacityLayerStar2 * UNITY_PI ) ) + weightedBlendVar5.z*float4( 0,0,0,0 ) + weightedBlendVar5.w*float4( 0,0,0,0 ) )/( weightedBlendVar5.x + weightedBlendVar5.y + weightedBlendVar5.z + weightedBlendVar5.w ) );
			float4 temp_output_18_0 = ( temp_output_12_0 + weightedAvg5 );
			float4 tex2DNode149 = tex2D( _TextMask2, i.uv_texcoord );
			float mulTime190 = _Time.y * 0.2;
			float cos191 = cos( mulTime190 );
			float sin191 = sin( mulTime190 );
			float2 rotator191 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos191 , -sin191 , sin191 , cos191 )) + float2( 0.5,0.5 );
			float mulTime199 = _Time.y * -0.15;
			float cos198 = cos( mulTime199 );
			float sin198 = sin( mulTime199 );
			float2 rotator198 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos198 , -sin198 , sin198 , cos198 )) + float2( 0.5,0.5 );
			float blendOpSrc194 = tex2DNode149.r;
			float blendOpDest194 = ( tex2D( _TextNoiseMask, rotator191 ).r + tex2D( _TextNoiseMask, rotator198 ).b );
			float lerpBlendMode194 = lerp(blendOpDest194,( blendOpDest194 - blendOpSrc194 ),( 1.0 - _AlphaSubstract ));
			float temp_output_194_0 = ( saturate( lerpBlendMode194 ));
			float temp_output_192_0 = ( temp_output_194_0 * tex2DNode135.a * tex2DNode149.r );
			float smoothstepResult209 = smoothstep( _MaskValue , ( _MaskValue + _MaskBias ) , ( 1.0 - temp_output_192_0 ));
			float4 temp_cast_6 = (( _lerp - smoothstepResult209 )).xxxx;
			float4 lerpResult208 = lerp( temp_output_18_0 , temp_cast_6 , ( 1.0 - temp_output_194_0 ));
			float temp_output_250_0 = ( temp_output_194_0 * tex2DNode135.a );
			float4 smoothstepResult309 = smoothstep( temp_cast_0 , temp_cast_1 , ( ( lerpResult208 * temp_output_250_0 ) * _NormalAlpha ));
			o.Normal = UnpackScaleNormal( tex2D( _NormalMap, -smoothstepResult309.rg ), _NormalScale );
			o.Albedo = lerpResult208.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 appendResult269 = (float2(0.0 , 0.0));
			float simplePerlin3D270 = snoise( ( temp_output_250_0 * ase_vertex3Pos )*appendResult269.x );
			simplePerlin3D270 = simplePerlin3D270*0.5 + 0.5;
			float clampResult281 = clamp( temp_output_250_0 , 0.25 , 1.0 );
			float smoothstepResult272 = smoothstep( simplePerlin3D270 , ( _DissolveAmount + _EdgeThickness ) , clampResult281);
			o.Emission = ( lerpResult208 + ( smoothstepResult272 * _ColorHDROutline ) ).rgb;
			o.Alpha = 1;
			clip( temp_output_192_0 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
1536;0;1536;803;-3134.482;220.6021;2.58223;True;True
Node;AmplifyShaderEditor.CommentaryNode;324;-9082.643,-1324.508;Inherit;False;721.9985;294.2;Comment;4;320;321;323;322;Position Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;99;-7785.865,-1599.937;Inherit;False;1867.307;2451.782;value;3;142;143;144;Global Parameters;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;320;-9032.643,-1274.508;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;144;-7649.911,-365.7529;Inherit;False;1230.429;665.7561;Comment;10;84;58;127;85;67;68;125;65;126;66;Rotation;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;321;-8759.042,-1274.507;Inherit;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;323;-8786.243,-1144.908;Inherit;False;Constant;_PositionSpace;Position Space;30;0;Create;True;0;0;False;0;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;322;-8525.44,-1218.508;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-7286.958,78.32228;Inherit;False;Constant;_Angle;Angle;8;0;Create;True;0;0;False;0;0.001;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-7541.138,-25.02015;Inherit;False;Property;_RotationUV;Rotation UV;24;0;Create;True;0;0;False;0;0;5.25;0;8.727;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;142;-7689.584,-1357.882;Inherit;False;1062.459;770.6912;Comment;6;138;129;130;132;56;131;Parallax Mapping;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;131;-7574.533,-750.7473;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.IntNode;125;-7065.847,159.2798;Inherit;False;Constant;_MultiplyInt;Multiply Int;11;0;Create;True;0;0;False;0;4;2;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-7505.121,-1104.831;Inherit;False;Constant;_Height;Height;12;0;Create;True;0;0;False;0;0.99;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode;65;-7072.957,65.32219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-7601.891,-1260.765;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;68;-7202.958,-19.67829;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-7486.537,-1001.042;Inherit;False;Property;_ParallaxMapping;Parallax Mapping;28;0;Create;True;0;0;False;0;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;109;-5809.017,-2048.778;Inherit;False;2732.206;2939.114;Comment;3;145;146;147;Parameters Locals + Speed UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;143;-6577.693,526.9936;Inherit;False;521.1489;207.4441;Comment;2;86;62;Pannar;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;145;-4881.115,-1767.575;Inherit;False;1685.6;694.3214;Comment;10;110;59;89;54;63;88;61;87;139;60;Main Text;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;58;-7212.271,-242.4809;Inherit;False;Constant;_Anchor;Anchor;12;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-6867.956,3.321714;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;129;-7226.497,-1097.487;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-6954.585,-243.9147;Inherit;False;anchor;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;138;-6914.243,-1099.904;Inherit;False;uvParallax;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-4560.187,-1211.334;Inherit;False;Property;_SpeedBackground;Speed Background;25;0;Create;True;0;0;False;0;0.005;0.005;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;62;-6530.535,588.7726;Inherit;False;Constant;_ValueSpeed;Value Speed;12;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-6679.074,-1.412166;Inherit;False;rotation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;167;-2583.413,-3487.652;Inherit;False;3176.057;1056.266;Comment;17;168;3;178;170;166;162;163;161;154;155;156;157;159;158;160;184;185;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-4438.062,-1416.658;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;61;-4205.83,-1209.842;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-4446.935,-1546.711;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-6311.164,589.1985;Inherit;False;valueSpeed;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-4431.6,-1327.079;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;63;-3906.746,-1211.144;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;159;-2376.877,-3077.091;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;160;-2414.385,-2718.396;Inherit;False;1;0;FLOAT;-0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-4104.77,-1393.199;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;158;-2458.303,-2903.413;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;54;-4115.242,-1532.802;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;239;-2123.728,-183.5132;Inherit;False;1;0;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;189;-2341.003,-298.3176;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosTime;205;-1450.305,-118.7698;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;156;-2087.813,-3119.824;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;59;-3770.244,-1524.549;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;240;-2131.475,-392.1632;Inherit;False;1;0;FLOAT;-0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;157;-2117.001,-2768.276;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1504.545,49.39715;Inherit;False;Property;_lerpSwitchAlpha;lerpSwitchAlpha;1;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;155;-1801.274,-3048.746;Inherit;True;Property;_CloudPositive;Cloud Positive;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-1283.381,-58.3927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;237;-1897.299,-468.7613;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;238;-1870.745,-242.2222;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-3499.831,-1530.175;Inherit;False;mainText;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;154;-1783.546,-2729.653;Inherit;True;Property;_CloudNegative;Cloud Negative;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;111;-1777.46,281.1442;Inherit;True;110;mainText;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;136;-1442.059,-366.1051;Inherit;True;Property;_AlphaOutline;AlphaOutline;17;1;[NoScaleOffset];Create;True;0;0;False;0;da2cc4c8e852a8846860172a4a6c99ab;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;161;-1388.116,-2886.235;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1854.315,22.32769;Inherit;True;Property;_MainText;MainText;14;1;[NoScaleOffset];Create;True;0;0;False;0;None;b24870fb88aedeb48a32ee9c030fd504;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;163;-1796.23,-3357.536;Inherit;True;Property;_TextNoise;Text Noise;18;1;[HideInInspector];Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;202;-1111.13,-177.3708;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BlendOpsNode;162;-1099.722,-3045.259;Inherit;True;Subtract;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-1358.083,258.0845;Inherit;True;Property;_Background;Background;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;135;-755.428,-226.6119;Inherit;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;147;-5716.6,77.98758;Inherit;False;1648.878;701.0396;Comment;10;114;107;106;108;105;141;104;101;103;100;Star 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;146;-5042.562,-821.8654;Inherit;False;1748.833;728.9836;Comment;10;95;140;96;113;94;91;93;97;90;92;Star 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;177;-320.624,-146.9074;Inherit;False;rotationNoise2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-1032.005,-2734.484;Inherit;False;Property;_CtrlCloud;Ctrl Cloud;29;0;Create;True;0;0;False;0;1.312268;1.31;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;166;-788.9257,-3001.371;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;171;-970.0079,253.9912;Inherit;False;rotationNoise1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;-418.3197,-2892.073;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;178;-407.23,-3189.991;Inherit;False;171;rotationNoise1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-4876.116,-248.3005;Inherit;False;Property;_SpeedStar1;Speed Star 1;26;0;Create;True;0;0;False;0;0.01;0.01;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-5630.62,636.7024;Inherit;False;Property;_SpeedStar2;Speed Star 2;27;0;Create;True;0;0;False;0;0.03;0.03;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-409.8828,-3081.208;Inherit;False;177;rotationNoise2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-5344.806,319.0694;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-4595.161,-624.2372;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;3;-99.76085,-3076.225;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;103;-5279.265,638.1946;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-4588.699,-534.6577;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;90;-4524.76,-246.8082;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-4610.805,-730.3591;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;-5359.241,205.6169;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;104;-5338.344,408.6484;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;-5015.325,485.3577;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;105;-4980.18,636.8926;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;199;-1584.507,-487.3777;Inherit;False;1;0;FLOAT;-0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;197.9202,-3080.38;Inherit;False;noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;91;-4232.756,-322.9945;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;190;-1590.358,-696.0284;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;106;-5050.553,296.2404;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;-4267.901,-474.5297;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;97;-4303.129,-663.6469;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-2259.255,991.502;Inherit;False;Property;_luminosity;luminosity;12;0;Create;True;0;0;False;0;1;4.84;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;198;-1329.628,-546.0868;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;107;-4669.596,454.8179;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;254;-2056.941,794.1042;Inherit;False;Property;_contrast;contrast;13;0;Create;True;0;0;False;0;1;1.17;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;-1936.539,569.7151;Inherit;True;168;noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;191;-1356.182,-772.626;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;94;-3922.173,-505.0695;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;188;-1469.878,-1017.846;Inherit;True;Property;_TextNoiseMask;TextNoiseMask;20;1;[NoScaleOffset];Create;True;0;0;False;0;None;b4899a8185d80da41917b6d1ceef9d38;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;187;-1141.826,-940.4543;Inherit;True;Property;_NoiseMask;NoiseMask;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;-3623.984,-507.384;Inherit;False;star1Text;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;148;-1518.79,-1346.934;Inherit;True;Property;_TextMask2;TextMask2;19;1;[NoScaleOffset];Create;True;0;0;False;0;None;ae1b02e7eac6a2148b03cd035a28bd24;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;-1929.082,968.8151;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;257;-1748.751,799.2893;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-1073.059,-692.5415;Inherit;False;Property;_AlphaSubstract;AlphaSubstract;0;0;Create;True;0;0;False;0;0.85;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;197;-1044.698,-511.2949;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;-4382.642,451.342;Inherit;False;star2Text;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1862.446,1554.595;Inherit;True;Property;_StarField;StarField;15;1;[NoScaleOffset];Create;True;0;0;False;0;None;f7b201bd0d48ae04883264fe3f4ca965;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1403.395,2018.54;Inherit;False;Property;_OpacityLayerStar2;Opacity Layer Star 2;23;0;Create;True;0;0;False;0;2;2;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;200;-693.621,-532.7866;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleContrastOpNode;255;-1555.309,697.7419;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1505.033,1539.946;Inherit;False;Property;_OpacityLayerStar1;Opacity Layer Star 1;22;0;Create;True;0;0;False;0;2;2;0;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-1397.414,1439.284;Inherit;False;113;star1Text;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;176;-1445.518,1401.292;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TFHCGrayscale;258;-1544.933,899.2925;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;196;-814.1813,-703.2646;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;-1421.397,1880.628;Inherit;False;114;star2Text;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;149;-1101.911,-1262.293;Inherit;True;Property;_mask;mask;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;262;-1088.246,751.3281;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;194;-277.2658,-912.1549;Inherit;True;Subtract;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1135.082,1288.24;Inherit;True;Property;_Star1;Star 1;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1125.516,968.8687;Inherit;False;Property;_OpacityLayerBackground;Opacity Layer Background;21;0;Create;True;0;0;False;0;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;-1166.58,1725.247;Inherit;True;Property;_Star2;Star 2;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;26;-1171.76,1543.868;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;27;-1056.946,2019.968;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-73.60596,145.2756;Inherit;False;Property;_MaskBias;Mask Bias;8;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-858.0145,534.8091;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;4575.861,214.7361;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-706.9594,1380.158;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-788.6201,797.5152;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-622.3369,1815.701;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-67.33891,2.503235;Inherit;False;Property;_MaskValue;Mask Value;7;0;Create;True;0;0;False;0;0;3;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;212;314.433,65.172;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;122;-348.3934,1723.497;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LayeredBlendNode;4;-512.4456,669.2127;Inherit;True;6;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;241;490.3804,-209.978;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;128;-391.2374,1400.311;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;209;793.2062,-25.94708;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WeightedBlendNode;5;217.8092,1015.933;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;219;897.4775,-136.9947;Inherit;False;Property;_lerp;lerp;6;0;Create;True;0;0;False;0;0;-0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;264;1352.127,808.3419;Inherit;False;2525.495;1135.108;Comment;14;250;216;271;266;267;275;272;281;270;269;279;265;268;282;Color Outline;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;236;1156.256,-86.88419;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;690.8724,917.546;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;265;1820.949,1481.185;Inherit;False;Constant;_Scale;Scale;12;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;214;1103.927,-364.8484;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;268;1674.45,1275.882;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;1467.467,927.1339;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;282;2231.734,1146.715;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;266;2357.281,1844.188;Inherit;False;Property;_EdgeThickness;Edge Thickness;9;0;Create;True;0;0;False;0;0.1377845;0.15;0;0.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;279;1991.801,1199.737;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;269;2163.778,1488.222;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;208;1815.359,-77.23283;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;267;2352.407,1693.127;Inherit;False;Property;_DissolveAmount;DissolveAmount;10;0;Create;True;0;0;False;0;0.4235094;0.121;-0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;271;2841.248,1722.453;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;281;2760.378,1320.547;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;315;2384.904,153.691;Inherit;True;Property;_NormalAlpha;Normal Alpha;2;0;Create;True;0;0;False;0;0.75;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;2376.992,-117.9577;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;270;2392.855,1397.313;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;272;3217.594,1419.397;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;319;2767.021,22.35674;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;216;3232.316,1678.538;Inherit;False;Property;_ColorHDROutline;Color HDR Outline;11;1;[HDR];Create;True;0;0;False;0;0,8.314797,15.1544,0;5.992157,1.129412,4.517647,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;311;2931.415,-296.4481;Inherit;False;Constant;_minNormal;min Normal;29;0;Create;True;0;0;False;0;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;310;2927.624,-170.8058;Inherit;False;Constant;_maxNormal;max Normal;29;0;Create;True;0;0;False;0;0.97;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;309;3310.813,-226.2361;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;3571.578,1428.634;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;284;3927.396,1310.267;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;288;3693.274,-91.2729;Inherit;False;Property;_NormalScale;Normal Scale;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;313;3698.711,-223.0332;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;283;2885.212,320.97;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;338;4085.435,-512.595;Inherit;False;Constant;_MaskY;Mask Y;4;0;Create;True;0;0;False;0;0.5;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;344;5413.727,-295.8893;Inherit;True;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;1822.315,243.8639;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;343;4761.897,-719.8541;Inherit;True;Rectangle;-1;;3;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;292;4022.468,-79.93906;Inherit;True;Property;_NormalMap;Normal Map;16;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;-1;None;84551997ee687484bbc6f7d3d265bb4b;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;340;4432.916,-850.7341;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;263;-1747.557,946.5529;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;2,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;5165.056,-163.4493;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;341;4565.35,-457.7416;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;243;4222.444,310.7553;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleNode;337;4350.135,-379.4819;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;127;-7509.903,-121.2447;Inherit;False;34,9 + 17.4 + 11,63 + 8.727$$Int 1 |  Int 2 |  Int 3 |   Int 4$;1;False;1;True;In0;FLOAT;0;In;;Float;False;Value Rotation;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;4544.28,-620.282;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;339;4081.211,-610.9739;Inherit;False;Constant;_MaskX;Mask X;3;0;Create;True;0;0;False;0;1.07;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;336;4028.305,-374.9348;Inherit;False;Property;_MaskScale;Mask Scale;4;0;Create;True;0;0;False;0;1;0.408;0;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5969.103,7.438876;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Thibaut/Vfx/Shader_DisfractionEnergy01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.17;True;True;0;True;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;267;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;321;0;320;0
WireConnection;322;0;321;0
WireConnection;322;1;323;0
WireConnection;65;0;66;0
WireConnection;56;1;322;0
WireConnection;68;0;126;0
WireConnection;67;0;68;0
WireConnection;67;1;65;0
WireConnection;67;2;125;0
WireConnection;129;0;56;0
WireConnection;129;1;132;0
WireConnection;129;2;130;0
WireConnection;129;3;131;0
WireConnection;84;0;58;0
WireConnection;138;0;129;0
WireConnection;85;0;67;0
WireConnection;61;0;60;0
WireConnection;86;0;62;0
WireConnection;63;0;61;0
WireConnection;54;0;139;0
WireConnection;54;1;87;0
WireConnection;54;2;88;0
WireConnection;156;0;158;0
WireConnection;156;2;159;0
WireConnection;59;0;54;0
WireConnection;59;2;89;0
WireConnection;59;1;63;0
WireConnection;157;0;158;0
WireConnection;157;2;160;0
WireConnection;155;1;156;0
WireConnection;207;0;205;2
WireConnection;207;1;206;0
WireConnection;237;0;189;0
WireConnection;237;2;240;0
WireConnection;238;0;189;0
WireConnection;238;2;239;0
WireConnection;110;0;59;0
WireConnection;154;1;157;0
WireConnection;161;0;155;3
WireConnection;161;1;154;3
WireConnection;202;0;237;0
WireConnection;202;1;238;0
WireConnection;202;2;207;0
WireConnection;162;0;163;0
WireConnection;162;1;161;0
WireConnection;2;0;1;0
WireConnection;2;1;111;0
WireConnection;135;0;136;0
WireConnection;135;1;202;0
WireConnection;177;0;135;1
WireConnection;166;0;162;0
WireConnection;171;0;2;0
WireConnection;185;0;166;0
WireConnection;185;1;184;0
WireConnection;3;0;178;0
WireConnection;3;1;170;0
WireConnection;3;2;185;0
WireConnection;103;0;100;0
WireConnection;90;0;92;0
WireConnection;105;0;103;0
WireConnection;168;0;3;0
WireConnection;91;0;90;0
WireConnection;106;0;141;0
WireConnection;106;1;101;0
WireConnection;106;2;104;0
WireConnection;97;0;140;0
WireConnection;97;1;96;0
WireConnection;97;2;95;0
WireConnection;198;0;189;0
WireConnection;198;2;199;0
WireConnection;107;0;106;0
WireConnection;107;2;108;0
WireConnection;107;1;105;0
WireConnection;191;0;189;0
WireConnection;191;2;190;0
WireConnection;94;0;97;0
WireConnection;94;2;93;0
WireConnection;94;1;91;0
WireConnection;187;0;188;0
WireConnection;187;1;191;0
WireConnection;113;0;94;0
WireConnection;259;0;169;0
WireConnection;259;1;260;0
WireConnection;257;0;254;0
WireConnection;197;0;188;0
WireConnection;197;1;198;0
WireConnection;114;0;107;0
WireConnection;200;0;187;1
WireConnection;200;1;197;3
WireConnection;255;1;169;0
WireConnection;255;0;257;0
WireConnection;176;0;6;0
WireConnection;258;0;259;0
WireConnection;196;0;195;0
WireConnection;149;0;148;0
WireConnection;149;1;189;0
WireConnection;262;0;255;0
WireConnection;262;1;258;0
WireConnection;194;0;149;1
WireConnection;194;1;200;0
WireConnection;194;2;196;0
WireConnection;7;0;176;0
WireConnection;7;1;112;0
WireConnection;69;0;6;0
WireConnection;69;1;115;0
WireConnection;26;0;14;0
WireConnection;27;0;16;0
WireConnection;192;0;194;0
WireConnection;192;1;135;4
WireConnection;192;2;149;1
WireConnection;13;0;7;0
WireConnection;13;1;26;0
WireConnection;12;0;262;0
WireConnection;12;1;11;0
WireConnection;15;0;69;0
WireConnection;15;1;27;0
WireConnection;212;0;210;0
WireConnection;212;1;211;0
WireConnection;122;0;15;0
WireConnection;4;0;10;0
WireConnection;4;1;12;0
WireConnection;4;2;12;0
WireConnection;4;3;12;0
WireConnection;241;0;192;0
WireConnection;128;0;13;0
WireConnection;209;0;241;0
WireConnection;209;1;210;0
WireConnection;209;2;212;0
WireConnection;5;0;4;0
WireConnection;5;1;128;0
WireConnection;5;2;122;0
WireConnection;236;0;219;0
WireConnection;236;1;209;0
WireConnection;18;0;12;0
WireConnection;18;1;5;0
WireConnection;214;0;194;0
WireConnection;250;0;194;0
WireConnection;250;1;135;4
WireConnection;282;0;250;0
WireConnection;279;0;250;0
WireConnection;279;1;268;0
WireConnection;269;0;265;0
WireConnection;269;1;265;0
WireConnection;208;0;18;0
WireConnection;208;1;236;0
WireConnection;208;2;214;0
WireConnection;271;0;267;0
WireConnection;271;1;266;0
WireConnection;281;0;282;0
WireConnection;304;0;208;0
WireConnection;304;1;250;0
WireConnection;270;0;279;0
WireConnection;270;1;269;0
WireConnection;272;0;281;0
WireConnection;272;1;270;0
WireConnection;272;2;271;0
WireConnection;319;0;304;0
WireConnection;319;1;315;0
WireConnection;309;0;319;0
WireConnection;309;1;311;0
WireConnection;309;2;310;0
WireConnection;275;0;272;0
WireConnection;275;1;216;0
WireConnection;284;0;275;0
WireConnection;313;0;309;0
WireConnection;283;0;208;0
WireConnection;344;0;345;0
WireConnection;303;0;135;0
WireConnection;303;1;18;0
WireConnection;343;1;340;0
WireConnection;343;2;342;0
WireConnection;343;3;341;0
WireConnection;292;1;313;0
WireConnection;292;5;288;0
WireConnection;263;0;259;0
WireConnection;345;0;343;0
WireConnection;345;1;192;0
WireConnection;341;0;338;0
WireConnection;341;1;337;0
WireConnection;243;0;283;0
WireConnection;243;1;284;0
WireConnection;337;0;336;0
WireConnection;342;0;339;0
WireConnection;342;1;337;0
WireConnection;0;0;208;0
WireConnection;0;1;292;0
WireConnection;0;2;243;0
WireConnection;0;10;192;0
ASEEND*/
//CHKSM=6101525F3BEEF23A7BC2B52A024710AE8F1F8930