using UnityEngine;
using UnityEngine.UI;

public class WorldEventBtnScrollPannel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Scrollbar scrollbar;
    public ScrollRect scrollRect;

    public UnityEngine.UI.Button ExtendBtn;
    private bool isExtendActive = false;

    private RectTransform rectTransform;

    public bool isLefPanel = false;


    void Start()
    {
        if (scrollbar == null)
            UnityEngine.Debug.LogError("Bind Scroll Panel to World Event Panel");

        if (scrollRect == null)
            UnityEngine.Debug.LogError("Bind Scroll View to World Event Panel");

        if (ExtendBtn == null)
            UnityEngine.Debug.LogError("Bind ExtenBtn to World Event Panel");

        rectTransform = GetComponent<RectTransform>();

        if (ExtendBtn == null)
            UnityEngine.Debug.LogError("Fail To Find Rect Transform");

        scrollRect.vertical = false;

        ExtendBtn.onClick.AddListener(() => OnExtendClicked());
    }

    public void MoveScroll(float amount)
    {
        if(isExtendActive)
        {
            scrollRect.vertical = true;
            scrollbar.value -= amount;
        }
        
    }

    public void StopScroll()
    {
        scrollRect.vertical = false;
    }

    void OnExtendClicked()
    {
        isExtendActive = !isExtendActive;

        if(isExtendActive)
        {
            //확장
            rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0.4f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

        }

        else
        {
            //축소
            StopScroll();
            rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0.6f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
