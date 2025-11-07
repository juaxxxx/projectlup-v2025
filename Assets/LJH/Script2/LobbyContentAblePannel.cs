using UnityEngine;
using UnityEngine.UI;

namespace RL
{
    public abstract class LobbyContentAblePannel : MonoBehaviour
    {
        protected RectTransform viewportRectTransform;
        protected RectTransform contentPanelRectTransform;
        protected PannelController pannelController;

        protected Scrollbar activatedVecticScrollbar;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            Canvas.ForceUpdateCanvases();

            viewportRectTransform = transform.parent.transform.parent.GetComponent<RectTransform>();

            if (viewportRectTransform == null)
            {
                UnityEngine.Debug.LogError("Fail to Find ViewportRectTransform");
            }

            Vector2 viewportSize = viewportRectTransform.rect.size;

            if (viewportSize == Vector2.zero)
            {
                UnityEngine.Debug.LogError("viewportSize is 0");
            }

            contentPanelRectTransform = gameObject.GetComponent<RectTransform>();
            contentPanelRectTransform.sizeDelta = viewportSize;

            pannelController = FindFirstObjectByType<PannelController>();

            if (pannelController == null)
            {
                UnityEngine.Debug.LogError("Find PannelController Fail");
            }
        }

        virtual public void OnSubPanelErase()
        {

        }

        void ResetPanel()
        {

        }

        public void Touching(Vector2 touchPos)
        {

        }

        private void OnDisable()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Scrollbar GetActiveVerticScrollbar()
        {
            return activatedVecticScrollbar;
        }

        virtual public void MoveTo()
        {

        }
    }
}

