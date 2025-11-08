using UnityEngine;

namespace LUP.PCR
{
    public class CameraProjectionTest : MonoBehaviour
    {
        private bool isOrtho = false;
        private Camera mainCam;

        void Start()
        {
            mainCam = Camera.main;
        }

        void Update()
        {
            // Space 키를 눌렀을 때 모드 전환
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isOrtho = !isOrtho;

                if (isOrtho)
                {
                    mainCam.orthographic = true;
                    mainCam.orthographicSize = 23.1f;
                    Debug.Log("현재 모드: Orthographic");
                }
                else
                {
                    // Perspective (원근 투영) 모드로 전환
                    mainCam.orthographic = false;
                    mainCam.fieldOfView = 112.5f;
                    Debug.Log("현재 모드: Perspective");
                }
            }
        }
    }
}
