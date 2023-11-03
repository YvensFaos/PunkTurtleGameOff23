using NaughtyAttributes;
using UnityEngine;

public class AddTextureWithComputeShader : MonoBehaviour
{
    [SerializeField]
    private ComputeShader addTexturesCompute;
    [SerializeField, ShowAssetPreview(128, 128)]
    private RenderTexture resultTexture;
    [SerializeField, ShowAssetPreview(128, 128)]
    private RenderTexture inputTexture1;
    [SerializeField, ShowAssetPreview(128, 128)]
    private RenderTexture inputTexture2;
    [SerializeField, ReadOnly, ShowAssetPreview(128, 128)]
    private RenderTexture cyclicalTexture;
    [SerializeField, ReadOnly]
    private int kernelID;

    private void Awake()
    {
        GenerateCyclicalTexture();
    }
    
    private void LateUpdate()
    {
        ForceRender();
    }
    
    [Button("Force Render")]
    private void ForceRender()
    {
        if (kernelID == 0)
        {
            kernelID = addTexturesCompute.FindKernel("CSMain");
        }

        GenerateCyclicalTexture();

        addTexturesCompute.SetTexture(kernelID, "Result", cyclicalTexture);
        addTexturesCompute.SetTexture(kernelID, "InputTexture1", inputTexture1);
        addTexturesCompute.SetTexture(kernelID, "InputTexture2", inputTexture2);
        addTexturesCompute.Dispatch(kernelID, cyclicalTexture.width / 8, cyclicalTexture.height / 8, 1);
        
        Graphics.Blit(cyclicalTexture, resultTexture);
    }

    private void GenerateCyclicalTexture()
    {
        if (cyclicalTexture == null)
        {
            ForceRegenerateCyclicalTexture();
        }
    }

    [Button("Force New Texture")]
    private void ForceRegenerateCyclicalTexture()
    {
        cyclicalTexture = new RenderTexture(resultTexture.width, resultTexture.height, resultTexture.depth,
            resultTexture.format)
        {
            enableRandomWrite = true,
            name = "Cyclical Texture"
        };
        cyclicalTexture.Create();
    }

    [Button("Delete Texture")]
    private void DeleteTexture()
    {
        cyclicalTexture = null;
    }
}
