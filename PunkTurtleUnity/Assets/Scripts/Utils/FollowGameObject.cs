using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField]
    private GameObject follow;
    [SerializeField]
    private bool x;
    [SerializeField]
    private bool y;
    [SerializeField]
    private bool z;

    private void LateUpdate()
    {
        var followPosition = follow.transform.position;
        var newPosition = transform.position;

        newPosition.x = x ? followPosition.x : newPosition.x;
        newPosition.y = y ? followPosition.y : newPosition.y;
        newPosition.z = z ? followPosition.z : newPosition.z;
        transform.position = newPosition;
    }
}
