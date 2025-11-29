using LUP;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ST
{

    public class SceneChanger : MonoBehaviour
    {
        public void LoadGameScene()
        {
            //SceneManager.LoadScene("GameScene");
            StageManager.Instance.GetCurrentStage().LoadStage(LUP.Define.StageKind.ST, 1);
        }
        public void LoadLobbyScene()
        {
            //SceneManager.LoadScene("LobbyScene");
            StageManager.Instance.GetCurrentStage().LoadStage(LUP.Define.StageKind.ST, 0);
        }
        public void LoadResultScene()
        {
            StageManager.Instance.GetCurrentStage().LoadStage(LUP.Define.StageKind.ST, 2);
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





