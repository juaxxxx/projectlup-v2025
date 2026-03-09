using UnityEngine;

namespace LUP.ES
{
    public class CharacterSelector : MonoBehaviour
    {
        ExtractionShooterStage extractionShooterStage;
        public GameObject[] characters;
        private int currentIndex = 0;

        public LobbyInventoryCenter inventoryCenter;

        public void Init()
        {
            extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;
            //currentIndex = extractionShooterStage.RuntimeData.PlayerID;
            ShowCharacter(currentIndex);
        }

        public void NextCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % characters.Length;
            ShowCharacter(currentIndex);
            SoundManager.Instance.PlaySFX("OnButton");
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void PrevCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
            ShowCharacter(currentIndex);
            SoundManager.Instance.PlaySFX("OnButton");
            Debug.Log("ÇöÀç ÀÎµ¦½º: " + currentIndex + "============");
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void SelectMeleeCharacter()
        {
            currentIndex = 0;
            ShowCharacter(currentIndex);
        }

        public void SelectRangedCharacter()
        {
            currentIndex = 1;
            ShowCharacter(currentIndex);
        }

        public void SelectThrowingCharacter()
        {
            currentIndex = 2;
            ShowCharacter(currentIndex);
        }
        public void ShowCharacter(int index)
        {
            for (int i = 0; i < characters.Length; i++)
                characters[i].SetActive(i == index);
        }

        public void SaveWeaponAndPlayer()
        {
            extractionShooterStage.RuntimeData.PlayerID = currentIndex;
            switch (currentIndex)
            {
                case 0:
                    if (inventoryCenter.meleeWeaponSlot.item == null)
                        extractionShooterStage.RuntimeData.WeaponID = 1;
                    else
                        extractionShooterStage.RuntimeData.WeaponID = inventoryCenter.meleeWeaponSlot.item.ItemID;
                    break;
                case 1:
                    if (inventoryCenter.rangedWeaponSlot.item == null)
                        extractionShooterStage.RuntimeData.WeaponID = 4;
                    else
                        extractionShooterStage.RuntimeData.WeaponID = inventoryCenter.rangedWeaponSlot.item.ItemID;
                    break;
                case 2:
                    if (inventoryCenter.throwingWeaponSlot.item == null)
                        extractionShooterStage.RuntimeData.WeaponID = 7;
                    else
                        extractionShooterStage.RuntimeData.WeaponID = inventoryCenter.throwingWeaponSlot.item.ItemID;
                    break;
                default:
                    break;
            }
        }
    }
}