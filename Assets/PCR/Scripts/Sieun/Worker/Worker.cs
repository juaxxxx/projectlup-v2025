using UnityEngine;

namespace LUP.PCR
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

        [SerializeField]Transform dest;

        void Awake()
        {
            originSpot = transform.position;
            eatingSpot = transform.position;
            pausedWorkSpot = transform.position;
            newTaskSpot = transform.position;
            loungeSpot = transform.position;
        }

        private void Start()
        {
            eatingSpot = dest.position;
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
