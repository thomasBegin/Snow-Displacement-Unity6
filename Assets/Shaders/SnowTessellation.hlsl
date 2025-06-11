/*
    Based on tutorial at https://catlikecoding.com/unity/tutorials/advanced-rendering/tessellation/
*/

// Vertex struct
struct Attributes
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
 
};

// Extra vertex struct
struct ControlPoint
{
    float4 vertex : INTERNALTESSPOS;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
};

// Vertex to fragment struct
struct Varyings
{
    float4 vertex : SV_POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
    float4 noise : TEXCOORD1;
};
 
// Tessellation data
struct TessellationFactors
{
    float edge[3] : SV_TessFactor;
    float inside : SV_InsideTessFactor;
};
 
// tessellation variable
float _Tess;
 

// Configuration for Hull Shader
[domain("tri")] // Working with triangles
[outputcontrolpoints(3)] // 3 control points
[outputtopology("triangle_cw")] // Creates triangle clockwise
[partitioning("integer")] // Partitioning method
[patchconstantfunc("PatchConstantFunction")] // Patch function
ControlPoint hull(InputPatch<ControlPoint, 3> patch, uint id : SV_OutputControlPointID)
{
    return patch[id];
}

// Patch function
TessellationFactors PatchConstantFunction(InputPatch<ControlPoint, 3> patch)
{
    TessellationFactors f;
    
    f.edge[0] = _Tess;
    f.edge[1] = _Tess;
    f.edge[2] = _Tess;
    f.inside = _Tess;
    
    return f;
}

// Macro used in domain program
#define Interpolate(fieldName) v.fieldName = \
				patch[0].fieldName * barycentricCoords.x + \
				patch[1].fieldName * barycentricCoords.y + \
				patch[2].fieldName * barycentricCoords.z;
