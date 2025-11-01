using UnityEngine;
using UnityEngine.UI;
using Roguelike.Define;

public class WorldPanel : LobbyContentAblePannel
{
    public ChapterSelectionScrollPanel chapterSelectionScrollPanel;
    FloatingChapterImage chapterImageBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();

        //왜 못 찾아!
        //chapterSelectionScrollPanel = GetComponentsInChildren<ChapterSelectionScrollPanel>();
        chapterImageBtn = GetComponentInChildren<FloatingChapterImage>();

        chapterImageBtn.gameObject.GetComponent<Button>().onClick.AddListener(()=> OnChapterImageBtnClicked());

    }

    void OnChapterImageBtnClicked()
    {
        LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

        chapterSelectionScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Horizontal);
        chapterSelectionScrollPanel.OpenPanel(lobbyGameCenter.chapterDatas, DisplayableDataType.ChapterData, lobbyGameCenter.GetChapterDisplayedOffset());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
