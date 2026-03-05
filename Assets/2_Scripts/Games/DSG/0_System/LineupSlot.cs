using UnityEngine;
using System;

namespace LUP.DSG
{
    public class LineupSlot : MonoBehaviour
    {
        public bool isPlaced { get; private set; } = false;
        public Character character { get; private set; }
        public Transform slotTransform { get; private set; }

        public Transform AttackedPosition;
        public Transform FocusedPosition;

        public event Action OnCPUpdated;

        private void Awake()
        {
            slotTransform = transform;
        }

        // 객체 생성 로직이 사라지고, 단순히 팩토리나 Presenter가 만든 객체를 세팅만 함
        public void SetCharacterView(Character newCharacter)
        {
            ClearCharacter(); // 기존 캐릭터 정리

            character = newCharacter;
            isPlaced = true;
            OnCPUpdated?.Invoke();
        }

        public void ClearCharacter()
        {
            if (character != null)
            {
                character.ReleaseCharacterUI();
                Destroy(character.gameObject);
                character = null;
            }
            isPlaced = false;
            OnCPUpdated?.Invoke();
        }

        public void ActivateBattleUI()
        {
            if (character != null)
                character.ActiveBattleUI();
        }
    }
}