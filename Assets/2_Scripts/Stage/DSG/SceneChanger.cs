using UnityEngine;
using UnityEngine.SceneManagement;

namespace LUP.DSG
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeToDeckEdit()
        {

            SceneManager.LoadScene("DeckEditsScene");
        }

        public void ChangeToBattle()
        {
            SceneManager.LoadScene("DeckBattleScene");
        }

        public void ChangeToMain()
        {
            SceneManager.LoadScene("DeckMainScene");
        }

        public void ChangeToResult()
        {
            SceneManager.LoadScene("DeckBattleResultScene");
        }
    }
}