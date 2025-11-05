using UnityEngine;

namespace RL
{
    public class sceneSelectBtnPanel : MonoBehaviour
    {
        TextImageBtn[] textImageBtns;
        PannelController pannelController;

        PanelType currPanle = PanelType.MAX;

        public float expandeWidth = 0.3f;

        private void Awake()
        {
            textImageBtns = GetComponentsInChildren<TextImageBtn>();
        }

        private void Start()
        {
            pannelController = FindFirstObjectByType<PannelController>();

            if( pannelController != null )
            {
                pannelController.OnPanelSwitched += Moveto;
            }

            for(int i = 0; i < textImageBtns.Length; i++)
            {
                int index = i;
                textImageBtns[i].Init();

                textImageBtns[index].button.onClick.AddListener(()=> CallMoveTo((PanelType)index));
            }
        }

        void Moveto(PanelType switchedtype)
        {
            if(currPanle != switchedtype)
            {
                int index = (int)switchedtype;

                highlightPanel(index);

                currPanle = switchedtype;
            }
            
        }

        void CallMoveTo(PanelType switchedtype)
        {
            if (currPanle != switchedtype)
            {
                pannelController.SwitchPannelTo(switchedtype);

                int index = (int)switchedtype;

                highlightPanel(index);

                currPanle = switchedtype;
            }
                
        }

        void highlightPanel(int index)
        {
            int count = textImageBtns.Length;
            if (count == 0) return;

            float totalWidth = 1.0f;
            float normalWidth = (totalWidth - expandeWidth) / (count - 1);

            float currentAnchor = 0f;

            for (int i = 0; i < count; i++)
            {
                RectTransform rect = textImageBtns[i].GetComponent<RectTransform>();

                float width = (i == index) ? expandeWidth : normalWidth;

                float anchorMinX = currentAnchor;
                float anchorMaxX = currentAnchor + width;
                currentAnchor += width;

                rect.anchorMin = new Vector2(anchorMinX, 0f);
                rect.anchorMax = new Vector2(anchorMaxX, 1f);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                textImageBtns[i].SetActive(false);
            }

            textImageBtns[index].SetActive(true);
        }
    }
}

