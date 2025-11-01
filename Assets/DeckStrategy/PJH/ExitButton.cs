using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // UI 버튼의 OnClick 이벤트에 연결
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("DeckMainScene");
    }
}