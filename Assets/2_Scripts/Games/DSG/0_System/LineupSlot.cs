using UnityEngine;
using System;

namespace LUP.DSG
{
    public class LineupSlot : MonoBehaviour
    {
        public bool isPlaced { get; private set; } = false;
        public Character character { get; private set; }

        public Transform AttackedPosition;
        public Transform FocusedPosition;

        public void SetCharacter(Character newCharacter)
        {
            ClearCharacter();

            character = newCharacter;
            isPlaced = true;
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
        }

        public void ActivateBattleUI()
        {
            if (character != null)
                character.ActiveBattleUI();
        }
    }
}