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
        [SerializeField] private List<Image> characterButtonImages = new List<Image>();
        [SerializeField] private List<CharacterHpSlotUI> hpSlots = new List<CharacterHpSlotUI>();
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Color disabledColor = Color.gray;

        [Header("카메라")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private List<Transform> cameraSlotPoints = new List<Transform>();

        [Header("크로스헤어")]
        [SerializeField] private CrosshairController crosshairManager;

        private bool isSkillAuto = true;
        private int currentSelectedIndex = -1;
        private STCharacterData[] currentTeam;
        private List<GameObject> spawnedCharactersList;

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
            if (modeToggleButton != null)
            {
                modeToggleButton.onClick.RemoveAllListeners();
                modeToggleButton.onClick.AddListener(OnModeToggleClicked);
            }

            for (int i = 0; i < characterSelectButtons.Count; i++)
            {
                Button btn = characterSelectButtons[i];
                if (btn == null) continue;

                btn.onClick.RemoveAllListeners();

                // 슬롯에 캐릭터가 있고, 실제로 스폰됐으면 활성화
                if (i < allCharacters.Count && allCharacters[i] != null)
                {
                    btn.interactable = true;
                    int index = i;
                    btn.onClick.AddListener(() => OnCharacterSelected(index));
                }
                else
                {
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
            // null 체크 추가
            if (allCharacters[index] == null) return;

            foreach (var r in rangedCharacters)
            {
                r.manualMode = false;
                r.playerInputExists = false;
            }

            GameObject selected = allCharacters[index];

            RangeBlackBoard ranged = selected.GetComponent<RangeBlackBoard>();
            MeleeBlackBoard melee = selected.GetComponent<MeleeBlackBoard>();

            if (ranged != null)
            {
                ranged.manualMode = true;
                Debug.Log($"{ranged.characterName} 선택 (원거리 수동 모드)");
            }

            Transform camPoint = null;

            if (ranged != null)
            {
                if (cameraSlotPoints != null && index < cameraSlotPoints.Count)
                {
                    camPoint = cameraSlotPoints[index];
                }
            }
            else if (melee != null)
            {
                if (melee.CameraFocusPoint != null)
                    camPoint = melee.CameraFocusPoint;
                else
                    camPoint = melee.transform;
            }

            if (camPoint != null)
            {
                cameraController?.FocusOnPoint(camPoint);
            }

            bool canZoom = ranged != null;
            cameraController?.SetZoomEnabled(canZoom);

            currentSelectedIndex = index;
            UpdateCharacterPanelColors();

            if (ranged != null)
            {
                // 현재 팀에서 캐릭터 데이터 가져와서 전달
                var characterData = currentTeam != null && index < currentTeam.Length
                    ? currentTeam[index]
                    : null;
                crosshairManager?.Show(characterData);
            }
            else
            {
                crosshairManager?.HideAll();
            }

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

            crosshairManager?.HideAll();

            Debug.Log("수동 조작 캐릭터 없음 → 전체 풀 오토 + 카메라 오버뷰");
        }

        public void RebuildAfterSpawn()
        {
            allCharacters.Clear();
            rangedCharacters.Clear();
            meleeCharacters.Clear();

            // spawnedCharactersList 기반으로 설정 (슬롯 순서 유지)
            if (spawnedCharactersList != null)
            {
                foreach (var go in spawnedCharactersList)
                {
                    allCharacters.Add(go);  // null도 포함해서 인덱스 유지

                    if (go != null)
                    {
                        if (go.TryGetComponent(out RangeBlackBoard rbb))
                            rangedCharacters.Add(rbb);
                        if (go.TryGetComponent(out MeleeBlackBoard mbb))
                            meleeCharacters.Add(mbb);
                    }
                }
            }

            SetupButtons();
            InitSkillAutoModeUI();
            DeselectAllCharacters();
        }
        public void ApplyTeamThumbnails(STCharacterData[] team5)
        {
            if (team5 == null || team5.Length < 5) return;

            currentTeam = team5;  // 저장

            for (int i = 0; i < characterSelectButtons.Count && i < 5; i++)
            {
                var btn = characterSelectButtons[i];
                if (btn == null) continue;

                Image img = (characterButtonImages != null && i < characterButtonImages.Count && characterButtonImages[i] != null)
                    ? characterButtonImages[i]
                    : btn.GetComponent<Image>();

                if (team5[i] == null)
                {
                    btn.gameObject.SetActive(false);
                }
                else
                {
                    btn.gameObject.SetActive(true);
                    if (img != null)
                    {
                        img.sprite = team5[i].thumbnail;
                        img.color = Color.white;
                    }
                    btn.interactable = true;
                }

                if (i < hpSlots.Count && hpSlots[i] != null)
                {
                    hpSlots[i].gameObject.SetActive(team5[i] != null);
                }
            }
        }

        public void BindHpToSpawnedCharacters(List<GameObject> spawnedCharactersInSlotOrder)
        {
            spawnedCharactersList = spawnedCharactersInSlotOrder;  // 저장

            for (int i = 0; i < 5 && i < hpSlots.Count; i++)
            {
                if (hpSlots[i] == null) continue;

                if (spawnedCharactersInSlotOrder != null &&
                    i < spawnedCharactersInSlotOrder.Count &&
                    spawnedCharactersInSlotOrder[i] != null)
                {
                    var stat = spawnedCharactersInSlotOrder[i].GetComponent<StatComponent>();
                    hpSlots[i].Bind(stat);
                }
                else
                {
                    hpSlots[i].Bind(null);
                }
            }
        }

    }
}