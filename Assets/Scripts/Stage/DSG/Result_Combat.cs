using UnityEngine;

namespace DSG
{
    public class Result_Combat : MonoBehaviour
    {
        [SerializeField]
        private DataCenter dataCenter;

        [Header("MVP 슬롯들")]
        [SerializeField] private CharacterSlotUI[] mvpSlots;

        [Header("결과 데이터 (전투 결과에서 받아온 캐릭터들)")]
        [SerializeField] private OwnedCharacterInfo[] mvpCharacters;

        void Start()
        {
            ApplyResults();
        }

        public void ApplyResults()
        {
            int count = Mathf.Min(mvpSlots.Length, mvpCharacters.Length);

            for (int i = 0; i < count; i++)
            {
                CharacterData data = dataCenter.FindCharacterData(mvpCharacters[i].characterID);
                CharacterModelData modelData = dataCenter.FindCharacterModel(mvpCharacters[i].characterModelID);

                if (mvpSlots[i] != null)
                    mvpSlots[i].SetData(data, modelData);
            }
        }
    }
}