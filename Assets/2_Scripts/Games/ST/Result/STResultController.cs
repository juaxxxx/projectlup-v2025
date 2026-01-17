using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LUP.ST
{
    /// <summary>
    /// ResultScene 관리자
    /// </summary>
    public class STResultController : MonoBehaviour
    {
        [Header("타이틀")]
        [SerializeField] private TextMeshProUGUI titleText;

        [Header("캐릭터 슬롯 (5개)")]
        [SerializeField] private List<STResultCharacterSlot> characterSlots = new List<STResultCharacterSlot>();

        [Header("골드 UI")]
        [SerializeField] private TextMeshProUGUI goldText;

        [Header("버튼")]
        [SerializeField] private Button exitButton;

        [Header("캐릭터 데이터 (모든 캐릭터 SO)")]
        [SerializeField] private List<STCharacterData> allCharacterData = new List<STCharacterData>();

        [Header("연출 설정")]
        [SerializeField] private float expAnimationDelay = 0.5f;
        [SerializeField] private float slotAnimationInterval = 0.3f;

        private ShootingRuntimeData runtimeData;

        void Start()
        {
            // RuntimeData 가져오기
            runtimeData = GetRuntimeData();

            if (runtimeData == null)
            {
                Debug.LogError("[STResultController] ShootingRuntimeData를 찾을 수 없습니다!");
                return;
            }

            // 결과 표시
            ShowResult();

            // 버튼 이벤트 연결
            if (exitButton != null)
                exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private ShootingRuntimeData GetRuntimeData()
        {
            // STDataManage에서 RuntimeData 가져오기
            var srd = STDataManage.Instance?.RuntimeData;
            if (srd != null)
            {
                return srd;
            }

            // 백업: StageManager에서 가져오기
            var stage = StageManager.Instance?.GetCurrentStage() as ShootingStage;
            if (stage != null && stage.RuntimeData is ShootingRuntimeData stageRd)
            {
                return stageRd;
            }

            Debug.LogWarning("[STResultController] RuntimeData를 찾을 수 없음");
            return null;
        }

        private void ShowResult()
        {
            // 타이틀
            if (titleText != null)
                titleText.text = GameResult.IsVictory ? "Operation Clear!" : "Operation Failed";

            // 골드 표시 (기본 200 * 배율)
            int totalGold = GameResult.CalculateTotalGold();
            if (goldText != null)
                goldText.text = $"+ {totalGold}G";

            Debug.Log($"[STResultController] 배율: {GameResult.DifficultyMultiplier}배");
            Debug.Log($"[STResultController] 경험치: {GameResult.CalculateTotalExp()} (기본 {GameResult.BaseExpReward} * {GameResult.DifficultyMultiplier})");
            Debug.Log($"[STResultController] 골드: {totalGold} (기본 {GameResult.BaseGoldReward} * {GameResult.DifficultyMultiplier})");

            // 캐릭터 슬롯 초기화
            SetupCharacterSlots();

            // 경험치 애니메이션 시작
            StartCoroutine(PlayExpAnimation());
        }

        private void SetupCharacterSlots()
        {
            if (runtimeData == null) return;

            // 모든 출전 캐릭터에게 동일한 경험치 지급 (기본 100 * 배율)
            int expPerCharacter = GameResult.CalculateTotalExp();

            Debug.Log($"[STResultController] 캐릭터당 경험치: {expPerCharacter}");

            for (int i = 0; i < characterSlots.Count; i++)
            {
                if (characterSlots[i] == null) continue;

                if (i < GameResult.ParticipatingCharacterIds.Count)
                {
                    int charId = GameResult.ParticipatingCharacterIds[i];

                    // 캐릭터 데이터 찾기
                    STCharacterData charData = allCharacterData.Find(c => c.characterId == charId);
                    OwnedCharacterInfo ownedInfo = runtimeData.GetCharacterInfo(charId);

                    if (charData != null && ownedInfo != null)
                    {
                        characterSlots[i].Setup(charData, ownedInfo, expPerCharacter);
                        Debug.Log($"[STResultController] 슬롯 {i}: {charData.characterName} Lv.{ownedInfo.level} +{expPerCharacter}exp");
                    }
                    else
                    {
                        characterSlots[i].gameObject.SetActive(false);
                        Debug.LogWarning($"[STResultController] 캐릭터 ID {charId} 데이터를 찾을 수 없음");
                    }
                }
                else
                {
                    // 빈 슬롯
                    characterSlots[i].gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator PlayExpAnimation()
        {
            yield return new WaitForSeconds(expAnimationDelay);

            // 각 캐릭터 슬롯에 경험치 적용
            for (int i = 0; i < characterSlots.Count; i++)
            {
                if (characterSlots[i] == null || !characterSlots[i].gameObject.activeSelf)
                    continue;

                if (i < GameResult.ParticipatingCharacterIds.Count)
                {
                    int charId = GameResult.ParticipatingCharacterIds[i];
                    OwnedCharacterInfo ownedInfo = runtimeData?.GetCharacterInfo(charId);

                    if (ownedInfo != null)
                    {
                        characterSlots[i].ApplyExpReward(ownedInfo);
                    }

                    yield return new WaitForSeconds(slotAnimationInterval);
                }
            }

            // 데이터 저장
            SaveData();
        }

        private void SaveData()
        {
            if (runtimeData != null)
            {
                Debug.Log("[STResultController] 결과 데이터 저장 완료");

                // TODO: 골드도 저장하려면 runtimeData에 Gold 필드 추가 필요
                // runtimeData.Gold += GameResult.CalculateTotalGold();
            }
        }

        private void OnExitButtonClicked()
        {
            // 로비로 이동
            StageManager.Instance?.GetCurrentStage()?.LoadStage(LUP.Define.StageKind.ST, 0);
        }
    }
}