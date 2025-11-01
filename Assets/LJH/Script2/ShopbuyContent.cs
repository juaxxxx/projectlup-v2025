using UnityEngine;
using UnityEngine.UI;
using Roguelike.Util;

public class ShopbuyContent : MonoBehaviour , IPanelContentAble
{
    public int btnNum;

    [HideInInspector]
    public TextImageBtn[] buttons;

    private GameObject parentContent;

    private Vector2 minRatio;
    private Vector2 maxRatio;

    public bool Init()
    {
        buttons = GetComponentsInChildren<TextImageBtn>();

        if(buttons.Length != btnNum)
        {
            UnityEngine.Debug.LogError("Buy Btn Num not match");
        }

        for(int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            if(buttons[i].Init())
            {
                buttons[i].button.onClick.AddListener(() => OnBuyBtnClicked(index));
                CanvasGroup buttonCanvasGroup = buttons[i].gameObject.AddComponent<CanvasGroup>();
                buttonCanvasGroup.interactable = true;
                buttonCanvasGroup.ignoreParentGroups = true;

            }
            
        }

        parentContent = gameObject.transform.parent.gameObject;

        return true;
    }

    public void SetRatio(Vector2 anchorMin, Vector2 anchorMax)
    {
        minRatio = anchorMin;
        maxRatio = anchorMax;

        StartCoroutine(RoguelikeUtil.DelayOneFrame(() => FitToParentContent()));
    }

    void OnBuyBtnClicked(int index)
    {
        UnityEngine.Debug.Log("Clicked");
    }

    void FitToParentContent()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.anchorMin = minRatio;
        rectTransform.anchorMax = maxRatio;


        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        Vector3 pos = rectTransform.localPosition;
        pos.z = 0f;
        rectTransform.localPosition = pos;
    }

}
