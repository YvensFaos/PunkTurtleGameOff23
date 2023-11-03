using UnityEngine;
using Utils;

[RequireComponent(typeof(BoxCollider2D))]
public class Box2DGizmoViewer : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D selfCollider;
    [SerializeField]
    private Color colliderColor = Color.white;
    private void Awake()
    {
        AssessUtils.CheckRequirement(ref selfCollider, this);
    }

    private void OnDrawGizmos()
    {
        if (selfCollider == null) return;
    
        Gizmos.color = colliderColor;
        var position = new Vector3(selfCollider.offset.x, selfCollider.offset.y, 0) + transform.position;
        Gizmos.DrawWireCube(position, selfCollider.size);
    }
}
