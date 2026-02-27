using LUP.DSG.Utils.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class CharacterHeadupUI : MonoBehaviour
    {
        private Transform target;
        private Vector3 offset = new Vector3(0, 0, 0);

        private Camera mainCamera;
        private RectTransform rectTransform;
        private RectTransform canvasRect;

        [Header("보정 설정")]
        [Range(0f, 0.2f)]
        public float distortionFactor = 0.00f;

        private CanvasGroup canvasGroup;

        private CharacterInfoUI characterInfoUI;
        private CharacterBattleUI chracterBattleUI;

        void Awake()
        {
            mainCamera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            //canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            characterInfoUI = GetComponentInChildren<CharacterInfoUI>();
            characterInfoUI.gameObject.SetActive(false);

            chracterBattleUI = GetComponentInChildren<CharacterBattleUI>();
            chracterBattleUI.gameObject.SetActive(false);
        }

        public void InitInfoUI(EAttributeType type, int level)
        {
            characterInfoUI.SetCharacterInfo(type, level);
        }

        public void InitBattleUI(Character character)
        {
            chracterBattleUI.Init(character);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 viewportPos = Camera.main.WorldToViewportPoint(target.position + offset);

            if (viewportPos.z < 0)
            {
                canvasGroup.alpha = 0;
                return;
            }

            canvasGroup.alpha = 1;

            // 화면 중앙(0.5)으로부터의 거리 계산
            float distanceFromCenter = viewportPos.x - 0.5f;

            // 보정치 적용: 중앙에서 멀어질수록 반대 방향으로 좌표를 이동시킴
            // x 좌표가 1에 가까워지면(오른쪽 끝), 값을 마이너스해서 왼쪽으로 당김
            float correctedX = viewportPos.x - (distanceFromCenter * distortionFactor);

            // 최종 좌표를 캔버스 크기에 맞게 변환
            Vector2 canvasSize = transform.parent.GetComponent<RectTransform>().sizeDelta;
            Vector2 finalPos = new Vector2((correctedX * canvasSize.x) - (canvasSize.x * 0.5f), (viewportPos.y * canvasSize.y) - (canvasSize.y * 0.5f));

            rectTransform.anchoredPosition = finalPos;
        }

        public void SetTarget(Canvas canvas, Transform newTarget, Vector3 uiOffset)
        {
            canvasRect = canvas.GetComponent<RectTransform>();
            target = newTarget;
            offset = uiOffset;
            gameObject.SetActive(true);
        }

        public void ReleaseTarget()
        {
            gameObject.SetActive(false);
            target = null;
        }

        public void ActiveInfoUI()
        {
            characterInfoUI.gameObject.SetActive(true);
            chracterBattleUI.gameObject.SetActive(false);
        }

        public void ActiveBattleUI()
        {
            characterInfoUI.gameObject.SetActive(false);
            chracterBattleUI.gameObject.SetActive(true);
        }
    }
}