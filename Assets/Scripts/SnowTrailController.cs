using System.Collections;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SnowTrailController : MonoBehaviour
{
    public static SnowTrailController Instance = null;

    // SnowComputeShader Properties names
    private const string CSMAIN_KERNEL = "CSMain";
    private const string FILL_WHITE_KERNEL = "FillWhite";
    private const string DRAW_TRAIL_KERNEL = "DrawTrail";
    private const string SNOW_TEXTURE_PROPERTY = "SnowTexture";
    private const string TRAIL_RADIUS_PROPERTY = "TrailRadius";
    private const string POSITION_PROPERTY = "Position";
    private const string SNOW_FILL_AMOUNT_PROPERTY = "SnowFillAmount";
    private const string RESOLUTION_PROPERTY = "Resolution";
    
    // SnowComputeShader kernels
    [ReadOnly(true)] private int csmain_kernelID => _snowCompute.FindKernel(CSMAIN_KERNEL);
    [ReadOnly(true)] private int fillWhite_kernelID => _snowCompute.FindKernel(FILL_WHITE_KERNEL);
    [ReadOnly(true)] private int drawTrail_kernelID => _snowCompute.FindKernel(DRAW_TRAIL_KERNEL);


    [Header("Components")]
    [SerializeField] private ComputeShader _snowCompute;
    [SerializeField] private RenderTexture _snowTrailTexture;
    [SerializeField] private MeshRenderer _goMesh;

    [Header("Properties")]
    [SerializeField] private float _trailRadius;
    [SerializeField] private float _snowFillAmount;
    [SerializeField] private float _snowFillRate;

    private int _textureRes => _snowTrailTexture.width; // Needs to be a square texture (TODO: Any size texture)
    private float _goWidth => _goMesh.bounds.size.x; // Needs to be a square GO (TODO: Any size GO)

    private Vector2 _trailPos;

    private IEnumerator _snowFillCoroutine;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SnowControllerInstance is already defined");
        }
        else
        {
            Instance = this;
        }

        // Sets up trail texture on GO to view path
        _goMesh.material.SetTexture("TrailPathTexture", _snowTrailTexture);

        //Sets up compute shader and texture
        SetComputeShaderProperies();
        SetTrailTextureWhite();

        //Starts coroutine to fill the trail with snow
        _snowFillCoroutine = AddSnowLayerCoroutine();
        StartCoroutine(_snowFillCoroutine);
    }

    private void SetComputeShaderProperies()
    {
        // Snow trail texture
        _snowCompute.SetTexture(csmain_kernelID, SNOW_TEXTURE_PROPERTY, _snowTrailTexture);
        _snowCompute.SetTexture(fillWhite_kernelID, SNOW_TEXTURE_PROPERTY, _snowTrailTexture);
        _snowCompute.SetTexture(drawTrail_kernelID, SNOW_TEXTURE_PROPERTY, _snowTrailTexture);

        // Other properties
        _snowCompute.SetFloat(TRAIL_RADIUS_PROPERTY, _trailRadius);
        _snowCompute.SetFloat(SNOW_FILL_AMOUNT_PROPERTY, _snowFillAmount);
        _snowCompute.SetFloat(RESOLUTION_PROPERTY, _textureRes);
    }

    private void SetTrailTextureWhite()
    {
        // Sets up trail texture as all white
        _snowCompute.Dispatch(fillWhite_kernelID, _textureRes / 8, _textureRes / 8, 1);
    }

    private IEnumerator AddSnowLayerCoroutine()
    {
        // Adds "_snowFillAmount" of snow to the trail texture every "_snowFillRate" seconds

        while (true)
        {
            _snowCompute.Dispatch(csmain_kernelID, _textureRes / 8, _textureRes / 8, 1);
            yield return new WaitForSecondsRealtime(_snowFillRate);
        }
    }
    
    public void DrawSnowTrail(Vector3 pos)
    {
        // Translates "pos" from world space to position on texture
        _trailPos.x = (_textureRes / 2) - (((pos.x - transform.position.x) * (_textureRes / 2)) / (_goWidth / 2));
        _trailPos.y = (_textureRes / 2) - (((pos.z - transform.position.z) * (_textureRes / 2)) / (_goWidth / 2));

        // Calls draw trail method from compute shader
        _snowCompute.SetVector(POSITION_PROPERTY, _trailPos);
        _snowCompute.Dispatch(drawTrail_kernelID, _textureRes / 8, _textureRes / 8, 1);
    }
}
