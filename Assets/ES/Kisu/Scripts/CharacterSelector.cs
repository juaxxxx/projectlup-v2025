using UnityEngine;

namespace ES
{
    public class CharacterSelector : MonoBehaviour
    {
        public GameObject[] characters;
        private int currentIndex = 0;

        void Start()
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
            ShowCharacter(currentIndex);
        }

        public void NextCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % characters.Length;
            ShowCharacter(currentIndex);
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void PrevCharacter()
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
            ShowCharacter(currentIndex);
            PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        }

        public void ShowCharacter(int index)
        {
            for (int i = 0; i < characters.Length; i++)
                characters[i].SetActive(i == index);
        }
    }
}