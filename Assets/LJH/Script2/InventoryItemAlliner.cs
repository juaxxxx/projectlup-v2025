using Roguelike.Define;
using Roguelike.Util;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemAlliner : MonoBehaviour, IPanelContentAble
{
    private Button alingBtn;
    private Button bagPreferedBtn;
    private Button blackSmithBtn;

    private Vector2 parentViewportSize;

    bool Align = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();

        StartCoroutine(RoguelikeUtil.DelayOneFrame(() =>
        {
            FitToParentSize();
        }
        ));
    }

    public bool Init()
    {
        ButtonRule[] buttonRules = GetComponentsInChildren<ButtonRule>();

        for(int i = 0; i < buttonRules.Length; i++)
        {
            if(buttonRules[i].buttonRole == ButtonRole.InventoryAlignBtn)
            {
                alingBtn = buttonRules[i].gameObject.GetComponent<Button>();
                alingBtn.onClick.AddListener(OnAlignBtnClicked);
            }

            else if (buttonRules[i].buttonRole == ButtonRole.BagPreferBtn)
            {
                bagPreferedBtn = buttonRules[i].gameObject.GetComponent<Button>();
                bagPreferedBtn.onClick.AddListener(OnPreferBtnClicked);
            }

            else if (buttonRules[i].buttonRole == ButtonRole.BlackSmithBtn)
            {
                blackSmithBtn = buttonRules[i].gameObject.GetComponent<Button>();
                blackSmithBtn.onClick.AddListener(OnBlackSmithBtnClicked);
            }
        }

        if(alingBtn == null ||
            bagPreferedBtn == null ||
            blackSmithBtn == null)
        {
            UnityEngine.Debug.LogError("Fail To Init Inventory Btns");
        }

        return true;
    }

    void FitToParentSize()
    {
        parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
        Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, parentViewportSize.y * 0.05f);
        gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;
    }

    void OnAlignBtnClicked()
    {
        Align = !Align;

    }

    void OnPreferBtnClicked()
    {

    }

    void OnBlackSmithBtnClicked()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
