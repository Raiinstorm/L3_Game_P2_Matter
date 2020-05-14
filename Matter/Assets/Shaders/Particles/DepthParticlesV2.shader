// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Vfx/Thibaut/depthParticles"
{
	Properties
	{
		[HDR]_ParticlesColorHDR("ParticlesColor HDR", Color) = (0.3584906,0.3584906,0.3584906,0)
		_MainText("MainText", 2D) = "white" {}
		_CtrlOpacity("CtrlOpacity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Background"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		AlphaToMask On
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ParticlesColorHDR;
		uniform sampler2D _MainText;
		uniform float _CtrlOpacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 tex2DNode58 = tex2D( _MainText, i.uv_texcoord );
			float4 temp_output_59_0 = ( _ParticlesColorHDR + tex2DNode58 );
			o.Albedo = temp_output_59_0.rgb;
			o.Emission = ( temp_output_59_0 * _CtrlOpacity ).rgb;
			o.Alpha = tex2DNode58.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1664.8;502.4;1536;803;-809.997;-148.4646;2.020764;True;True
Node;AmplifyShaderEditor.TexturePropertyNode;53;428.5486,259.7162;Inherit;True;Property;_MainText;MainText;1;0;Create;True;0;0;False;0;163ba977e60d65444a9377c80c60cb1b;163ba977e60d65444a9377c80c60cb1b;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;535.1069,536.9384;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;57;978.6721,279.5362;Inherit;False;Property;_ParticlesColorHDR;ParticlesColor HDR;0;1;[HDR];Create;True;0;0;False;0;0.3584906,0.3584906,0.3584906,0;12.42669,2.610182,14.18868,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;857.2657,519.8959;Inherit;True;Property;_GradientParticles;GradientParticles;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;1414.276,522.3832;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;61;932.022,903.6863;Inherit;False;Property;_CtrlOpacity;CtrlOpacity;2;0;Create;True;0;0;False;0;1;0.69;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;1631.625,688.2045;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;1667.508,893.1351;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2012.229,731.9987;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Vfx/Thibaut/depthParticles;False;False;False;False;False;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Background;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;58;0;53;0
WireConnection;58;1;54;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;62;0;59;0
WireConnection;62;1;61;0
WireConnection;63;1;61;0
WireConnection;0;0;59;0
WireConnection;0;2;62;0
WireConnection;0;9;58;0
ASEEND*/
//CHKSM=A663A004D49A8B94EDA0DA0023340E2A939D8D1A