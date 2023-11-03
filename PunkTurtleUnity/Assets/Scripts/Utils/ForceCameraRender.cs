using NaughtyAttributes;
using UnityEngine;

namespace Utils
{
    public class ForceCameraRender : MonoBehaviour
    {
        [SerializeField]
        private Camera forceCamera;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref forceCamera, this);
        }

        [Button("Force")]
        public void Force()
        {
            forceCamera.Render();
        }
    }
}