using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class SelectedButton : MonoBehaviour
    {
        public Button button { get; private set; }
        [SerializeField]
        private CanvasGroup canvasGroup;

        public bool isSelected = false;

        private void Awake()
        {
            button = GetComponent<Button>();
            canvasGroup.alpha = 0f;
        }

        public void ButtonClicked()
        {
            isSelected = !isSelected;
            canvasGroup.alpha = isSelected ? 1f : 0f;
        }
    }
}