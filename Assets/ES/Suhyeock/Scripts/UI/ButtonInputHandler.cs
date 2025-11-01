using UnityEngine;
using UnityEngine.UI;

public class ButtonInputHandler : MonoBehaviour
{
    public PlayerBlackboard blackboard;
    public GameObject pausePanel;

    public void OnInteractPressed()
    {
        blackboard.isInteractionButtonPressed = true;
      
    }

    public void OnReloadPressed()
    {
        if (blackboard.gun.state == GunState.RELOADING)
            return;
        blackboard.isReloadButtonPressed = true;
    }

    public void OnPausePressed()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void OnClosePressedInPausePanel()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
}
