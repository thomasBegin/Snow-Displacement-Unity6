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


    [Header("Components")]
    [SerializeField] private ComputeShader _snowCompute;
    [SerializeField] private RenderTexture _snowTrailTexture;

    [Header("Properties")]
    [SerializeField] private float _trailRadius;
    [SerializeField] private float _snowFillAmount;
    [SerializeField] private float _snowFillRate;

    [ReadOnly(true)] public int textureWidth => _snowTrailTexture.width;
    [ReadOnly(true)] public int textureHeight => _snowTrailTexture.height;

    private int csmain_kernelID => _snowCompute.FindKernel(CSMAIN_KERNEL);
    private int fillWhite_kernelID => _snowCompute.FindKernel(FILL_WHITE_KERNEL);
    private int drawTrail_kernelID => _snowCompute.FindKernel(DRAW_TRAIL_KERNEL);

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

        this.GetComponent<MeshRenderer>().material.SetTexture("TrailPathTexture", _snowTrailTexture);

        SetComputeShaderProperies();
        SetTrailTextureWhite();

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
        _snowCompute.SetVector(POSITION_PROPERTY, new Vector2());
        _snowCompute.SetFloat(SNOW_FILL_AMOUNT_PROPERTY, _snowFillAmount);
    }

    private void SetTrailTextureWhite()
    {
        _snowCompute.Dispatch(fillWhite_kernelID, textureWidth / 8, textureHeight / 8, 1);
    }

    private IEnumerator AddSnowLayerCoroutine()
    {
        // Add "_snowFillAmount" of snow to the trail texture every "_snowFillRate" seconds

        while (true)
        {
            _snowCompute.Dispatch(csmain_kernelID, textureWidth / 8, textureHeight / 8, 1);
            yield return new WaitForSecondsRealtime(_snowFillRate);
        }
    }
    
    public void DrawSnowTrail()
    {

    }
}
