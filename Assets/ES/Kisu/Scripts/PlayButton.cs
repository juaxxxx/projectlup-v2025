using UnityEngine;
using UnityEngine.SceneManagement;

namespace ES
{
    public class PlayButton : MonoBehaviour
    {

        public void LoadInGame()
        {
            SceneManager.LoadScene("GameScene 1031");
        }
    }

}
