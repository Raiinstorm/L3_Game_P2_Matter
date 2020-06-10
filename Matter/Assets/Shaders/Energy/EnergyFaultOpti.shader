// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Thibaut/Vfx/Shader_EnergyOpti"
{
	Properties
	{
		[NoScaleOffset]_MainText("MainText", 2D) = "white" {}
		[NoScaleOffset]_StarField("StarField", 2D) = "white" {}
		[NoScaleOffset]_TextNoiseMask("TextNoiseMask", 2D) = "white" {}
		[NoScaleOffset]_TextMask2("TextMask2", 2D) = "white" {}
		_ParallaxMapping("Parallax Mapping", Float) = 100
		_Cutoff( "Mask Clip Value", Float ) = 0.17
		_AlphaSubstract("AlphaSubstract", Float) = 1.15
		_Noise("Noise", Range( 0 , 1)) = 0.28
		_MaskNoise("Mask Noise", Range( -1 , 1)) = 0.5
		_EdgeThickness("Edge Thickness", Range( 0 , 0.15)) = 0.15
		_DissolveAmount("Dissolve Amount", Range( -0.1 , 0.13)) = 0.121
		_Luminosity("Luminosity", Range( 1 , 5)) = 2.3
		_Contrast("Contrast", Range( 0 , 1.4)) = 0.8
		[HDR]_ColorHDROutline("Color HDR Outline", Color) = (5.992157,1.129412,4.517647,0)
		_ToogleHUE("Toogle HUE", Range( 0 , 1)) = 0
		_HUE("HUE", Range( 0 , 1)) = 0.246
		_Saturation("Saturation", Range( 0 , 1)) = 0.5
		_OpacityLayerBackground("Opacity Layer Background", Range( 0 , 5)) = 1.2
		_OpacityLayerStar1("Opacity Layer Star 1", Range( 0 , 2)) = 0.5
		_OpacityLayerStar2("Opacity Layer Star 2", Range( 0 , 2)) = 0.5
		_RotationUV("Rotation UV", Range( 0 , 8.727)) = 5.25
		_SpeedBackground("Speed Background", Range( 0 , 0.1)) = 0.005
		_SpeedStar1("Speed Star 1", Range( 0 , 0.1)) = 0.01
		_SpeedStar2("Speed Star 2", Range( 0 , 0.1)) = 0.03
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _MainText;
		uniform float4 _MainText_ST;
		uniform float _Contrast;
		uniform float _SpeedBackground;
		uniform float _ParallaxMapping;
		uniform float _RotationUV;
		uniform sampler2D _TextNoiseMask;
		uniform float _Noise;
		uniform float _Luminosity;
		uniform float _HUE;
		uniform float _Saturation;
		uniform float _ToogleHUE;
		uniform float _OpacityLayerBackground;
		uniform sampler2D _StarField;
		uniform float _SpeedStar1;
		uniform float _OpacityLayerStar1;
		uniform float _SpeedStar2;
		uniform float _OpacityLayerStar2;
		uniform float _MaskNoise;
		uniform sampler2D _TextMask2;
		uniform float _AlphaSubstract;
		uniform float _DissolveAmount;
		uniform float _EdgeThickness;
		uniform float4 _ColorHDROutline;
		uniform float _Cutoff = 0.17;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
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
			o.Normal = float3(0,0,1);
			float2 uv0_MainText = i.uv_texcoord * _MainText_ST.xy + _MainText_ST.zw;
			float clampResult257 = clamp( _Contrast , 0.0 , 2.0 );
			float mulTime63 = _Time.y * ( _SpeedBackground * UNITY_PI );
			float2 valueSpeed86 = float2( 0,1 );
			float3 ase_worldPos = i.worldPos;
			float2 appendResult321 = (float2(ase_worldPos.xy));
			float2 positionWorld359 = ( appendResult321 * 0.04 );
			float2 uv_TexCoord56 = i.uv_texcoord + positionWorld359;
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
			float2 uv_TextNoiseMask163 = i.uv_texcoord;
			float mulTime159 = _Time.y * 0.2;
			float cos156 = cos( mulTime159 );
			float sin156 = sin( mulTime159 );
			float2 rotator156 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos156 , -sin156 , sin156 , cos156 )) + float2( 0.5,0.5 );
			float mulTime160 = _Time.y * -0.1;
			float cos157 = cos( mulTime160 );
			float sin157 = sin( mulTime160 );
			float2 rotator157 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos157 , -sin157 , sin157 , cos157 )) + float2( 0.5,0.5 );
			float blendOpSrc162 = tex2D( _TextNoiseMask, uv_TextNoiseMask163 ).b;
			float blendOpDest162 = ( tex2D( _MainText, rotator156 ).b + tex2D( _MainText, rotator157 ).b );
			float4 lerpResult3 = lerp( rotationNoise1171 , float4( 0,0,0,0 ) , ( ( 1.0 - ( blendOpDest162 - blendOpSrc162 ) ) * _Noise ));
			float4 noise168 = lerpResult3;
			float4 temp_output_259_0 = ( noise168 * _Luminosity );
			float grayscale258 = Luminance(temp_output_259_0.rgb);
			float3 hsvTorgb347 = HSVToRGB( float3(_HUE,( _Saturation * _ToogleHUE ),1.0) );
			float4 appendResult351 = (float4(hsvTorgb347 , 0.0));
			float4 temp_output_12_0 = ( ( ( CalculateContrast(clampResult257,noise168) * grayscale258 ) * appendResult351 ) * _OpacityLayerBackground );
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
			float4 colorAlbedo389 = ( layeredBlend4 + weightedAvg5 );
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
			float temp_output_192_0 = ( temp_output_194_0 * tex2DNode149.r );
			float smoothstepResult209 = smoothstep( _MaskNoise , ( _MaskNoise + 1 ) , ( 1.0 - temp_output_192_0 ));
			float4 temp_cast_2 = (( smoothstepResult209 - 0.1 )).xxxx;
			float4 lerpResult208 = lerp( colorAlbedo389 , temp_cast_2 , ( 1.0 - temp_output_194_0 ));
			float4 albedo398 = lerpResult208;
			o.Albedo = albedo398.rgb;
			float blendColorOutline387 = temp_output_194_0;
			float temp_output_250_0 = ( blendColorOutline387 * 1.0 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 appendResult269 = (float2(0.0 , 0.0));
			float simplePerlin3D270 = snoise( ( temp_output_250_0 * ase_vertex3Pos )*appendResult269.x );
			simplePerlin3D270 = simplePerlin3D270*0.5 + 0.5;
			float clampResult281 = clamp( temp_output_250_0 , 0.25 , 1.0 );
			float smoothstepResult272 = smoothstep( simplePerlin3D270 , ( _DissolveAmount + _EdgeThickness ) , clampResult281);
			float4 colorOutline384 = ( smoothstepResult272 * _ColorHDROutline );
			o.Emission = ( albedo398 + colorOutline384 ).rgb;
			o.Alpha = 1;
			float opacityMask391 = temp_output_192_0;
			clip( opacityMask391 - _Cutoff );
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
1640.8;113.6;1536;803;13348.44;4922.293;12.3232;True;False
Node;AmplifyShaderEditor.CommentaryNode;324;-9631.739,-1279.089;Inherit;False;1205.043;401.5604;Comment;5;359;322;323;321;320;Position Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;320;-9505.844,-1169.455;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;323;-9259.444,-1039.855;Inherit;False;Constant;_PositionSpace;Position Space;30;0;Create;True;0;0;False;0;0.04;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;321;-9232.243,-1169.454;Inherit;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;99;-7960.326,-1622.144;Inherit;False;2028.522;2837.078;value;3;143;144;142;Global Parameters;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;322;-8998.642,-1113.455;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;142;-7813.254,-1251.508;Inherit;False;1404.427;809.7326;Comment;7;360;138;129;132;131;130;56;Parallax Mapping;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;359;-8783.304,-1120.467;Inherit;False;positionWorld;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;144;-7783.46,-148.4117;Inherit;False;1230.429;665.7561;Comment;10;84;58;127;85;67;68;125;65;126;66;Rotation;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;360;-7687.952,-1114.803;Inherit;False;359;positionWorld;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-7674.687,192.321;Inherit;False;Property;_RotationUV;Rotation UV;20;0;Create;True;0;0;False;0;5.25;5.25;0;8.727;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-7420.507,295.6634;Inherit;False;Constant;_Angle;Angle;8;0;Create;True;0;0;False;0;0.001;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DegreesOpNode;65;-7206.506,282.6633;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-7286.824,-998.4569;Inherit;False;Constant;_Height;Height;12;0;Create;True;0;0;False;0;0.99;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;109;-5809.017,-2048.778;Inherit;False;2732.206;2939.114;Comment;3;145;146;147;Local Parameters  + Speed UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;125;-7199.396,376.6208;Inherit;False;Constant;_MultiplyInt;Multiply Int;11;0;Create;True;0;0;False;0;4;2;0;1;INT;0
Node;AmplifyShaderEditor.PiNode;68;-7336.507,197.6628;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;131;-7356.236,-644.3723;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;130;-7294.24,-889.6675;Inherit;False;Property;_ParallaxMapping;Parallax Mapping;4;0;Create;True;0;0;False;0;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-7383.594,-1154.391;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;58;-7345.82,-25.13976;Inherit;False;Constant;_Anchor;Anchor;12;0;Create;True;0;0;False;0;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-7001.505,220.6629;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;129;-7008.2,-991.1129;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;145;-4881.115,-1767.575;Inherit;False;1685.6;694.3214;Comment;10;110;59;89;54;63;88;61;87;139;60;Main Text;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;143;-6766.58,815.1961;Inherit;False;521.1489;267.0779;Comment;2;86;62;Pannar;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-7088.134,-26.57356;Inherit;False;anchor;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-6812.623,215.929;Inherit;False;rotation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;62;-6703.158,909.5027;Inherit;False;Constant;_ValueSpeed;Value Speed;12;0;Create;True;0;0;False;0;0,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;60;-4560.187,-1211.334;Inherit;False;Property;_SpeedBackground;Speed Background;21;0;Create;True;0;0;False;0;0.005;0.005;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;138;-6695.946,-993.5299;Inherit;False;uvParallax;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;167;-2358.725,-3131.407;Inherit;False;6123.936;2651.671;Comment;19;168;3;185;178;166;184;162;163;161;383;394;155;154;157;156;158;159;160;397;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-4446.935,-1546.711;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;-4438.062,-1416.658;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;-4431.6,-1327.079;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-6483.787,909.9286;Inherit;False;valueSpeed;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;61;-4205.83,-1209.842;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;63;-3906.746,-1211.144;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;158;-868.8719,-2258.332;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;89;-4104.77,-1393.199;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;54;-4115.242,-1532.802;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;160;-824.9543,-2073.315;Inherit;False;1;0;FLOAT;-0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;159;-787.4461,-2432.012;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;397;-2264.415,-1737.48;Inherit;False;4178.668;1155.577;Comment;28;188;189;190;199;198;191;148;187;197;195;200;196;149;194;387;192;210;377;241;376;209;219;390;236;214;208;398;391;Mask Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.RotatorNode;156;-498.3824,-2474.745;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;188;-2143.781,-1418.774;Inherit;True;Property;_TextNoiseMask;TextNoiseMask;2;1;[NoScaleOffset];Create;True;0;0;False;0;b4899a8185d80da41917b6d1ceef9d38;b4899a8185d80da41917b6d1ceef9d38;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RotatorNode;157;-527.5703,-2123.196;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;59;-3770.244,-1524.549;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;383;-899.6293,-2467.524;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;154;-194.115,-2084.573;Inherit;True;Property;_CloudNegative;Cloud Negative;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-3499.831,-1530.175;Inherit;False;mainText;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;155;-211.8431,-2403.667;Inherit;True;Property;_CloudPositive;Cloud Positive;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;394;2114.459,-1227.738;Inherit;False;1202.987;654.2164;Comment;4;171;2;1;111;Rotation Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;163;-206.7988,-2712.456;Inherit;True;Property;_TextNoise;Text Noise;13;1;[HideInInspector];Create;True;0;0;False;0;-1;b4899a8185d80da41917b6d1ceef9d38;b4899a8185d80da41917b6d1ceef9d38;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;161;201.3148,-2241.154;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;2232.611,-829.4366;Inherit;True;110;mainText;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;2184.64,-1131.578;Inherit;True;Property;_MainText;MainText;0;1;[NoScaleOffset];Create;True;0;0;False;0;b24870fb88aedeb48a32ee9c030fd504;b24870fb88aedeb48a32ee9c030fd504;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;162;491.3014,-2478.315;Inherit;True;Subtract;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;2695.314,-874.1592;Inherit;True;Property;_Background;Background;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;184;745.38,-2118.939;Inherit;False;Property;_Noise;Noise;7;0;Create;True;0;0;False;0;0.28;1.31;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;171;3083.389,-878.2524;Inherit;False;rotationNoise1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;166;846.7456,-2380.21;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;147;-5716.6,77.98758;Inherit;False;1648.878;701.0396;Comment;10;114;107;106;108;105;141;104;101;103;100;Star 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;146;-5042.562,-821.8654;Inherit;False;1748.833;728.9836;Comment;10;95;140;96;113;94;91;93;97;90;92;Star 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;1171.109,-2246.992;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;178;1182.199,-2544.911;Inherit;False;171;rotationNoise1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;3;1489.669,-2431.146;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-4876.116,-248.3005;Inherit;False;Property;_SpeedStar1;Speed Star 1;22;0;Create;True;0;0;False;0;0.01;0.01;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-5630.62,636.7024;Inherit;False;Property;_SpeedStar2;Speed Star 2;23;0;Create;True;0;0;False;0;0.03;0.03;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-4610.805,-730.3591;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;101;-5344.806,319.0694;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;393;-2885.179,151.3706;Inherit;False;3984.438;1833.455;Hue, Emission Starfield, Luminosity, Constrast;40;396;395;128;389;18;5;169;263;4;15;13;10;69;12;27;7;11;115;352;26;16;112;351;14;176;262;255;347;6;258;367;259;371;257;356;354;260;254;355;403;Colorimetry + Layers;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;-5359.241,205.6169;Inherit;False;138;uvParallax;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PiNode;90;-4524.76,-246.8082;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-4595.161,-624.2372;Inherit;False;84;anchor;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;104;-5338.344,408.6484;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;1787.35,-2435.301;Inherit;False;noise;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;103;-5279.265,638.1946;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;95;-4588.699,-534.6577;Inherit;False;85;rotation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;189;-2190.854,-1169.041;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;169;-2795.182,500.2017;Inherit;True;168;noise;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;-4267.901,-474.5297;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;199;-1799.149,-889.0819;Inherit;False;1;0;FLOAT;-0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;190;-1805,-1097.734;Inherit;False;1;0;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;108;-5015.325,485.3577;Inherit;False;86;valueSpeed;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;97;-4303.129,-663.6469;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-2696.829,758.1136;Inherit;False;Property;_Luminosity;Luminosity;11;0;Create;True;0;0;False;0;2.3;4.84;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;354;-2414.753,1035.414;Inherit;False;Property;_Saturation;Saturation;16;0;Create;True;0;0;False;0;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;355;-2391.396,1252.228;Inherit;False;Constant;_HUEAlpha;HUE Alpha;26;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;254;-2494.515,560.7157;Inherit;False;Property;_Contrast;Contrast;12;0;Create;True;0;0;False;0;0.8;1.17;0;1.4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;403;-2467.249,1146.827;Inherit;False;Property;_ToogleHUE;Toogle HUE;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;105;-4980.18,636.8926;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;106;-5050.553,296.2404;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;91;-4232.756,-322.9945;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;371;-2083.951,1221.016;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;191;-1570.823,-1174.331;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;356;-2439.225,932.1218;Inherit;False;Property;_HUE;HUE;15;0;Create;True;0;0;False;0;0.246;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;107;-4669.596,454.8179;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;94;-3922.173,-505.0695;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;-2366.656,735.4268;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;198;-1575.309,-951.6716;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;257;-2186.325,565.9009;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;367;-2122.951,1059.016;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1487.593,1311.836;Inherit;True;Property;_StarField;StarField;1;1;[NoScaleOffset];Create;True;0;0;False;0;f7b201bd0d48ae04883264fe3f4ca965;f7b201bd0d48ae04883264fe3f4ca965;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;-4382.642,451.342;Inherit;False;star2Text;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.HSVToRGBNode;347;-1895.085,1009.236;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleContrastOpNode;255;-1992.882,464.3535;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;197;-1309.778,-986.72;Inherit;True;Property;_TextureSample0;Texture Sample 0;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;187;-1306.026,-1256.8;Inherit;True;Property;_NoiseMask;NoiseMask;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;148;-1574.351,-1589.559;Inherit;True;Property;_TextMask2;TextMask2;3;1;[NoScaleOffset];Create;True;0;0;False;0;ae1b02e7eac6a2148b03cd035a28bd24;ae1b02e7eac6a2148b03cd035a28bd24;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;-3623.984,-507.384;Inherit;False;star1Text;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCGrayscale;258;-1982.507,665.9041;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;195;-1151.899,-690.7256;Inherit;False;Property;_AlphaSubstract;AlphaSubstract;6;0;Create;True;0;0;False;0;1.15;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1130.18,1297.187;Inherit;False;Property;_OpacityLayerStar1;Opacity Layer Star 1;18;0;Create;True;0;0;False;0;0.5;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;200;-900.502,-1015.972;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;196;-886.1873,-793.1812;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;-1022.561,1196.525;Inherit;False;113;star1Text;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;176;-1070.665,1158.532;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;262;-1628.03,419.8834;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1028.542,1775.781;Inherit;False;Property;_OpacityLayerStar2;Opacity Layer Star 2;19;0;Create;True;0;0;False;0;0.5;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;-1046.544,1637.869;Inherit;False;114;star2Text;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;351;-1570.034,705.0774;Inherit;True;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;149;-1200.151,-1594.158;Inherit;True;Property;_mask;mask;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;69;-791.7262,1482.488;Inherit;True;Property;_Star2;Star 2;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1367.975,956.9367;Inherit;False;Property;_OpacityLayerBackground;Opacity Layer Background;17;0;Create;True;0;0;False;0;1.2;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;194;-674.7405,-1164.445;Inherit;True;Subtract;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-760.2282,1045.481;Inherit;True;Property;_Star1;Star 1;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;26;-796.9063,1301.109;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;352;-1260.69,583.4998;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PiNode;27;-682.092,1777.209;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1008.523,527.3671;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-332.105,1137.399;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;264;1424.294,897.4467;Inherit;False;2788.438;1145.353;Comment;17;275;272;216;271;281;270;279;266;267;282;269;268;265;250;363;384;388;Outline Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;387;-287.0535,-1174.55;Inherit;False;blendColorOutline;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-247.4825,1572.942;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-991.9752,778.0627;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;363;1511.441,1130.595;Inherit;False;Constant;_ColorOutline;Color Outline;17;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;128;-65.39685,1126.918;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;388;1475.038,1013.092;Inherit;False;387;blendColorOutline;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;4;-662.9547,661.7708;Inherit;True;6;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;395;30.04506,1449.309;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;268;1746.617,1364.986;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;377;-251.696,-774.3323;Inherit;False;Constant;_MaskAdd;Mask Add;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-326.3445,-1471.765;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-392.0507,-888.0966;Inherit;False;Property;_MaskNoise;Mask Noise;8;0;Create;True;0;0;False;0;0.5;3;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WeightedBlendNode;5;122.0894,975.0021;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;265;1893.116,1570.29;Inherit;False;Constant;_Scale;Scale;12;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;1763.681,1062.662;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;396;134.1983,806.007;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;269;2235.943,1577.327;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;241;28.36252,-1466.671;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;376;-27.81435,-828.2643;Inherit;False;2;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;282;2303.898,1235.82;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;266;2429.445,1933.294;Inherit;False;Property;_EdgeThickness;Edge Thickness;9;0;Create;True;0;0;False;0;0.15;0.15;0;0.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;2424.571,1782.233;Inherit;False;Property;_DissolveAmount;Dissolve Amount;10;0;Create;True;0;0;False;0;0.121;0.121;-0.1;0.13;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;279;2063.969,1288.843;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;531.1036,785.2032;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;389;797.992,778.3959;Inherit;False;colorAlbedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;219;358.8621,-1028.762;Inherit;False;Constant;_Luminositynoise;Luminosity noise;9;0;Create;True;0;0;False;0;0.1;-0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;271;2913.412,1811.558;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;281;2832.542,1409.651;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;270;2465.019,1486.417;Inherit;True;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;209;346.7713,-1376.619;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;390;834.0848,-1221.741;Inherit;False;389;colorAlbedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;272;3289.758,1508.501;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;214;-225.2706,-1059.995;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;216;3304.48,1767.642;Inherit;False;Property;_ColorHDROutline;Color HDR Outline;13;1;[HDR];Create;True;0;0;False;0;5.992157,1.129412,4.517647,0;5.992157,1.129412,4.517647,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;236;769.2302,-1059.648;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;275;3643.742,1517.739;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;208;1261.01,-939.0505;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;402;3022.68,2.038364;Inherit;False;1106.66;656.67;shader fault;6;401;399;385;243;0;392;MAT;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;384;3901.677,1506.995;Inherit;False;colorOutline;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;398;1615.021,-943.0768;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;385;3110.654,292.8466;Inherit;False;384;colorOutline;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;399;3121.025,117.0118;Inherit;False;398;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;391;-3.30163,-1367.615;Inherit;False;opacityMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;263;-2187.031,769.3647;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;2,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;243;3442.761,207.7695;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;127;-7643.452,96.09641;Inherit;False;34,9 + 17.4 + 11,63 + 8.727$$Int 1 |  Int 2 |  Int 3 |   Int 4$;1;False;1;True;In0;FLOAT;0;In;;Float;False;Value Rotation;True;False;0;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;401;3617.114,167.8264;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;392;3456.856,457.4018;Inherit;False;391;opacityMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3802.468,166.7104;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Thibaut/Vfx/Shader_EnergyOpti;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.17;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;267;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;321;0;320;0
WireConnection;322;0;321;0
WireConnection;322;1;323;0
WireConnection;359;0;322;0
WireConnection;65;0;66;0
WireConnection;68;0;126;0
WireConnection;56;1;360;0
WireConnection;67;0;68;0
WireConnection;67;1;65;0
WireConnection;67;2;125;0
WireConnection;129;0;56;0
WireConnection;129;1;132;0
WireConnection;129;2;130;0
WireConnection;129;3;131;0
WireConnection;84;0;58;0
WireConnection;85;0;67;0
WireConnection;138;0;129;0
WireConnection;86;0;62;0
WireConnection;61;0;60;0
WireConnection;63;0;61;0
WireConnection;54;0;139;0
WireConnection;54;1;87;0
WireConnection;54;2;88;0
WireConnection;156;0;158;0
WireConnection;156;2;159;0
WireConnection;157;0;158;0
WireConnection;157;2;160;0
WireConnection;59;0;54;0
WireConnection;59;2;89;0
WireConnection;59;1;63;0
WireConnection;383;0;188;0
WireConnection;154;1;157;0
WireConnection;110;0;59;0
WireConnection;155;1;156;0
WireConnection;163;0;383;0
WireConnection;161;0;155;3
WireConnection;161;1;154;3
WireConnection;162;0;163;3
WireConnection;162;1;161;0
WireConnection;2;0;1;0
WireConnection;2;1;111;0
WireConnection;171;0;2;0
WireConnection;166;0;162;0
WireConnection;185;0;166;0
WireConnection;185;1;184;0
WireConnection;3;0;178;0
WireConnection;3;2;185;0
WireConnection;90;0;92;0
WireConnection;168;0;3;0
WireConnection;103;0;100;0
WireConnection;97;0;140;0
WireConnection;97;1;96;0
WireConnection;97;2;95;0
WireConnection;105;0;103;0
WireConnection;106;0;141;0
WireConnection;106;1;101;0
WireConnection;106;2;104;0
WireConnection;91;0;90;0
WireConnection;371;0;355;0
WireConnection;191;0;189;0
WireConnection;191;2;190;0
WireConnection;107;0;106;0
WireConnection;107;2;108;0
WireConnection;107;1;105;0
WireConnection;94;0;97;0
WireConnection;94;2;93;0
WireConnection;94;1;91;0
WireConnection;259;0;169;0
WireConnection;259;1;260;0
WireConnection;198;0;189;0
WireConnection;198;2;199;0
WireConnection;257;0;254;0
WireConnection;367;0;354;0
WireConnection;367;1;403;0
WireConnection;114;0;107;0
WireConnection;347;0;356;0
WireConnection;347;1;367;0
WireConnection;347;2;371;0
WireConnection;255;1;169;0
WireConnection;255;0;257;0
WireConnection;197;0;188;0
WireConnection;197;1;198;0
WireConnection;187;0;188;0
WireConnection;187;1;191;0
WireConnection;113;0;94;0
WireConnection;258;0;259;0
WireConnection;200;0;187;1
WireConnection;200;1;197;3
WireConnection;196;0;195;0
WireConnection;176;0;6;0
WireConnection;262;0;255;0
WireConnection;262;1;258;0
WireConnection;351;0;347;0
WireConnection;149;0;148;0
WireConnection;149;1;189;0
WireConnection;69;0;6;0
WireConnection;69;1;115;0
WireConnection;194;0;149;1
WireConnection;194;1;200;0
WireConnection;194;2;196;0
WireConnection;7;0;176;0
WireConnection;7;1;112;0
WireConnection;26;0;14;0
WireConnection;352;0;262;0
WireConnection;352;1;351;0
WireConnection;27;0;16;0
WireConnection;13;0;7;0
WireConnection;13;1;26;0
WireConnection;387;0;194;0
WireConnection;15;0;69;0
WireConnection;15;1;27;0
WireConnection;12;0;352;0
WireConnection;12;1;11;0
WireConnection;128;0;13;0
WireConnection;4;0;10;0
WireConnection;4;1;12;0
WireConnection;4;2;12;0
WireConnection;4;3;12;0
WireConnection;395;0;15;0
WireConnection;192;0;194;0
WireConnection;192;1;149;1
WireConnection;5;0;4;0
WireConnection;5;1;128;0
WireConnection;5;2;395;0
WireConnection;250;0;388;0
WireConnection;250;1;363;0
WireConnection;396;0;4;0
WireConnection;269;0;265;0
WireConnection;269;1;265;0
WireConnection;241;0;192;0
WireConnection;376;0;210;0
WireConnection;376;1;377;0
WireConnection;282;0;250;0
WireConnection;279;0;250;0
WireConnection;279;1;268;0
WireConnection;18;0;396;0
WireConnection;18;1;5;0
WireConnection;389;0;18;0
WireConnection;271;0;267;0
WireConnection;271;1;266;0
WireConnection;281;0;282;0
WireConnection;270;0;279;0
WireConnection;270;1;269;0
WireConnection;209;0;241;0
WireConnection;209;1;210;0
WireConnection;209;2;376;0
WireConnection;272;0;281;0
WireConnection;272;1;270;0
WireConnection;272;2;271;0
WireConnection;214;0;194;0
WireConnection;236;0;209;0
WireConnection;236;1;219;0
WireConnection;275;0;272;0
WireConnection;275;1;216;0
WireConnection;208;0;390;0
WireConnection;208;1;236;0
WireConnection;208;2;214;0
WireConnection;384;0;275;0
WireConnection;398;0;208;0
WireConnection;391;0;192;0
WireConnection;263;0;259;0
WireConnection;243;0;399;0
WireConnection;243;1;385;0
WireConnection;401;0;399;0
WireConnection;0;0;401;0
WireConnection;0;2;243;0
WireConnection;0;10;392;0
ASEEND*/
//CHKSM=94C4AEB7E48FBC10C43D75EBF40A734F8AA2988F