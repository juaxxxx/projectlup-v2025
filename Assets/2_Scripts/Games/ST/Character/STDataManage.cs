using System.Collections.Generic;
using UnityEngine;

namespace LUP.ST
{
    public class STDataManage : MonoBehaviour
    {
        public static STDataManage Instance { get; private set; }

        [Header("모든 캐릭터 데이터 (인스펙터에서 할당)")]
        [SerializeField] private List<STCharacterData> allCharacterDatas;

        public ShootingRuntimeData RuntimeData { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadRuntimeData();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LoadRuntimeData()
        {
            RuntimeData = JsonDataHelper.LoadData<ShootingRuntimeData>("shooting_runtime.json");
            Debug.Log($"[STDataManager] 로드 완료 - 보유 캐릭터: {RuntimeData.OwnedCharacterList.Count}명");
        }

        public void SaveRuntimeData()
        {
            JsonDataHelper.SaveData(RuntimeData, "shooting_runtime.json");
            Debug.Log("[STDataManager] 저장 완료");
        }

        // characterId로 ScriptableObject 찾기
        public STCharacterData GetCharacterData(int characterId)
        {
            return allCharacterDatas.Find(c => c.characterId == characterId);
        }

        // 보유 캐릭터 목록 가져오기
        public List<STCharacterData> GetOwnedCharacters()
        {
            var result = new List<STCharacterData>();
            foreach (var owned in RuntimeData.OwnedCharacterList)
            {
                var data = GetCharacterData(owned.characterId);
                if (data != null)
                    result.Add(data);
            }
            return result;
        }

        // 팀 슬롯 설정
        public void SetTeamSlot(int slotIndex, int characterId)
        {
            if (slotIndex >= 0 && slotIndex < 5)
            {
                RuntimeData.TeamSlots[slotIndex] = characterId;
                SaveRuntimeData();
            }
        }

        // 현재 팀 가져오기
        public STCharacterData[] GetCurrentTeam()
        {
            var team = new STCharacterData[5];
            for (int i = 0; i < 5; i++)
            {
                int charId = RuntimeData.TeamSlots[i];
                team[i] = (charId > 0) ? GetCharacterData(charId) : null;
            }
            return team;
        }
    }
}