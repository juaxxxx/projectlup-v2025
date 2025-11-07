using UnityEngine;

namespace PCR
{
    public class Worker : MonoBehaviour
    {
        public float hunger = 0.8f;
        public bool hasPausedWork = false;
        public bool hasNewTask = true;

        public Vector3 eatingSpot;
        public Vector3 originSpot;
        public Vector3 pausedWorkSpot;
        public Vector3 newTaskSpot;
        public Vector3 loungeSpot;

        void Awake()
        {
            originSpot = transform.position;
            eatingSpot = transform.position + Vector3.forward * 5f;
            pausedWorkSpot = transform.position + Vector3.right * 5f;
            newTaskSpot = transform.position + Vector3.left * 5f;
            loungeSpot = transform.position + Vector3.back * 5f;
        }

        public void MoveTo(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 2f);
        }

        public bool IsAt(Vector3 target)
        {
            return Vector3.Distance(transform.position, target) < 0.1f;
        }
    }

}
