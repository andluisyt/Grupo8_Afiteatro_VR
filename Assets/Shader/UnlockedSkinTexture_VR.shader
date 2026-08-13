Shader "Custom/UnlockedSkinTexture_VR_Solid"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white" {}

        _Radius("Radius", Range(0.0, 5.0)) = 0.5
        _CenterPoint("Center", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalPipeline"
        }

        LOD 200

        Cull Back
        ZWrite On
        ZTest LEqual

        Pass
        {
            Name "Forward"

            Tags
            {
                "LightMode"="UniversalForward"
            }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            half4 _Color;
            float4 _CenterPoint;
            float _Radius;

            CBUFFER_END


            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs positionInputs =
                    GetVertexPositionInputs(
                        IN.positionOS.xyz
                    );

                OUT.positionHCS =
                    positionInputs.positionCS;

                OUT.worldPos =
                    positionInputs.positionWS;

                OUT.uv =
                    TRANSFORM_TEX(
                        IN.uv,
                        _MainTex
                    );

                return OUT;
            }


            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                // Distancia entre este píxel de la piel
                // y AR_Window.
                float distancia =
                    distance(
                        IN.worldPos,
                        _CenterPoint.xyz
                    );

                // Dentro del radio se elimina el píxel.
                clip(
                    distancia - _Radius
                );


                // Fuera del agujero:
                // piel completamente sólida.
                half4 textura =
                    SAMPLE_TEXTURE2D(
                        _MainTex,
                        sampler_MainTex,
                        IN.uv
                    );

                textura *= _Color;

                return half4(
                    textura.rgb,
                    1.0
                );
            }

            ENDHLSL
        }
    }

    FallBack Off
}