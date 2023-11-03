using NaughtyAttributes;
using UnityEngine;
using Utils;

public class SwapRenderToTexture : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;
    [SerializeField]
    private RenderTexture textureA;
    [SerializeField]
    private RenderTexture textureB;
    [SerializeField, ReadOnly]
    private bool internalSwapTexture;

    private void Awake()
    {
        AssessUtils.CheckRequirement(ref targetCamera, this);
        AssessUtils.CheckRequirement(ref textureA, this);
        AssessUtils.CheckRequirement(ref textureB, this);
    }
    
    private void LateUpdate()
    {
        internalSwapTexture = !internalSwapTexture;
        ForceRenderTo(internalSwapTexture ? textureA : textureB);
    }

    [Button("Force Render to A")]
    private void ForceRenderToA()
    {
        ForceRenderTo(textureA);
    }

    [Button("Force Render to B")]
    private void ForceRenderToB()
    {
        ForceRenderTo(textureB);
    }
    
    private void ForceRenderTo(RenderTexture texture)
    {
        var current = targetCamera.targetTexture;
        targetCamera.targetTexture = texture;
        targetCamera.Render();
        targetCamera.targetTexture = current;
    }
}
