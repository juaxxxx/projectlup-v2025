using UnityEngine;

namespace LUP.ES
{
    public class CharacterSelector : MonoBehaviour
    {
        public ExtractionShooterStage extractionShooterStage;
        public GameObject[] characters;
        private int currentIndex = 0;

        void Start()
        {
            switch (currentIndex)
            {
                case 0:
                    extractionShooterStage.RuntimeData.PlayerID = 0;
                    extractionShooterStage.RuntimeData.WeaponID = 0;
                    break;
                case 1:
                    extractionShooterStage.RuntimeData.PlayerID = 1;
                    extractionShooterStage.RuntimeData.WeaponID = 7;
                    break;
                case 2:
                    extractionShooterStage.RuntimeData.PlayerID = 2;
                    extractionShooterStage.RuntimeData.WeaponID = 8;
                    break;
                default:
                    break;
            }
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
            ShowCharacter(currentIndex);
        }

        public void NextCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % characters.Length;
            ShowCharacter(currentIndex);
            switch (currentIndex)
            {
                case 0:
                    extractionShooterStage.RuntimeData.PlayerID = 0;
                    extractionShooterStage.RuntimeData.WeaponID = 0;
                    break;
                case 1:
                    extractionShooterStage.RuntimeData.PlayerID = 1;
                    extractionShooterStage.RuntimeData.WeaponID = 7;
                    break;
                case 2:
                    extractionShooterStage.RuntimeData.PlayerID = 2;
                    extractionShooterStage.RuntimeData.WeaponID = 8;
                    break;
                default:
                    break;
            }
            Debug.Log("ÇöÀç ÀÎµ¦½º: " + currentIndex + "============");
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void PrevCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
            ShowCharacter(currentIndex);
            switch (currentIndex)
            {
                case 0:
                    extractionShooterStage.RuntimeData.PlayerID = 0;
                    extractionShooterStage.RuntimeData.WeaponID = 0;
                    break;
                case 1:
                    extractionShooterStage.RuntimeData.PlayerID = 1;
                    extractionShooterStage.RuntimeData.WeaponID = 7;
                    break;
                case 2:
                    extractionShooterStage.RuntimeData.PlayerID = 2;
                    extractionShooterStage.RuntimeData.WeaponID = 8;
                    break;
                default:
                    break;
            }
            Debug.Log("ÇöÀç ÀÎµ¦½º: " + currentIndex + "============");
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void ShowCharacter(int index)
        {
            for (int i = 0; i < characters.Length; i++)
                characters[i].SetActive(i == index);
        }
    }
}