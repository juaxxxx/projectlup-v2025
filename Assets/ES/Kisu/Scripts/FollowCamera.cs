using UnityEngine;
using UnityEngine.UIElements;

namespace ES
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private bool x, y, z;
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = new Vector3(
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z - 5 : transform.position.z));
        }
    }
}