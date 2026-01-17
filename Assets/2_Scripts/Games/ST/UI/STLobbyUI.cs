using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LUP.ST
{
    public class STLobbyUI : MonoBehaviour
    {
        [Header("팀 슬롯")]
        [SerializeField] private Button[] teamSlotButtons;      // 5개
        [SerializeField] private Image[] teamSlotImages;        // 5개 (CharacterImage)

        [Header("캐릭터 선택 팝업")]
        [SerializeField] private GameObject characterSelectPopup;
        [SerializeField] private Transform characterGrid;
        [SerializeField] private GameObject characterButtonPrefab;

        [Header("게임 시작")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button backButton;

        private int selectedSlotIndex = -1;

        void Start()
        {
            SetupTeamSlots();
            SetupButtons();
            RefreshTeamUI();
            characterSelectPopup.SetActive(false);
        }

        void SetupTeamSlots()
        {
            for (int i = 0; i < teamSlotButtons.Length; i++)
            {
                int index = i;
                teamSlotButtons[i].onClick.AddListener(() => OnTeamSlotClicked(index));
            }
        }

        void SetupButtons()
        {
            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
        }

        void OnTeamSlotClicked(int slotIndex)
        {
            selectedSlotIndex = slotIndex;
            OpenCharacterSelectPopup();
        }

        void OpenCharacterSelectPopup()
        {
            foreach (Transform child in characterGrid)
            {
                Destroy(child.gameObject);
            }

            var selectedIds = GetSelectedCharacterIds();

            // 현재 슬롯에 있는 캐릭터 ID
            int currentSlotCharId = STDataManage.Instance.RuntimeData.TeamSlots[selectedSlotIndex];

            var ownedCharacters = STDataManage.Instance.GetOwnedCharacters();
            foreach (var charData in ownedCharacters)
            {
                var btnObj = Instantiate(characterButtonPrefab, characterGrid);

                var charImage = btnObj.transform.Find("CharacterImage")?.GetComponent<Image>();
                if (charImage != null && charData.thumbnail != null)
                    charImage.sprite = charData.thumbnail;

                var levelText = btnObj.transform.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
                if (levelText != null)
                {
                    var charInfo = STDataManage.Instance.RuntimeData.OwnedCharacterList
                        .Find(c => c.characterId == charData.characterId);
                    int level = charInfo != null ? charInfo.level : 1;
                    levelText.text = $"Lv.{level}";
                }

                var btn = btnObj.GetComponent<Button>();
                int charId = charData.characterId;

                // 현재 슬롯에 있는 캐릭터면 → 클릭 시 비우기 (노란색 테두리)
                if (charId == currentSlotCharId)
                {
                    var btnImage = btnObj.GetComponent<Image>();
                    if (btnImage != null)
                        btnImage.color = Color.yellow;  // 현재 선택된 캐릭터 표시

                    btn.onClick.AddListener(() => OnCharacterDeselected());
                }
                // 다른 슬롯에 있는 캐릭터면 → 비활성화
                else if (selectedIds.Contains(charId))
                {
                    btn.interactable = false;
                    if (charImage != null)
                        charImage.color = new Color(0.4f, 0.4f, 0.4f);
                }
                // 선택 안 된 캐릭터면 → 선택 가능
                else
                {
                    btn.onClick.AddListener(() => OnCharacterSelected(charId));
                }
            }

            characterSelectPopup.SetActive(true);
        }

        // 캐릭터 선택 해제 (슬롯 비우기)
        void OnCharacterDeselected()
        {
            if (selectedSlotIndex < 0) return;

            STDataManage.Instance.SetTeamSlot(selectedSlotIndex, 0);  // 0 = 빈 슬롯

            characterSelectPopup.SetActive(false);
            selectedSlotIndex = -1;

            RefreshTeamUI();
        }

        void OnCharacterSelected(int characterId)
        {
            if (selectedSlotIndex < 0) return;

            STDataManage.Instance.SetTeamSlot(selectedSlotIndex, characterId);

            characterSelectPopup.SetActive(false);
            selectedSlotIndex = -1;

            RefreshTeamUI();
        }

        void RefreshTeamUI()
        {
            var team = STDataManage.Instance.GetCurrentTeam();

            for (int i = 0; i < 5; i++)
            {
                if (team[i] != null && team[i].thumbnail != null)
                {
                    teamSlotImages[i].sprite = team[i].thumbnail;
                    teamSlotImages[i].color = Color.white;
                }
                else
                {
                    teamSlotImages[i].sprite = null;
                    teamSlotImages[i].color = new Color(0.3f, 0.3f, 0.3f);
                }
            }
        }

        void OnStartClicked()
        {
            var team = STDataManage.Instance.GetCurrentTeam();

            // 최소 1명 이상 있어야 시작
            bool hasCharacter = false;
            foreach (var c in team)
            {
                if (c != null) { hasCharacter = true; break; }
            }

            if (!hasCharacter)
            {
                Debug.LogWarning("팀에 캐릭터를 최소 1명 배치해주세요!");
                return;
            }

            // Team 배열을 RuntimeData에 설정
            STDataManage.Instance.RuntimeData.Team = team;
            STDataManage.Instance.SaveRuntimeData();

            Debug.Log("게임 시작!");
            StageManager.Instance.LoadStage(Define.StageKind.ST, 1);
        }

        void OnBackClicked()
        {
            Debug.Log("메뉴로 돌아가기");
            // TODO: 메뉴 씬으로 전환
        }

        private System.Collections.Generic.HashSet<int> GetSelectedCharacterIds()
        {
            var selectedIds = new System.Collections.Generic.HashSet<int>();
            var runtimeData = STDataManage.Instance.RuntimeData;

            if (runtimeData != null && runtimeData.TeamSlots != null)
            {
                foreach (int id in runtimeData.TeamSlots)
                {
                    if (id != 0) // 0은 빈 슬롯을 의미한다고 가정
                    {
                        selectedIds.Add(id);
                    }
                }
            }
            return selectedIds;
        }

        // 팝업 닫기 (CloseButton이나 DimBackground에 연결)
        public void ClosePopup()
        {
            characterSelectPopup.SetActive(false);
            selectedSlotIndex = -1;
        }
    }
}