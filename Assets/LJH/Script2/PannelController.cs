using Roguelike.Util;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PanelType
{
    SHOP,
    INVENTORY,
    WORLD,
    AVILITY,
    EVENT,
    MAX
}

public class PannelController : MonoBehaviour
{
    [SerializeField]
    PanelType currentPanel = PanelType.WORLD;

    public PannelInputController inputCacher;
    public Scrollbar mainHorizenScrollBar;
    private ScrollRect mainScrollRect;

    private LobbyContentAblePannel[] lobbyPannels = new LobbyContentAblePannel[(int)PanelType.MAX];
    private Vector2[] panelCenterPosition = new Vector2[(int)PanelType.MAX];

    [HideInInspector]
    public WorldEventBtnScrollPannel LeftEventPanel;
    [HideInInspector]
    public WorldEventBtnScrollPannel RightEventPanel;

    public float switchingDuration = 0.1f;
    private Coroutine scrollCoroutine;

    public float swipeThreshold = 50f;

    float contentWidth = 0.0f;
    float contentPannelhalfWidht = 0.0f;

    [HideInInspector]
    public Vector2 pannelSize;
    Vector2 screenCenter;
    Vector2 minMaxPannelLR;

    public Vector2 touchStartPos;
    public Vector2 touchCurrentPos;
    public Vector2 touchEndPos;
    private Vector2 localStartPos;
    private Vector2 localEndPos;

    void Start()
    {
        if(inputCacher == null ||
            mainHorizenScrollBar == null)
        {
            UnityEngine.Debug.LogError("Bind Input Cacher!!");
        }

        StartCoroutine(RoguelikeUtil.DelayForSeconds(0.5f, () =>  InitPannelCntroller()));

    }

    void InitPannelCntroller()
    {
        LobbyContentAblePannel[] pannelArray = FindObjectsByType<LobbyContentAblePannel>(FindObjectsSortMode.None);

        for(int i = 0; i < pannelArray.Length; i++)
        {
            LobbyContentAblePannel pannel = pannelArray[i];

            if(pannel.GetComponent<ShopPanel>())
            {
                lobbyPannels[(int)PanelType.SHOP] = pannel;
                panelCenterPosition[(int)PanelType.SHOP] = pannel.GetComponent<RectTransform>().anchoredPosition;
                continue;
            }

            if (pannel.GetComponent<InventorPanel>())
            {
                lobbyPannels[(int)PanelType.INVENTORY] = pannel;
                panelCenterPosition[(int)PanelType.INVENTORY] = pannel.GetComponent<RectTransform>().anchoredPosition;
                continue;
            }

            if (pannel.GetComponent<WorldPanel>())
            {
                lobbyPannels[(int)PanelType.WORLD] = pannel;
                panelCenterPosition[(int)PanelType.WORLD] = pannel.GetComponent<RectTransform>().anchoredPosition;
                continue;
            }

            if (pannel.GetComponent<AbilityPanel>())
            {
                lobbyPannels[(int)PanelType.AVILITY] = pannel;
                panelCenterPosition[(int)PanelType.AVILITY] = pannel.GetComponent<RectTransform>().anchoredPosition;
                continue;
            }

            if (pannel.GetComponent<EventPanel>())
            {
                lobbyPannels[(int)PanelType.EVENT] = pannel;
                panelCenterPosition[(int)PanelType.EVENT] = pannel.GetComponent<RectTransform>().anchoredPosition;
                continue;
            }
        }

        WorldEventBtnScrollPannel[] worldEventBtnScrollPannels = FindObjectsByType<WorldEventBtnScrollPannel>(FindObjectsSortMode.None);

        for(int i = 0; i < worldEventBtnScrollPannels.Length; i++)
        {
            if (worldEventBtnScrollPannels[i].isLefPanel)
            {
                LeftEventPanel = worldEventBtnScrollPannels[i];
            }

            else
            {
                RightEventPanel = worldEventBtnScrollPannels[i];
            }
        }

        CalrkParmas();


        SwitchPannelTo(PanelType.WORLD);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchPannelTo(PanelType switchedtype)
    {

        Vector2 pos = panelCenterPosition[(int)switchedtype];
        float targetValue = PositionInterpolation(pos.x);

        if (scrollCoroutine != null)
            StopCoroutine(scrollCoroutine);

        lobbyPannels[(int)switchedtype].MoveTo();
        scrollCoroutine = StartCoroutine(SmoothScrollTo(targetValue, switchedtype));
    }

    private IEnumerator SmoothScrollTo(float targetValue, PanelType targetPanel)
    {
        float startValue = mainHorizenScrollBar.value;
        float elapsed = 0f;

        while (elapsed < switchingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / switchingDuration;

            mainHorizenScrollBar.value = Mathf.Lerp(startValue, targetValue, t);

            yield return null;
        }

        mainHorizenScrollBar.value = targetValue;
        currentPanel = targetPanel;

        SetActiveVerticScroll(lobbyPannels[(int)currentPanel].GetActiveVerticScrollbar());

    }

    public void SetActiveMainHorizon(bool active)
    {
        mainHorizenScrollBar.gameObject.SetActive(active);
    }

    void CalrkParmas()
    {
        contentWidth = lobbyPannels[0].transform.parent.GetComponent<RectTransform>().rect.width;

        contentPannelhalfWidht = lobbyPannels[0].GetComponent<RectTransform>().rect.width * 0.5f;
        minMaxPannelLR = new Vector2(panelCenterPosition[0].x - contentPannelhalfWidht, 
            panelCenterPosition[(int)PanelType.MAX - 1].x + contentPannelhalfWidht);

        pannelSize = lobbyPannels[0].GetComponent<RectTransform>().rect.size;
        screenCenter = new Vector2(pannelSize.x / 2f, pannelSize.y / 2f);

        mainScrollRect = mainHorizenScrollBar.gameObject.transform.parent.GetComponent<ScrollRect>();
    }

    float PositionInterpolation(float currentPos)
    {
        float outputMin = minMaxPannelLR.x + contentPannelhalfWidht;
        float outputMax = minMaxPannelLR.y - contentPannelhalfWidht;

        float t = Mathf.InverseLerp(outputMin, outputMax, currentPos);

        return Mathf.Lerp(0, 1, t);

    }

    public void TouchStart()
    {
        localStartPos = touchStartPos - screenCenter;
        //UnityEngine.Debug.LogWarning(touchStartPos);
    }

    public void TouchEnd()
    {
        localEndPos = touchEndPos - screenCenter;
        //UnityEngine.Debug.LogWarning(touchEndPos);

        Vector2 delta = localEndPos - localStartPos;

        if (Mathf.Abs(delta.x) > swipeThreshold)
        {
            if (delta.x > 0) OnSwipeRight();
            else OnSwipeLeft();
        }
        else
        {
            // 상하 스와이프
            if (delta.y > 0) OnSwipeUp();
            else OnSwipeDown();
        }
    }

    public void OnDrawing()
    {
        Vector2 localCurrentPos = touchCurrentPos - screenCenter;
        //UnityEngine.Debug.Log(touchCurrentPos);

    }

    public void SetActiveVerticScroll(Scrollbar newbar)
    {
        inputCacher.targetVerticScrollbar = newbar;
    }

    void OnSwipeRight()
    {
        if (currentPanel <= 0)
            return;

        SwitchPannelTo(currentPanel - 1);
    }

    void OnSwipeLeft()
    {
        if (currentPanel >= PanelType.MAX - 1)
            return;

        SwitchPannelTo(currentPanel + 1);
    }

    public void OnSubPannelErase()
    {
        lobbyPannels[(int)currentPanel].OnSubPanelErase();
        SetActiveInputcacher(true);
    }

    public void SetActiveInputcacher(bool active)
    {
        inputCacher.gameObject.SetActive(active);
    }

    public void SetAllMainScrollActive(bool active)
    {
        inputCacher.gameObject.SetActive(active);
        mainScrollRect.horizontal = active;
    }

    public void DisableWorldEventSubPanel()
    {
        if(LeftEventPanel)
        {
            LeftEventPanel.StopScroll();
        }
        
        if(RightEventPanel)
        {
            RightEventPanel.StopScroll();
        }
        
    }

    void OnSwipeUp()
    {

    }

    void OnSwipeDown()
    {

    }

}
