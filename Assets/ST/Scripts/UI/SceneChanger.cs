using UnityEngine;
using UnityEngine.SceneManagement;
namespace ST
{

    public class SceneChanger : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
        public void LoadLobbyScene()
        {
            SceneManager.LoadScene("LobbyScene");
        }
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
        public void QuitGame()
        {
            Debug.Log("게임 종료!");
            Application.Quit();
        }
    }
}