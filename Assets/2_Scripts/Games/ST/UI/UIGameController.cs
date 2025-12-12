using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace LUP.ST
{

    public class UIGameController : MonoBehaviour
    {
        [Header("상단 UI")]
        [SerializeField] private Button modeToggleButton;
        [SerializeField] private TextMeshProUGUI modeButtonText;

        [Header("하단 캐릭터 선택 패널")]
        [SerializeField] private List<Button> characterSelectButtons = new List<Button>();
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color disabledColor = Color.gray;

        [Header("카메라")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private List<Transform> cameraSlotPoints = new List<Transform>();

        private bool isSkillAuto = true;
        private int currentSelectedIndex = -1;

        private readonly List<GameObject> allCharacters = new List<GameObject>();
        private readonly List<RangeBlackBoard> rangedCharacters = new List<RangeBlackBoard>();
        private readonly List<MeleeBlackBoard> meleeCharacters = new List<MeleeBlackBoard>();

        void Start()
        {
            InitializeCharacters();   // 씬에서 캐릭터 찾고, 왼쪽→오른쪽으로 정렬
            SetupButtons();           // 버튼에 캐릭터 매핑 및 활성/비활성 설정
            InitSkillAutoModeUI();    // AUTO 버튼 텍스트 초기화
            //SelectInitialManualRanged(); // 가장 왼쪽 원거리 캐릭터를 수동 모드로 선택
            DeselectAllCharacters();
        }


        private void InitializeCharacters()
        {
            allCharacters.Clear();
            rangedCharacters.Clear();
            meleeCharacters.Clear();

            // 1) 씬에서 원거리/근거리 캐릭터 전부 찾기
            RangeBlackBoard[] ranged = FindObjectsByType<RangeBlackBoard>(FindObjectsSortMode.None);
            MeleeBlackBoard[] melee = FindObjectsByType<MeleeBlackBoard>(FindObjectsSortMode.None);

            // 2) GameObject 기준으로 임시 리스트에 넣고, x 좌표 기준으로 왼쪽→오른쪽 정렬
            List<GameObject> temp = new List<GameObject>();
            foreach (var r in ranged) temp.Add(r.gameObject);
            foreach (var m in melee) temp.Add(m.gameObject);

            temp = temp
                .OrderBy(go => go.transform.position.x)
                .ToList();

            // 3) 정렬된 순서대로 allCharacters 채우고, 타입별 리스트도 같이 구성
            foreach (GameObject go in temp)
            {
                allCharacters.Add(go);

                if (go.TryGetComponent(out RangeBlackBoard rbb))
                    rangedCharacters.Add(rbb);

                if (go.TryGetComponent(out MeleeBlackBoard mbb))
                    meleeCharacters.Add(mbb);
            }

            Debug.Log($"캐릭터 초기화 완료: 전체 {allCharacters.Count} (원거리 {rangedCharacters.Count}, 근거리 {meleeCharacters.Count})");
        }

        private void SetupButtons()
        {
            // AUTO / SKILL 토글 버튼
            if (modeToggleButton != null)
            {
                modeToggleButton.onClick.RemoveAllListeners();
                modeToggleButton.onClick.AddListener(OnModeToggleClicked);
            }

            // 캐릭터 선택 버튼 설정
            for (int i = 0; i < characterSelectButtons.Count; i++)
            {
                Button btn = characterSelectButtons[i];
                if (btn == null) continue;

                btn.onClick.RemoveAllListeners();

                if (i < allCharacters.Count)
                {
                    // 해당 슬롯에 캐릭터가 있으면 원거리/근거리 관계없이 선택 가능
                    btn.interactable = true;

                    int index = i;
                    btn.onClick.AddListener(() => OnCharacterSelected(index));
                }
                else
                {
                    // 캐릭터 없는 버튼은 비활성
                    btn.interactable = false;
                }
            }

            UpdateCharacterPanelColors();
        }

        private void InitSkillAutoModeUI()
        {
            isSkillAuto = true;
            if (modeButtonText != null)
            {
                modeButtonText.text = "AUTO SKILL: ON";
            }
        }

        public void OnModeToggleClicked()
        {
            isSkillAuto = !isSkillAuto;

            if (modeButtonText != null)
            {
                modeButtonText.text = isSkillAuto ? "AUTO SKILL: ON" : "AUTO SKILL: OFF";
            }

            Debug.Log($"스킬 자동 사용 모드: {(isSkillAuto ? "ON" : "OFF")}");
        }

        private void SelectInitialManualRanged()
        {
            if (rangedCharacters.Count == 0)
            {
                currentSelectedIndex = -1;
                UpdateCharacterPanelColors();
                Debug.LogWarning("원거리 캐릭터가 없어 수동 조작 캐릭터를 설정하지 못했습니다.");
                return;
            }

            // x 좌표 기준으로 가장 왼쪽 원거리 캐릭터 찾기
            RangeBlackBoard leftMostRanged = rangedCharacters
                .OrderBy(r => r.transform.position.x)
                .First();

            GameObject targetGO = leftMostRanged.gameObject;
            int index = allCharacters.IndexOf(targetGO);

            if (index >= 0)
            {
                SelectCharacter(index);
                Debug.Log($"{leftMostRanged.characterName} (원거리) 를 초기 수동 캐릭터로 설정");
            }
            else
            {
                Debug.LogWarning("왼쪽 원거리 캐릭터를 allCharacters에서 찾지 못했습니다.");
            }
        }

        public void OnCharacterSelected(int index)
        {
            if (index < 0 || index >= allCharacters.Count)
                return;

            // 같은 버튼 다시 누르면 → 선택 해제 + 오버뷰
            if (index == currentSelectedIndex)
            {
                DeselectAllCharacters();
                return;
            }

            // 새 캐릭터 선택
            SelectCharacter(index);
        }

        private void SelectCharacter(int index)
        {
            // 1) 모든 원거리 캐릭터를 자동 모드로 되돌림
            foreach (var r in rangedCharacters)
            {
                r.manualMode = false;
                r.playerInputExists = false;
            }

            GameObject selected = allCharacters[index];

            RangeBlackBoard ranged = selected.GetComponent<RangeBlackBoard>();
            MeleeBlackBoard melee = selected.GetComponent<MeleeBlackBoard>();

            // 2) 선택된 캐릭터가 원거리라면 → 수동 모드 ON
            if (ranged != null)
            {
                ranged.manualMode = true;
                Debug.Log($"{ranged.characterName} 선택 (원거리 수동 모드)");
            }

            // 3) 카메라 포인트 결정
            Transform camPoint = null;

            if (ranged != null)
            {
                // 원거리: 슬롯 카메라 사용 (SlotCam)
                if (cameraSlotPoints != null && index < cameraSlotPoints.Count)
                {
                    camPoint = cameraSlotPoints[index];
                }
            }
            else if (melee != null)
            {
                // 근거리: 캐릭터에 달린 MeleeCamPoint 사용
                if (melee.CameraFocusPoint != null)
                    camPoint = melee.CameraFocusPoint;
                else
                    camPoint = melee.transform; // 예외: 포인트 안 넣었을 때
            }

            // 4) 카메라 이동
            if (camPoint != null)
            {
                cameraController?.FocusOnPoint(camPoint);
            }

            // 5) 줌 가능 여부 갱신 (원거리만 줌 허용)
            bool canZoom = ranged != null;   // 원거리면 true, 근거리면 false
            cameraController?.SetZoomEnabled(canZoom);

            currentSelectedIndex = index;
            UpdateCharacterPanelColors();
        }

        private void UpdateCharacterPanelColors()
        {
            for (int i = 0; i < characterSelectButtons.Count; i++)
            {
                Button btn = characterSelectButtons[i];
                if (btn == null) continue;

                Image image = btn.GetComponent<Image>();
                if (image == null) continue;

                if (!btn.interactable)
                {
                    // 근거리 캐릭터 또는 빈 슬롯 → 회색 처리
                    image.color = disabledColor;
                }
                else if (i == currentSelectedIndex)
                {
                    image.color = selectedColor;
                }
                else
                {
                    image.color = normalColor;
                }
            }
        }

        void Update()
        {
            // 디버그 정보 출력 (개발 중에만 사용)
            if (Input.GetKeyDown(KeyCode.F1))
            {
                DebugCurrentState();
            }
        }

        private void DebugCurrentState()
        {
            Debug.Log("=== UI Controller 상태 ===");
            Debug.Log($"Skill Auto: {(isSkillAuto ? "ON" : "OFF")}");
            Debug.Log($"Selected Index: {currentSelectedIndex}");

            for (int i = 0; i < allCharacters.Count; i++)
            {
                GameObject character = allCharacters[i];
                RangeBlackBoard ranged = character.GetComponent<RangeBlackBoard>();

                if (ranged != null)
                {
                    Debug.Log($"[{i}] {ranged.characterName} (원거리): manual={ranged.manualMode}, input={ranged.playerInputExists}, enemy={ranged.enemyInRange}");
                }
                else if (character.GetComponent<MeleeBlackBoard>() != null)
                {
                    Debug.Log($"[{i}] (근거리): manual=자동전용");
                }
            }
        }

        private void DeselectAllCharacters()
        {
            foreach (var r in rangedCharacters)
            {
                r.manualMode = false;
                r.playerInputExists = false;
            }

            currentSelectedIndex = -1;
            UpdateCharacterPanelColors();

            // 카메라 전체 보기로 복귀
            cameraController?.SetOverviewMode();
            cameraController?.SetZoomEnabled(false);

            Debug.Log("수동 조작 캐릭터 없음 → 전체 풀 오토 + 카메라 오버뷰");
        }

    }
}