using TMPro.Examples;
using UnityEngine;

namespace LUP.PCR
{
    public class TestCameraController : MonoBehaviour
    {
        //public static TestCameraController instance;
        public Camera cam;

        //private void Awake()
        //{
        //    instance = this;
        //}

        public void FocusOn(Vector3 targetPos)
        {
            Vector3 newPos = new Vector3(targetPos.x, transform.position.y, targetPos.z - 10f);
            transform.position = newPos;
        }

        private void OnMouseEnter()
        {
            FocusOn(this.transform.position);
            // Debug.Log($"[Camera] {name} 포커스");
        }
    }
 }


