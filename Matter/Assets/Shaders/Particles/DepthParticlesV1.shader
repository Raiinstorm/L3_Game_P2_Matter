// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ZdepthParticles"
{
	Properties
	{
		_TransitionFallOff("TransitionFallOff", Float) = 12
		[HDR]_ParticlesColorHDR("ParticlesColor HDR", Color) = (0.3584906,0.3584906,0.3584906,0)
		_TransitionDistance("TransitionDistance", Float) = 8
		_MainText("MainText", 2D) = "white" {}
		_CtrlOpacity("CtrlOpacity", Range( 0 , 1)) = 0
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
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _ParticlesColorHDR;
		uniform sampler2D _MainText;
		uniform float _CtrlOpacity;
		uniform float4 _MainText_ST;
		uniform float _TransitionDistance;
		uniform float _TransitionFallOff;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 temp_output_59_0 = ( _ParticlesColorHDR + tex2D( _MainText, i.uv_texcoord ) );
			o.Albedo = temp_output_59_0.rgb;
			o.Emission = ( temp_output_59_0 * _CtrlOpacity ).rgb;
			float2 uv_MainText = i.uv_texcoord * _MainText_ST.xy + _MainText_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float clampResult64 = clamp( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / _TransitionDistance ) , -_TransitionFallOff ) , 0.0 , 1.0 );
			o.Alpha = ( tex2D( _MainText, uv_MainText ) * clampResult64 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;0;1536;803;1847.685;298.6139;3.192513;True;False
Node;AmplifyShaderEditor.WorldSpaceCameraPos;65;-40.57747,1045.516;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;48;23.4225,853.5143;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;49;311.4225,1157.516;Inherit;False;Property;_TransitionDistance;TransitionDistance;2;0;Create;True;0;0;False;0;8;8.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;50;311.4225,917.5143;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;666.3035,1128.004;Inherit;False;Property;_TransitionFallOff;TransitionFallOff;0;0;Create;True;0;0;False;0;12;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;51;647.4221,997.515;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;52;843.1766,1067.797;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;53;428.5486,259.7162;Inherit;True;Property;_MainText;MainText;3;0;Create;True;0;0;False;0;163ba977e60d65444a9377c80c60cb1b;163ba977e60d65444a9377c80c60cb1b;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;54;535.1069,536.9384;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NegateNode;66;889.6125,1127.986;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;58;857.2657,519.8959;Inherit;True;Property;_GradientParticles;GradientParticles;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;1090.891,1219.399;Inherit;False;Constant;_min;min;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;1096.462,1307.126;Inherit;False;Constant;_max;max;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;56;1096.91,1046.935;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;57;978.6721,279.5362;Inherit;False;Property;_ParticlesColorHDR;ParticlesColor HDR;1;1;[HDR];Create;True;0;0;False;0;0.3584906,0.3584906,0.3584906,0;108.9255,103.7971,15.39784,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;60;863.2673,733.2473;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Instance;58;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;1414.276,522.3832;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;64;1346.756,1095.968;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;1274.763,703.0912;Inherit;False;Property;_CtrlOpacity;CtrlOpacity;4;0;Create;True;0;0;False;0;0;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;1647.73,974.3353;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;1631.625,688.2045;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2012.229,731.9987;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ZdepthParticles;False;False;False;False;False;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Background;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;5;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;50;0;48;0
WireConnection;50;1;65;0
WireConnection;51;0;50;0
WireConnection;51;1;49;0
WireConnection;52;0;51;0
WireConnection;66;0;55;0
WireConnection;58;0;53;0
WireConnection;58;1;54;0
WireConnection;56;0;52;0
WireConnection;56;1;66;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;64;0;56;0
WireConnection;64;1;67;0
WireConnection;64;2;68;0
WireConnection;63;0;60;0
WireConnection;63;1;64;0
WireConnection;62;0;59;0
WireConnection;62;1;61;0
WireConnection;0;0;59;0
WireConnection;0;2;62;0
WireConnection;0;9;63;0
ASEEND*/
//CHKSM=0D5C662D3134A35B55E75F3864EA72DC96C1A7E4