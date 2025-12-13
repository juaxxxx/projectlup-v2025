using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class WorkerOverlayUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image statusIcon;
        [SerializeField] private Canvas canvas;

        [Header("Icons")]
        [SerializeField] private Sprite iconIdle;      // 놂 (Zzz...)
        [SerializeField] private Sprite iconMoving;    // 이동/작업하러 감 (신발 or 깃발)
        [SerializeField] private Sprite iconWorking;   // 작업 중 (망치)
        [SerializeField] private Sprite iconHungry;    // 배고픔 (밥그릇)

        private WorkerAI targetWorker;
        private Camera mainCam;

        private void Start()
        {
            mainCam = Camera.main;

            // 캔버스 설정 강제 (월드 스페이스)
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = mainCam;
            }
        }

        public void Setup(WorkerAI worker)
        {
            targetWorker = worker;
            UpdateIcon(); // 초기화
        }

        private void LateUpdate()
        {
            // 1. 빌보드 처리 (항상 카메라 정면 보기)
            if (mainCam != null)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
            }

            // 2. 상태에 따른 아이콘 업데이트
            if (targetWorker != null)
            {
                UpdateIcon();
            }
        }

        private void UpdateIcon()
        {
            if (targetWorker.IsHunger)
            {
                statusIcon.sprite = iconHungry;
                statusIcon.enabled = true;
            }
            else if (targetWorker.HasTask)
            {
                // HasTask가 true면 이동 중이거나 작업 중
                // (세분화하고 싶다면 WorkerAI에 IsMoving 같은 상태를 추가해서 구분)
                //statusIcon.sprite = iconMoving;
                statusIcon.sprite = iconWorking;
                statusIcon.enabled = true;
            }
            else
            {
                // 할 일 없음
                statusIcon.sprite = iconIdle;
                // 혹은 놀 때는 아이콘을 끄고 싶다면: statusIcon.enabled = false;
                statusIcon.enabled = true;
            }
        }
    }
}