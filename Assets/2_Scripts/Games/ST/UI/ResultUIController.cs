using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

namespace LUP.ST
{
    public class ResultUIController : MonoBehaviour
    {
        [Header("참조")]
        public Transform slotContainer;
        public GameObject slotPrefab;
        public TextMeshProUGUI totalGoldText;

        [Header("보상 설정")]
        [SerializeField] private int goldReward = 1000; // 스테이지 클리어 기본 골드
        [SerializeField] private int expReward = 100;   // 스테이지 클리어 기본 경험치

        void Start()
        {
            // 게임이 끝나고 결과 씬이 로드되면 자동으로 실행
            DisplayResults();
        }

        public void DisplayResults()
        {
            foreach (Transform child in slotContainer) Destroy(child.gameObject);

            var team = LobbyTeamCache.GetCopy();
            if (team == null) return;

            // 1. 골드 보상 지급 (세이브 데이터에 바로 반영)
            if (STSaveHandler.CurrentData != null)
            {
                // STSaveHandler에 Gold 변수가 있다면 아래처럼 추가
                // STSaveHandler.CurrentData.gold += goldReward;
                totalGoldText.text = $"GOLD + {goldReward:N0}";
            }

            // 2. 캐릭터별 경험치 지급 및 UI 생성
            for (int i = 0; i < team.Length; i++)
            {
                if (team[i] == null) continue;

                GameObject go = Instantiate(slotPrefab, slotContainer);
                ResultSlotUI ui = go.GetComponent<ResultSlotUI>();

                // 세이브 데이터에서 해당 캐릭터 찾기
                var saveData = STSaveHandler.CurrentData.characterList.Find(c => c.characterId == team[i].characterId);

                if (saveData != null)
                {
                    int currentLevel = saveData.level;
                    int currentExp = saveData.currentExp;
                    int maxExp = currentLevel * 100; // 레벨당 필요 경험치 공식

                    // UI 초기 세팅
                    ui.SetupSlot(team[i].thumbnail, currentLevel, currentExp, maxExp);

                    // 3. 실제 데이터 갱신 (경험치 추가 및 레벨업 체크)
                    UpdateCharacterData(saveData, expReward);

                    // 4. 경험치 상승 애니메이션 실행
                    StartCoroutine(ui.AnimateExp(currentExp, expReward, maxExp));
                }
            }

            // 5. 모든 변경사항을 파일로 저장!
            STSaveHandler.Save();
            Debug.Log("스테이지 결과가 세이브 데이터에 저장되었습니다.");
        }

        private void UpdateCharacterData(CharacterLevelData data, int gain)
        {
            data.currentExp += gain;

            // 레벨업 로직 (경험치가 필요치보다 높으면 레벨업)
            while (data.currentExp >= data.level * 100)
            {
                data.currentExp -= data.level * 100;
                data.level++;
                Debug.Log($"{data.characterId} 레벨업! 현재 레벨: {data.level}");
            }
        }
    }
}