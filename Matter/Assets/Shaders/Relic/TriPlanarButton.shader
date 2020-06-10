// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriPlanar"
{
	Properties
	{
		[NoScaleOffset]_MainTex1("Albedo", 2D) = "white" {}
		[NoScaleOffset]_TextureUVEmissive("Texture UV Emissive", 2D) = "white" {}
		_UVControl("UV Control", Vector) = (1,1,0,0)
		_Hardness("Hardness", Range( 0 , 1)) = 0
		_ColorRelic("Color Relic", Color) = (0.745283,0.7206746,0.7206746,0)
		[HDR]_ColorHDR("Color HDR", Color) = (3.090196,1.270588,2.964706,0)
		_ColorEmissive("Color Emissive", Float) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float4 _ColorRelic;
		uniform sampler2D _MainTex1;
		uniform float4 _UVControl;
		uniform float _Hardness;
		uniform sampler2D _TextureUVEmissive;
		uniform float _ColorEmissive;
		uniform float4 _ColorHDR;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 break1_g6 = _UVControl;
			float2 appendResult4_g6 = (float2(break1_g6.x , break1_g6.y));
			float3 ase_worldPos = i.worldPos;
			float3 break2_g6 = ase_worldPos;
			float2 appendResult6_g6 = (float2(break2_g6.y , break2_g6.z));
			float2 appendResult10_g6 = (float2(break1_g6.z , break1_g6.w));
			float3 ase_worldNormal = i.worldNormal;
			float3 temp_cast_0 = ((5.0 + (_Hardness - 0.0) * (100.0 - 5.0) / (1.0 - 0.0))).xxx;
			float3 temp_output_27_0_g6 = pow( abs( ase_worldNormal ) , temp_cast_0 );
			float3 break28_g6 = temp_output_27_0_g6;
			float3 break18_g6 = ( temp_output_27_0_g6 / ( break28_g6.x + break28_g6.y + break28_g6.z ) );
			float2 appendResult5_g6 = (float2(break2_g6.x , break2_g6.z));
			float2 appendResult3_g6 = (float2(break2_g6.x , break2_g6.y));
			half3 gammaToLinear57 = ( _ColorRelic * ( ( tex2D( _MainTex1, ( ( appendResult4_g6 * appendResult6_g6 ) + appendResult10_g6 ) ) * break18_g6.x ) + ( tex2D( _MainTex1, ( ( appendResult4_g6 * appendResult5_g6 ) + appendResult10_g6 ) ) * break18_g6.y ) + ( tex2D( _MainTex1, ( ( appendResult3_g6 * appendResult4_g6 ) + appendResult10_g6 ) ) * break18_g6.z ) ) ).rgb;
			gammaToLinear57 = half3( GammaToLinearSpaceExact(gammaToLinear57.r), GammaToLinearSpaceExact(gammaToLinear57.g), GammaToLinearSpaceExact(gammaToLinear57.b) );
			o.Albedo = gammaToLinear57;
			float2 uv_TextureUVEmissive59 = i.uv_texcoord;
			half3 gammaToLinear61 = ( ( tex2D( _TextureUVEmissive, uv_TextureUVEmissive59 ) * _ColorEmissive ) * _ColorHDR ).rgb;
			gammaToLinear61 = half3( GammaToLinearSpaceExact(gammaToLinear61.r), GammaToLinearSpaceExact(gammaToLinear61.g), GammaToLinearSpaceExact(gammaToLinear61.b) );
			o.Emission = gammaToLinear61;
			o.Alpha = 1;
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
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
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
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
0;0;1920;1019;2408.69;-107.8658;1;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;58;-2033.981,323.9273;Inherit;True;Property;_TextureUVEmissive;Texture UV Emissive;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;812b9396a629bb44f90717ac5cb0c608;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector4Node;36;-2681.928,-18.59655;Inherit;False;Property;_UVControl;UV Control;2;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-2682.249,150.9877;Inherit;False;Property;_Hardness;Hardness;3;0;Create;True;0;0;False;0;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;59;-1753.559,371.078;Inherit;True;Property;_textEmissive;textEmissive;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;63;-1624.018,620.8638;Inherit;False;Property;_ColorEmissive;Color Emissive;6;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;40;-2665.896,394.2262;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-2676.194,238.4179;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;15;-2257.075,-324.9359;Inherit;True;Property;_MainTex1;Albedo;0;1;[NoScaleOffset];Create;False;0;0;False;0;None;62be4d3b0c28ee741a76ee265799e4d9;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;37;-1534.669,-24.20228;Inherit;False;TriPlanar_Function;-1;;6;9de942f2c326e824b89e72f8f7650899;0;5;40;SAMPLER2D;0,0,0,0;False;35;FLOAT4;0,0,0,0;False;38;FLOAT;0;False;37;FLOAT3;0,0,0;False;39;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;60;-1357.495,593.3798;Inherit;False;Property;_ColorHDR;Color HDR;5;1;[HDR];Create;True;0;0;False;0;3.090196,1.270588,2.964706,0;0.7542472,0.604507,1.059274,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;55;-1493.915,-244.0074;Inherit;False;Property;_ColorRelic;Color Relic;4;0;Create;True;0;0;False;0;0.745283,0.7206746,0.7206746,0;0.8018868,0.7527145,0.7527145,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1341.648,449.3514;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-1129.119,-77.73929;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1068.595,457.5189;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GammaToLinearNode;61;-871.8319,455.9571;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GammaToLinearNode;57;-938.0335,-70.29446;Inherit;False;1;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-391.1383,139.1725;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TriPlanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;59;0;58;0
WireConnection;37;40;15;0
WireConnection;37;35;36;0
WireConnection;37;38;38;0
WireConnection;37;37;39;0
WireConnection;37;39;40;0
WireConnection;64;0;59;0
WireConnection;64;1;63;0
WireConnection;56;0;55;0
WireConnection;56;1;37;0
WireConnection;62;0;64;0
WireConnection;62;1;60;0
WireConnection;61;0;62;0
WireConnection;57;0;56;0
WireConnection;0;0;57;0
WireConnection;0;2;61;0
ASEEND*/
//CHKSM=DF4947A8EE97EA40901D1ECFABD0E018D72ACCA6