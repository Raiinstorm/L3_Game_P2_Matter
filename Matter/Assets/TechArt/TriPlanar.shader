// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriPlanar"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[NoScaleOffset]_MainTex1("DetailPlanar", 2D) = "white" {}
		_ColorAlbedo("Color Albedo", Color) = (0.5660378,0.5660378,0.5660378,0)
		_Hardness("Hardness", Range( 0 , 1)) = 0
		_UVControl("UV Control", Vector) = (1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _MainTex;
		uniform sampler2D _MainTex1;
		uniform float4 _UVControl;
		uniform float _Hardness;
		uniform float4 _ColorAlbedo;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 break1_g3 = _UVControl;
			float2 appendResult4_g3 = (float2(break1_g3.x , break1_g3.y));
			float3 ase_worldPos = i.worldPos;
			float3 break2_g3 = ase_worldPos;
			float2 appendResult6_g3 = (float2(break2_g3.y , break2_g3.z));
			float2 appendResult10_g3 = (float2(break1_g3.z , break1_g3.w));
			float3 ase_worldNormal = i.worldNormal;
			float3 temp_cast_0 = ((5.0 + (_Hardness - 0.0) * (100.0 - 5.0) / (1.0 - 0.0))).xxx;
			float3 temp_output_27_0_g3 = pow( abs( ase_worldNormal ) , temp_cast_0 );
			float3 break28_g3 = temp_output_27_0_g3;
			float3 break18_g3 = ( temp_output_27_0_g3 / ( break28_g3.x + break28_g3.y + break28_g3.z ) );
			float2 appendResult5_g3 = (float2(break2_g3.x , break2_g3.z));
			float2 appendResult3_g3 = (float2(break2_g3.x , break2_g3.y));
			o.Albedo = ( tex2D( _MainTex, i.uv_texcoord ) * ( 2.0 * ( ( tex2D( _MainTex1, ( ( appendResult4_g3 * appendResult6_g3 ) + appendResult10_g3 ) ) * break18_g3.x ) + ( tex2D( _MainTex1, ( ( appendResult4_g3 * appendResult5_g3 ) + appendResult10_g3 ) ) * break18_g3.y ) + ( tex2D( _MainTex1, ( ( appendResult3_g3 * appendResult4_g3 ) + appendResult10_g3 ) ) * break18_g3.z ) ) ) * _ColorAlbedo ).rgb;
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
118.4;738.4;1536;519;3721.592;846.4241;2.41409;True;False
Node;AmplifyShaderEditor.RangedFloatNode;38;-2282.537,61.40497;Inherit;False;Property;_Hardness;Hardness;3;0;Create;True;0;0;False;0;0;0.15;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;40;-1988.676,484.1229;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;39;-2252.679,248.4065;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;36;-2681.928,-18.59655;Inherit;False;Property;_UVControl;UV Control;4;0;Create;True;0;0;False;0;1,1,0,0;0.1,0.1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;15;-2502.134,-281.7192;Inherit;True;Property;_MainTex1;DetailPlanar;1;1;[NoScaleOffset];Create;False;0;0;False;0;None;62be4d3b0c28ee741a76ee265799e4d9;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-2914.425,-348.7269;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;41;-2838.542,-631.285;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;None;da1b837a635e4954c8bf5c13c6f018ca;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1337.252,-186.0188;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;37;-1845.683,-47.64681;Inherit;True;TriPlanar_Function;-1;;3;9de942f2c326e824b89e72f8f7650899;0;5;40;SAMPLER2D;0,0,0,0;False;35;FLOAT4;0,0,0,0;False;38;FLOAT;0;False;37;FLOAT3;0,0,0;False;39;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-2124.146,-511.0547;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;47;-1059.224,-439.5737;Inherit;False;Property;_ColorAlbedo;Color Albedo;2;0;Create;True;0;0;False;0;0.5660378,0.5660378,0.5660378,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1151.128,-99.25605;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-622.6017,-125.3224;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-151.1677,-317.4522;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TriPlanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;40;15;0
WireConnection;37;35;36;0
WireConnection;37;38;38;0
WireConnection;37;37;39;0
WireConnection;37;39;40;0
WireConnection;42;0;41;0
WireConnection;42;1;43;0
WireConnection;45;0;46;0
WireConnection;45;1;37;0
WireConnection;44;0;42;0
WireConnection;44;1;45;0
WireConnection;44;2;47;0
WireConnection;0;0;44;0
ASEEND*/
//CHKSM=AF7694E829F1C601CFE360B20EA0EFDDBA058FE2