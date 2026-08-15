Shader "Custom/UnlockedSkinTexture_VR_Depth"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white" {}

        _Radius("Radius", Range(0.01, 1.0)) = 0.15
        _CenterPoint("Center", Vector) = (0,0,0,0)

        _CutDepth("Profundidad del corte", Range(0.005, 0.25)) = 0.03
        _SurfaceTolerance("Tolerancia superficie", Range(0.0, 0.10)) = 0.02

        // BORDE ROJO
        _EdgeColor("Color del borde", Color) = (0.55, 0.05, 0.035, 1)
        _EdgeWidth("Grosor del borde", Range(0.001, 0.05)) = 0.012
        _EdgeStrength("Intensidad del borde", Range(0.0, 1.0)) = 0.85
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

        Cull Off
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
                float _CutDepth;
                float _SurfaceTolerance;

                half4 _EdgeColor;
                float _EdgeWidth;
                float _EdgeStrength;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs positionInputs =
                    GetVertexPositionInputs(IN.positionOS.xyz);

                OUT.positionHCS = positionInputs.positionCS;
                OUT.worldPos = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                tex *= _Color;

                float3 cameraPosition = GetCameraPositionWS();

                // Dirección desde la cámara al centro de la disección
                float3 cutDirection = normalize(_CenterPoint.xyz - cameraPosition);

                // Vector desde el centro de la disección al fragmento
                float3 delta = IN.worldPos - _CenterPoint.xyz;

                // Profundidad del fragmento a lo largo del eje del corte
                float depth = dot(delta, cutDirection);

                // Distancia lateral respecto al eje del corte
                float3 lateralOffset = delta - cutDirection * depth;
                float lateralDistance = length(lateralOffset);

                // -----------------------------
                // MÁSCARA DE CORTE
                // -----------------------------
                float radiusCondition = lateralDistance - _Radius;
                float frontCondition = -_SurfaceTolerance - depth;
                float depthCondition = depth - _CutDepth;

                float cutMask = max(radiusCondition, max(frontCondition, depthCondition));

                // -----------------------------
                // BORDE ROJO ALREDEDOR DEL CORTE
                // -----------------------------
                // Anillo desde _Radius hasta _Radius + _EdgeWidth
                float safeEdgeWidth = max(_EdgeWidth, 0.0001);
                float edgeT = saturate((lateralDistance - _Radius) / safeEdgeWidth);

                // Solo fuera del radio del agujero
                float edgeRing = (1.0 - edgeT) * step(_Radius, lateralDistance);

                // Limitar el borde a la zona superficial para que no pinte cosas raras en profundidad
                float safeTolerance = max(_SurfaceTolerance, 0.0001);
                float surfaceMask = 1.0 - smoothstep(0.0, safeTolerance, abs(depth));

                float finalEdgeMask = saturate(edgeRing * surfaceMask * _EdgeStrength);

                // Primero recortamos la disección
                clip(cutMask);

                // Luego teñimos de rojo el borde visible
                tex.rgb = lerp(tex.rgb, _EdgeColor.rgb, finalEdgeMask);

                return half4(tex.rgb, 1.0);
            }

            ENDHLSL
        }
    }

    FallBack Off
}