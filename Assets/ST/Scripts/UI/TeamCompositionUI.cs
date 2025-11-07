using UnityEngine;
using UnityEngine.UI;
namespace ST
{

    public class TeamCompositionUI : MonoBehaviour
    {
        public Button[] slotButtons;            // 상단 슬롯 선택 버튼(5개)
        public Image[] lobbyTeamImages;         // 실제 로비 씬의 팀 슬롯 이미지(5개, confirm 후 반영)
        public Button[] characterButtons;       // 하단 캐릭터 버튼(15개)
        public Sprite[] characterSprites;       // 캐릭터별 스프라이트(15개, 인덱스 매칭)
        public Button confirmButton;
        public Button cancelButton;

        private int selectedSlot = -1;          // 현재 선택 슬롯(없으면 -1)
        private int[] teamCandidate = new int[5]; // 팀 후보 슬롯 별 인덱스
        private int[] oldTeam = new int[5];     // confirm/cancel 비교용(초기 상태 저장)

        void Start()
        {
            // 초기 oldTeam/teamCandidate 동기화(예시는 0~4번 초기화)
            for (int i = 0; i < 5; i++) teamCandidate[i] = oldTeam[i] = i;

            // 슬롯 버튼 이벤트 연결
            for (int i = 0; i < slotButtons.Length; i++)
            {
                int idx = i;
                slotButtons[i].onClick.AddListener(() => OnSlotSelected(idx));
            }
            // 캐릭터 버튼 이벤트 연결
            for (int i = 0; i < characterButtons.Length; i++)
            {
                int idx = i;
                characterButtons[i].onClick.AddListener(() => OnCharacterSelected(idx));
            }
            confirmButton.onClick.AddListener(OnConfirm);
            cancelButton.onClick.AddListener(OnCancel);

            RefreshUI();
        }

        void OnSlotSelected(int slotIdx)
        {
            selectedSlot = slotIdx;
            RefreshUI();
        }

        void OnCharacterSelected(int charIdx)
        {
            if (selectedSlot == -1) return; // 슬롯 먼저 선택 필요

            // 이미 배치된 캐릭터면 무시
            for (int i = 0; i < 5; i++)
                if (teamCandidate[i] == charIdx) return;

            teamCandidate[selectedSlot] = charIdx;
            RefreshUI();
        }

        void OnConfirm()
        {
            for (int i = 0; i < 5; i++)
            {
                lobbyTeamImages[i].sprite = characterSprites[teamCandidate[i]];
                oldTeam[i] = teamCandidate[i]; // 저장
            }
            // 패널 비활성 처리 등 추가
            selectedSlot = -1;
            RefreshUI();
        }

        void OnCancel()
        {
            // teamCandidate를 oldTeam으로 롤백
            for (int i = 0; i < 5; i++)
                teamCandidate[i] = oldTeam[i];
            // 패널 비활성 처리 등 추가
            selectedSlot = -1;
            RefreshUI();
        }

        void RefreshUI()
        {
            // 슬롯 하이라이트 및 이미지 반영
            for (int i = 0; i < slotButtons.Length; i++)
            {
                slotButtons[i].GetComponent<Image>().color = (i == selectedSlot) ? Color.yellow : Color.white;
                slotButtons[i].GetComponentInChildren<Image>().sprite = characterSprites[teamCandidate[i]];
                // (슬롯 안에 Image 있는 경우, 버튼 자체가 이미지라면 이 부분만)
                // 번호/Text등이 있으면 추가적 연출도 가능
            }

            // 캐릭터 선택 상태 표시 및 상호작용 제한
            for (int i = 0; i < characterButtons.Length; i++)
            {
                characterButtons[i].image.sprite = characterSprites[i];

                // 이미 다른 슬롯에 배치된 캐릭터인지 체크
                bool isAssigned = false;
                for (int j = 0; j < 5; j++)
                    if (teamCandidate[j] == i)
                        isAssigned = true;

                if (isAssigned)
                {
                    characterButtons[i].image.color = Color.gray; // 회색처리(선택됨)
                    characterButtons[i].interactable = false;
                }
                else
                {
                    // 슬롯이 선택된 경우만 선택 가능, 아니면 비활성(혹은 연출)
                    characterButtons[i].image.color = Color.white;
                    characterButtons[i].interactable = (selectedSlot != -1);
                }
            }
        }
    }

}