using UnityEngine;
using Roguelike.Util;
using static UnityEngine.UI.Image;
using UnityEngine.UI;

namespace LUP.RL
{
    public class EventPanel : LobbyContentAblePannel
    {
        public GameObject eventMapPrefab;
        public GameObject Content;
        public Scrollbar scrollbar;

        public int ContetnNum = 5;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            base.Start();

            StartCoroutine(RoguelikeUtil.DelayOneFrame(DisplayEvents));

            activatedVecticScrollbar = scrollbar;
        }

        void DisplayEvents()
        {
            Content.GetComponent<VerticalLayoutGroup>().padding.top = (int)(contentPanelRectTransform.rect.size.y * 0.08f);

            for (int i = 0; i < ContetnNum; i++)
            {
                GameObject prefab = Instantiate(eventMapPrefab, Content.GetComponent<Transform>());

                prefab.GetComponent<RectTransform>().sizeDelta = new Vector2(contentPanelRectTransform.rect.size.x, 100);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

