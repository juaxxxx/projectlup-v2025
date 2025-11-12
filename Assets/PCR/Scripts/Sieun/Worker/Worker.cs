using UnityEngine;

namespace LUP.PCR
{
    public class Worker : MonoBehaviour
    {


        //public Vector3 eatingSpot;
        //public Vector3 originSpot;
        //public Vector3 pausedWorkSpot;
        //public Vector3 newTaskSpot;
        //public Vector3 loungeSpot;

        [SerializeField]Transform dest;

        void Awake()
        {
            //originSpot = transform.position;
            //eatingSpot = transform.position;
            //pausedWorkSpot = transform.position;
            //newTaskSpot = transform.position;
            //loungeSpot = transform.position;
        }

        private void Start()
        {
            //eatingSpot = dest.position;
        }

       
    }

}
