using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
namespace ST
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

        private bool isAutoMode = true;
        private int currentSelectedIndex = -1;

        private List<GameObject> allCharacters = new List<GameObject>();
        private List<CharacterInfo> rangedCharacters = new List<CharacterInfo>();
        private List<CharacterInfo_Melee> meleeCharacters = new List<CharacterInfo_Melee>();

        void Start()
        {
            InitializeCharacters();
            SetupButtons();
            SetAutoMode();

        }


        private void InitializeCharacters()
        {
            // 원거리 캐릭터 찾기
            CharacterInfo[] ranged = FindObjectsByType<CharacterInfo>(FindObjectsSortMode.None);
            rangedCharacters.AddRange(ranged);

            // 근거리 캐릭터 찾기
            CharacterInfo_Melee[] melee = FindObjectsByType<CharacterInfo_Melee>(FindObjectsSortMode.None);
            meleeCharacters.AddRange(melee);

            // 전체 캐릭터 리스트 구성
            foreach (var character in rangedCharacters)
            {
                allCharacters.Add(character.gameObject);
            }
            foreach (var character in meleeCharacters)
            {
                allCharacters.Add(character.gameObject);
            }

            Debug.Log($"캐릭터 초기화 완료: 원거리 {rangedCharacters.Count}, 근거리 {meleeCharacters.Count}");
        }

        private void SetupButtons()
        {
            if (modeToggleButton != null)
            {
                modeToggleButton.onClick.AddListener(OnModeToggleClicked);
            }

            for (int i = 0; i < characterSelectButtons.Count && i < allCharacters.Count; i++)
            {
                int index = i;
                if (characterSelectButtons[i] != null)
                {
                    characterSelectButtons[i].onClick.AddListener(() => OnCharacterSelected(index));
                }
            }
        }

        public void OnModeToggleClicked()
        {
            if (isAutoMode)
            {
                SetManualMode();
            }
            else
            {
                SetAutoMode();
            }
        }

        private void SetAutoMode()
        {
            isAutoMode = true;

            if (modeButtonText != null)
            {
                modeButtonText.text = "AUTO";
            }

            // 모든 캐릭터를 자동 모드로 설정
            foreach (var character in rangedCharacters)
            {
                character.manualMode = false;
                // 입력 상태 초기화
                character.playerInputExists = false;
            }
            foreach (var character in meleeCharacters)
            {
                character.manualMode = false;
                // 입력 상태 초기화
                character.playerInputExists = false;
            }

            currentSelectedIndex = -1;
            UpdateCharacterPanelColors();

            Debug.Log("모든 캐릭터 AUTO 모드");
        }

        private void SetManualMode()
        {
            isAutoMode = false;

            if (modeButtonText != null)
            {
                modeButtonText.text = "MANUAL";
            }

            // 첫 번째 캐릭터를 선택된 상태로 설정
            if (allCharacters.Count > 0)
            {
                SelectCharacter(0);
            }

            Debug.Log("MANUAL 모드 - 첫 번째 캐릭터 선택");
        }

        public void OnCharacterSelected(int index)
        {
            if (isAutoMode) return;
            if (index < 0 || index >= allCharacters.Count) return;

            SelectCharacter(index);
        }

        private void SelectCharacter(int index)
        {
            // 모든 캐릭터를 자동 모드로 먼저 설정
            foreach (var character in rangedCharacters)
            {
                character.manualMode = false;
                character.playerInputExists = false;
            }
            foreach (var character in meleeCharacters)
            {
                character.manualMode = false;
                character.playerInputExists = false;
            }

            // 선택된 캐릭터만 수동 모드로 설정
            GameObject selected = allCharacters[index];

            CharacterInfo ranged = selected.GetComponent<CharacterInfo>();
            if (ranged != null)
            {
                ranged.manualMode = true;
                Debug.Log($"{ranged.characterName} 선택 (원거리)");
            }

            CharacterInfo_Melee melee = selected.GetComponent<CharacterInfo_Melee>();
            if (melee != null)
            {
                melee.manualMode = true;
                Debug.Log($"{melee.characterName} 선택 (근거리)");
            }

            currentSelectedIndex = index;
            UpdateCharacterPanelColors();
        }

        private void UpdateCharacterPanelColors()
        {
            for (int i = 0; i < characterSelectButtons.Count; i++)
            {
                if (characterSelectButtons[i] == null) continue;

                Image image = characterSelectButtons[i].GetComponent<Image>();
                if (image != null)
                {
                    if (i == currentSelectedIndex && !isAutoMode)
                    {
                        image.color = selectedColor;
                    }
                    else
                    {
                        image.color = normalColor;
                    }
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
            Debug.Log($"=== UI Controller 상태 ===");
            Debug.Log($"Mode: {(isAutoMode ? "AUTO" : "MANUAL")}");
            Debug.Log($"Selected Index: {currentSelectedIndex}");

            for (int i = 0; i < allCharacters.Count; i++)
            {
                GameObject character = allCharacters[i];
                CharacterInfo ranged = character.GetComponent<CharacterInfo>();
                CharacterInfo_Melee melee = character.GetComponent<CharacterInfo_Melee>();

                if (ranged != null)
                {
                    Debug.Log($"[{i}] {ranged.characterName} (원거리): manual={ranged.manualMode}, input={ranged.playerInputExists}, enemy={ranged.enemyInRange}");
                }
                if (melee != null)
                {
                    Debug.Log($"[{i}] {melee.characterName} (근거리): manual={melee.manualMode}, input={melee.playerInputExists}");
                }
            }
        }
    }
}