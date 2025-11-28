using UnityEngine;
using UnityEngine.UI;
using Roguelike.Define;

namespace LUP.RL
{
    public class WorldPanel : LobbyContentAblePannel
    {
        public ChapterSelectionScrollPanel chapterSelectionScrollPanel;
        public Button patrolBtn;
        FloatingChapterImage chapterImageBtn;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        new void Start()
        {
            base.Start();

            //chapterSelectionScrollPanel = GetComponentsInChildren<ChapterSelectionScrollPanel>();
            chapterImageBtn = GetComponentInChildren<FloatingChapterImage>();

            chapterImageBtn.gameObject.GetComponent<Button>().onClick.AddListener(() => OnChapterImageBtnClicked());


            //Temp
            patrolBtn.onClick.AddListener(TestFuntion);
        }

        void OnChapterImageBtnClicked()
        {
            LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

            chapterSelectionScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Horizontal);
            chapterSelectionScrollPanel.OpenPanel(lobbyGameCenter.chapterDatas, DisplayableDataType.ChapterData, lobbyGameCenter.GetChapterDisplayedOffset());
        }

        void TestFuntion()
        {
            PannelController pannelController = FindFirstObjectByType<PannelController>();
            pannelController.SetAllMainScrollActive(false);
            pannelController.PopWarningPanel();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

