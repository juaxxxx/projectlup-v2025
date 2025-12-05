using UnityEngine;
using UnityEngine.EventSystems;
namespace LUP.ST
{

    public class PlayerController : MonoBehaviour
    {
        [Header("캐릭터 참조")]
        private RangeBlackBoard rangedCharacter;
        private RangeActions weaponActions;

        [Header("디버그 설정")]
        public bool showInputDebug = true;

        private bool prevManualMode = false;

        void Awake()
        {
            rangedCharacter = GetComponent<RangeBlackBoard>();
            weaponActions = GetComponent<RangeActions>();

            if (showInputDebug)
            {
                Debug.Log($"PlayerController 초기화: {gameObject.name}");
            }
        }

        void Update()
        {
            if (rangedCharacter == null)
                return;

            // ★ 수동 모드로 "갓 진입한" 순간 감지
            if (rangedCharacter.manualMode && !prevManualMode)
            {
                // 이 캐릭터를 새로 조작하기 시작함
                weaponActions?.OnEnterManualMode();
            }

            prevManualMode = rangedCharacter.manualMode;

            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            // 원거리 캐릭터 입력 처리
            if (rangedCharacter != null && rangedCharacter.manualMode)
            {
                HandleRangedInput();
            }
        }

        private void HandleRangedInput()
        {
            // 재장전 중이면 클릭 무시
            if (weaponActions != null && weaponActions.IsReloading)
            {
                return;
            }

            // 마우스 클릭 처리
            if (Input.GetMouseButtonDown(0))
            {
                rangedCharacter.playerInputExists = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                rangedCharacter.playerInputExists = false;
            }

            // 터치 입력 처리 (모바일)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    rangedCharacter.playerInputExists = true;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    rangedCharacter.playerInputExists = false;
                }
            }
        }

    }
}