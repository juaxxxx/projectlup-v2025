using UnityEngine;

namespace LUP.RL
{
    public class Wall : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 WorldPosition;

        private void Awake()
        {
            WorldPosition = this.transform.position;
        }
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}

