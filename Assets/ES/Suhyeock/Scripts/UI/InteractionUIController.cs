using UnityEngine;
using UnityEngine.UI;

namespace ES
{
    public class InteractionUIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject InteractionPromptPrefab;
        [SerializeField]
        private GameObject InteractionTimerPrefab;
        [SerializeField]
        private GameObject InteractionCanvas;
        [SerializeField]
        private float YOffset = 40.0f;

        private Image InteractionTimerUIImage;
        private Image InteractionTimerImage;

        private Image InteractionPromptImage;
        private Camera MainCamera;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            MainCamera = Camera.main;
            InitUI();
        }

        private void FixedUpdate()
        {
            if (InteractionPromptImage.IsActive() == true)
            {
                Vector3 ScreenPostion = MainCamera.WorldToScreenPoint(transform.position);
                ScreenPostion.y += YOffset;
                InteractionPromptImage.rectTransform.position = ScreenPostion;
            }

            if (InteractionTimerUIImage.IsActive() == true)
            {
                Vector3 ScreenPostion = MainCamera.WorldToScreenPoint(transform.position);
                ScreenPostion.y += YOffset;
                InteractionTimerUIImage.rectTransform.position = ScreenPostion;
            }
        }
        void InitUI()
        {
            GameObject UIInstance1 = Instantiate(InteractionPromptPrefab, InteractionCanvas.transform);
            InteractionPromptImage = UIInstance1.GetComponent<Image>();
            HideInteractionPrompt();

            GameObject UIInstance2 = Instantiate(InteractionTimerPrefab, InteractionCanvas.transform);
            InteractionTimerUIImage = UIInstance2.GetComponent<Image>();

            Transform child = UIInstance2.transform.Find("InteractionTimer");
            InteractionTimerImage = child.GetComponent<Image>();
            InteractionTimerImage.fillAmount = 1;
            HideInteractionTimerUI();

        }

        public void UpdateInteractionTimerUI(float interactionDuration, float currentTime)
        {
            InteractionTimerImage.fillAmount = currentTime / interactionDuration;
            //Debug.Log(InteractionTimerImage.fillAmount);
        }

        public void ShowInteractionPrompt()
        {
            InteractionPromptImage.gameObject.SetActive(true);
        }

        public void HideInteractionPrompt()
        {
            InteractionPromptImage.gameObject.SetActive(false);
        }

        public void ShowInteractionTimerUI()
        {
            InteractionTimerUIImage.gameObject.SetActive(true);
        }

        public void HideInteractionTimerUI()
        {
            InteractionTimerUIImage.gameObject.SetActive(false);
        }
    }
}
