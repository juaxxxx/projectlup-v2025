using UnityEngine;

namespace LUP.ST
{
    public static class STSaveHandler
    {
        private const string FILE_NAME = "shooting_runtime.json";
        public static STSaveDataContainer CurrentData = new STSaveDataContainer();

        public static void Load()
        {
            CurrentData = JsonDataHelper.LoadData<STSaveDataContainer>(FILE_NAME);
            Debug.Log($"[STSaveHandler] 로드 완료 - 캐릭터 {CurrentData.characterList.Count}명, 팀 {CurrentData.currentTeam.Count}슬롯");
        }

        public static void Save()
        {
            CurrentData.filename = FILE_NAME;
            JsonDataHelper.SaveData(CurrentData, FILE_NAME);
            Debug.Log("[STSaveHandler] 저장 완료");
        }

        // 캐릭터 레벨 가져오기
        public static int GetCharacterLevel(int characterId)
        {
            var data = CurrentData.characterList.Find(c => c.characterId == characterId);
            return data?.level ?? 1;
        }

        // 경험치 추가 & 레벨업
        public static void AddExp(int characterId, int exp)
        {
            var data = CurrentData.characterList.Find(c => c.characterId == characterId);
            if (data == null)
            {
                data = new CharacterLevelData { characterId = characterId, level = 1, currentExp = 0 };
                CurrentData.characterList.Add(data);
            }

            data.currentExp += exp;
            // TODO: 레벨업 로직 추가
        }
    }
}