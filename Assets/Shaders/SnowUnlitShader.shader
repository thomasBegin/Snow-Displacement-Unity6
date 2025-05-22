
Shader "Custom/SnowUnlitShader"
{
    Properties
    {
        _Tess ("Tessellation factor", Range(1, 20)) = 10
        _PathText ("Trail Path Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderingPipeline" = "UniversalRendringPipeline"
        }

        Pass
        {
            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "SnowTessellation.hlsl"

            #pragma hull hull
            #pragma domain domain
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _PathText;

            // Before tessellation vert program
            ControlPoint vert(Attributes v)
            {
                ControlPoint p;

                p.vertex = v.vertex;
                p.normal = v.normal;
                p.uv = v.uv;
                p.color = v.color;

                return p;
            }

            // After tessellation vert program
            Varyings tessVert(Attributes v) 
            {
                Varyings output;

                output.vertex = TransformObjectToHClip(v.vertex.xyz);
                output.normal = v.normal;
                output.uv = v.uv;
                output.color = v.color;

                return output; 
            }

            // Domain program (compute actual vertex position after tessellation)
            [domain("tri")]
            Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoords : SV_DomainLocation) 
            {
                Attributes v;

                Interpolate(vertex)
                Interpolate(normal)
                Interpolate(uv)
                Interpolate(color)

                return tessVert(v);
            }

            // Frag program
            half4 frag(Varyings f) : SV_Target
            {
                return tex2D(_PathText, f.uv);
            }

            ENDHLSL
        }
    }
}
