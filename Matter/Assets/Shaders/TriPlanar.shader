// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriPlanar"
{
	Properties
	{
		[NoScaleOffset]_MainTex1("Albedo", 2D) = "white" {}
		[NoScaleOffset]_MainTex2("Normal", 2D) = "white" {}
		[NoScaleOffset]_MainTex3("MetallicGlossiness", 2D) = "white" {}
		_Hardness("Hardness", Range( 0 , 1)) = 0
		_UVControl("UV Control", Vector) = (1,1,0,0)
		_ScaleNormal("Scale Normal", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
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
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _MainTex2;
		uniform float4 _UVControl;
		uniform float _Hardness;
		uniform float _ScaleNormal;
		uniform sampler2D _MainTex1;
		uniform sampler2D _MainTex3;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 break1_g4 = _UVControl;
			float2 appendResult4_g4 = (float2(break1_g4.x , break1_g4.y));
			float3 ase_worldPos = i.worldPos;
			float3 break2_g4 = ase_worldPos;
			float2 appendResult6_g4 = (float2(break2_g4.y , break2_g4.z));
			float2 appendResult10_g4 = (float2(break1_g4.z , break1_g4.w));
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_cast_0 = ((5.0 + (_Hardness - 0.0) * (100.0 - 5.0) / (1.0 - 0.0))).xxx;
			float3 temp_output_27_0_g4 = pow( abs( ase_worldNormal ) , temp_cast_0 );
			float3 break28_g4 = temp_output_27_0_g4;
			float3 break18_g4 = ( temp_output_27_0_g4 / ( break28_g4.x + break28_g4.y + break28_g4.z ) );
			float2 appendResult5_g4 = (float2(break2_g4.x , break2_g4.z));
			float2 appendResult3_g4 = (float2(break2_g4.x , break2_g4.y));
			o.Normal = UnpackScaleNormal( ( ( tex2D( _MainTex2, ( ( appendResult4_g4 * appendResult6_g4 ) + appendResult10_g4 ) ) * break18_g4.x ) + ( tex2D( _MainTex2, ( ( appendResult4_g4 * appendResult5_g4 ) + appendResult10_g4 ) ) * break18_g4.y ) + ( tex2D( _MainTex2, ( ( appendResult3_g4 * appendResult4_g4 ) + appendResult10_g4 ) ) * break18_g4.z ) ), _ScaleNormal );
			float4 break1_g3 = _UVControl;
			float2 appendResult4_g3 = (float2(break1_g3.x , break1_g3.y));
			float3 break2_g3 = ase_worldPos;
			float2 appendResult6_g3 = (float2(break2_g3.y , break2_g3.z));
			float2 appendResult10_g3 = (float2(break1_g3.z , break1_g3.w));
			float3 temp_cast_2 = ((5.0 + (_Hardness - 0.0) * (100.0 - 5.0) / (1.0 - 0.0))).xxx;
			float3 temp_output_27_0_g3 = pow( abs( ase_worldNormal ) , temp_cast_2 );
			float3 break28_g3 = temp_output_27_0_g3;
			float3 break18_g3 = ( temp_output_27_0_g3 / ( break28_g3.x + break28_g3.y + break28_g3.z ) );
			float2 appendResult5_g3 = (float2(break2_g3.x , break2_g3.z));
			float2 appendResult3_g3 = (float2(break2_g3.x , break2_g3.y));
			o.Albedo = ( ( tex2D( _MainTex1, ( ( appendResult4_g3 * appendResult6_g3 ) + appendResult10_g3 ) ) * break18_g3.x ) + ( tex2D( _MainTex1, ( ( appendResult4_g3 * appendResult5_g3 ) + appendResult10_g3 ) ) * break18_g3.y ) + ( tex2D( _MainTex1, ( ( appendResult3_g3 * appendResult4_g3 ) + appendResult10_g3 ) ) * break18_g3.z ) ).rgb;
			float4 break1_g5 = _UVControl;
			float2 appendResult4_g5 = (float2(break1_g5.x , break1_g5.y));
			float3 break2_g5 = ase_worldPos;
			float2 appendResult6_g5 = (float2(break2_g5.y , break2_g5.z));
			float2 appendResult10_g5 = (float2(break1_g5.z , break1_g5.w));
			float3 temp_cast_4 = ((5.0 + (_Hardness - 0.0) * (100.0 - 5.0) / (1.0 - 0.0))).xxx;
			float3 temp_output_27_0_g5 = pow( abs( ase_worldNormal ) , temp_cast_4 );
			float3 break28_g5 = temp_output_27_0_g5;
			float3 break18_g5 = ( temp_output_27_0_g5 / ( break28_g5.x + break28_g5.y + break28_g5.z ) );
			float2 appendResult5_g5 = (float2(break2_g5.x , break2_g5.z));
			float2 appendResult3_g5 = (float2(break2_g5.x , break2_g5.y));
			float4 break54 = ( ( tex2D( _MainTex3, ( ( appendResult4_g5 * appendResult6_g5 ) + appendResult10_g5 ) ) * break18_g5.x ) + ( tex2D( _MainTex3, ( ( appendResult4_g5 * appendResult5_g5 ) + appendResult10_g5 ) ) * break18_g5.y ) + ( tex2D( _MainTex3, ( ( appendResult3_g5 * appendResult4_g5 ) + appendResult10_g5 ) ) * break18_g5.z ) );
			o.Metallic = break54;
			o.Smoothness = break54.a;
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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
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
236;173;1920;938;2980.784;534.0043;2.063514;True;True
Node;AmplifyShaderEditor.Vector4Node;36;-2681.928,-18.59655;Inherit;False;Property;_UVControl;UV Control;4;0;Create;True;0;0;False;0;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-2682.249,150.9877;Inherit;False;Property;_Hardness;Hardness;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-2676.194,238.4179;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;40;-2665.896,394.2262;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;48;-2247.686,-108.3559;Inherit;True;Property;_MainTex2;Normal;1;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;50;-2322.391,716.942;Inherit;True;Property;_MainTex3;MetallicGlossiness;2;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;47;-1553.165,418.9271;Inherit;False;TriPlanar_Function;-1;;4;9de942f2c326e824b89e72f8f7650899;0;5;40;SAMPLER2D;0,0,0,0;False;35;FLOAT4;0,0,0,0;False;38;FLOAT;0;False;37;FLOAT3;0,0,0;False;39;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;49;-1655.049,752.5433;Inherit;False;TriPlanar_Function;-1;;5;9de942f2c326e824b89e72f8f7650899;0;5;40;SAMPLER2D;0,0,0,0;False;35;FLOAT4;0,0,0,0;False;38;FLOAT;0;False;37;FLOAT3;0,0,0;False;39;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1234.341,496.757;Inherit;False;Property;_ScaleNormal;Scale Normal;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;15;-2257.075,-324.9359;Inherit;True;Property;_MainTex1;Albedo;0;1;[NoScaleOffset];Create;False;0;0;False;0;None;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;37;-1534.669,-24.20228;Inherit;False;TriPlanar_Function;-1;;3;9de942f2c326e824b89e72f8f7650899;0;5;40;SAMPLER2D;0,0,0,0;False;35;FLOAT4;0,0,0,0;False;38;FLOAT;0;False;37;FLOAT3;0,0,0;False;39;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;51;-1071.362,413.5699;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;54;-1282.543,728.8665;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TriPlanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;47;40;48;0
WireConnection;47;35;36;0
WireConnection;47;38;38;0
WireConnection;47;37;39;0
WireConnection;47;39;40;0
WireConnection;49;40;50;0
WireConnection;49;35;36;0
WireConnection;49;38;38;0
WireConnection;49;37;39;0
WireConnection;49;39;40;0
WireConnection;37;40;15;0
WireConnection;37;35;36;0
WireConnection;37;38;38;0
WireConnection;37;37;39;0
WireConnection;37;39;40;0
WireConnection;51;0;47;0
WireConnection;51;1;52;0
WireConnection;54;0;49;0
WireConnection;0;0;37;0
WireConnection;0;1;51;0
WireConnection;0;3;54;0
WireConnection;0;4;54;3
ASEEND*/
//CHKSM=019C6686F2CC41716B3056683AFED2833A959143