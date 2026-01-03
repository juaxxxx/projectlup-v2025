using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class NotReadyPopup : MonoBehaviour
    {
        private PannelController pannelController;
        public Button NotReadyBtn;
        void Start()
        {
            pannelController = FindFirstObjectByType<PannelController>();

            NotReadyBtn.onClick.AddListener(OpenNotReadyPopUp);
        }

        void OpenNotReadyPopUp()
        {
            pannelController.SetAllMainScrollActive(false);
            pannelController.PopWarningPanel();
        }

    }
}

