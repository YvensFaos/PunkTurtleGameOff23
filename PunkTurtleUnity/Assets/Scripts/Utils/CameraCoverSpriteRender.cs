using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public class CameraCoverSpriteRender : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer targetSpriteRenderer;
        [SerializeField] 
        private TilemapRenderer targetTilemapRenderer;
        [SerializeField]
        private Camera cameraToAdjust;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref targetSpriteRenderer, this);
            AssessUtils.CheckRequirement(ref cameraToAdjust, this);
        }

        [Button("Fit Sprite")]
        public void FitSprite()
        {
            if (targetSpriteRenderer == null || cameraToAdjust == null)
            {
                DebugUtils.DebugLogErrorMsg("Please assign the target Sprite Renderer and the Camera to adjust.");
                return;
            }

            var spriteBounds = targetSpriteRenderer.bounds;
            var spriteSize = Mathf.Max(spriteBounds.size.x, spriteBounds.size.y);
            var cameraSize = spriteSize * 0.5f;

            cameraToAdjust.orthographic = true;
            cameraToAdjust.orthographicSize = cameraSize;
            cameraToAdjust.transform.position = new Vector3(spriteBounds.center.x, spriteBounds.center.y, cameraToAdjust.transform.position.z);
        }
        
        [Button("Fit Tilemap")]
        public void FitTilemap()
        {
            if (targetTilemapRenderer == null || cameraToAdjust == null)
            {
                DebugUtils.DebugLogErrorMsg("Please assign the target Tilemap and the Camera to adjust.");
                return;
            }

            var tilemapBounds = targetTilemapRenderer.bounds;
            var tilemapSize = Mathf.Max(tilemapBounds.size.x, tilemapBounds.size.y);
            var cameraSize = tilemapSize * 0.5f;

            cameraToAdjust.orthographic = true;
            cameraToAdjust.orthographicSize = cameraSize;
            cameraToAdjust.transform.position = new Vector3(tilemapBounds.center.x, tilemapBounds.center.y, cameraToAdjust.transform.position.z);
        }
    }
}
