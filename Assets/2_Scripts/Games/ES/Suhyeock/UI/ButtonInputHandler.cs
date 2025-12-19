using UnityEngine;
using UnityEngine.UI;

namespace LUP.ES
{
    public class ButtonInputHandler : MonoBehaviour
    {
        private PlayerBlackboard blackboard;
        public GameObject pausePanel;

        private void Start()
        {
            blackboard = FindAnyObjectByType<PlayerBlackboard>();   
        }
        public void OnInteractPressed()
        {
            blackboard.isInteractionButtonPressed = true;
      
        }

        public void OnReloadPressed()
        {
            if (blackboard.weapon.state == WeaponState.RELOADING)
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
}
