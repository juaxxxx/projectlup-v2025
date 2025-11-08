using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace LUP.DSG
{
    public class LineupSlot : MonoBehaviour
    {
        [SerializeField]
        Character CharacterModelPrefab;

        Transform slotTransform;
        public bool isPlaced = false;
        public Character character { get; private set; }

        public Transform AttackedPosition;

        public event System.Action OnCPUpdated;
        public OwnedCharacterInfo characterInfo { get; private set; }

        void Awake()
        {
            // 토글 버튼의 start가 LineupSlot의 Start보다 먼저 실행되어 모델을 생성하지 못할 수도 있기에 Awake에서 호출
            slotTransform = this.transform;
            character = Instantiate(CharacterModelPrefab, slotTransform);
            character.gameObject.SetActive(false);
        }

        public void SetSelectedCharacter(OwnedCharacterInfo info, bool isEnemy)
        {
            isPlaced = true;
            characterInfo = info;
            character.isEnemy = isEnemy;
            character.SetCharacterData(info);
            OnCPUpdated?.Invoke();
        }

        public void DeselectCharacter()
        {
            isPlaced = false;
            characterInfo = null;
            character.ClearCharacterInfo();
            OnCPUpdated?.Invoke();
        }

        public void ActivateBattleUI()
        {
            character.ActiveBattleUI();
        }

        public void ClearCharacter()
        {
            character = null;
        }
    }

}