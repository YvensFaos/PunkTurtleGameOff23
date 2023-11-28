using Spine.Unity;
using UnityEngine;

namespace Core
{
    public class ChangeSpineMaterialTo : MonoBehaviour
    {
        [SerializeField] 
        private SkeletonAnimation playerSkeletonAnimator;
        [SerializeField]
        private Material changeTo;
        [SerializeField]
        private bool onStart;

        private void Start()
        {
            if (onStart)
            {
                ChangeMaterialTo(changeTo);    
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void ChangeMaterialTo(Material material)
        {
            var originalMaterial = playerSkeletonAnimator.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial;
            playerSkeletonAnimator.CustomMaterialOverride[originalMaterial] = material;
        }
    }
}