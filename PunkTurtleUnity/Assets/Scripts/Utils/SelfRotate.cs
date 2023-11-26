using UnityEngine;

namespace Utils
{
    public class SelfRotate : MonoBehaviour
    {
        [SerializeField]
        private Vector3 axis;
        [SerializeField]
        private float speed;

        private void Update()
        {
            transform.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
