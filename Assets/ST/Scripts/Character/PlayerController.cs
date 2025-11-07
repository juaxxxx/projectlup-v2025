using UnityEngine;
using UnityEngine.EventSystems;
namespace ST
{

    public class PlayerController : MonoBehaviour
    {
        [Header("캐릭터 참조")]
        private CharacterInfo rangedCharacter;
        private CharacterInfo_Melee meleeCharacter;
        private CharacterActions weaponActions;

        [Header("디버그 설정")]
        public bool showInputDebug = true;

        void Awake()
        {
            rangedCharacter = GetComponent<CharacterInfo>();
            meleeCharacter = GetComponent<CharacterInfo_Melee>();
            weaponActions = GetComponent<CharacterActions>();

            if (showInputDebug)
            {
                Debug.Log($"PlayerController 초기화: {gameObject.name}");
            }
        }

        void Update()
        {
            HandlePlayerInput();
        }

        private void HandlePlayerInput()
        {
            // 원거리 캐릭터 입력 처리
            if (rangedCharacter != null && rangedCharacter.manualMode)
            {
                HandleRangedInput();
            }

            // 근거리 캐릭터 입력 처리  
            if (meleeCharacter != null && meleeCharacter.manualMode)
            {
                HandleMeleeInput();
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

        private void HandleMeleeInput()
        {
            // 마우스 클릭 처리
            if (Input.GetMouseButtonDown(0))
            {
                meleeCharacter.playerInputExists = true;
            }

            // 터치 입력 처리 (모바일)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                meleeCharacter.playerInputExists = true;
            }
        }

    }
}