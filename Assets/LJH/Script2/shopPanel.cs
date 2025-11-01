using NUnit.Framework;
using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : LobbyContentAblePannel
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Tab1Panel;
    public GameObject Tab2Panel;
    public GameObject Tab3Panel;

    public GameObject shopSelectBtnsPanel;

    private List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    private List<TextImageBtn> tabBtns = new List<TextImageBtn>();
    private List<Scrollbar> tabsMovingVerticScrollBar = new List<Scrollbar>();

    new void Start()
    {
        base.Start();

        StartCoroutine(RoguelikeUtil.DelayOneFrame(() => PostPanelInitShop()));
    }

    void PostPanelInitShop()
    {

        InitParams();

        HideAllPanel();

        ActiveShopTabList(0);

        SetAllChildVerticScrollPadding();
    }

    void InitParams()
    {
        Scrollbar tab1VerticScroll = Tab1Panel.GetComponentInChildren<Scrollbar>();
        Scrollbar tab2VerticScroll = Tab2Panel.GetComponentInChildren<Scrollbar>();
        Scrollbar tab3VerticScroll = Tab3Panel.GetComponentInChildren<Scrollbar>();

        CanvasGroup tab1CanvasGroup = Tab1Panel.GetComponentInChildren<CanvasGroup>();
        CanvasGroup tab2CanvasGroup = Tab2Panel.GetComponentInChildren<CanvasGroup>();
        CanvasGroup tab3CanvasGroup = Tab3Panel.GetComponentInChildren<CanvasGroup>();

        ButtonRule[] tabSelectionBtns = shopSelectBtnsPanel.GetComponentsInChildren<ButtonRule>();

        if (tab1VerticScroll && tab2VerticScroll && tab3VerticScroll)
        {
            {
                tabsMovingVerticScrollBar.Clear();

                tabsMovingVerticScrollBar.Add(tab1VerticScroll);
                tabsMovingVerticScrollBar.Add(tab2VerticScroll);
                tabsMovingVerticScrollBar.Add(tab3VerticScroll);
            }
            

            {
                canvasGroups.Clear();

                canvasGroups.Add(tab1CanvasGroup);
                canvasGroups.Add(tab2CanvasGroup);
                canvasGroups.Add(tab3CanvasGroup);
            }
            

            {
                tabBtns.Clear();

                for(int i = 0; i < tabSelectionBtns.Length; i++)
                {
                    ButtonRole target = (ButtonRole)((int)ButtonRole.ShopTabBtn1 + i);

                    for(int n = 0; i < tabSelectionBtns.Length; n++)
                    {
                        if (tabSelectionBtns[n].buttonRole == target)
                        {
                            TextImageBtn textImageBtn = tabSelectionBtns[n].gameObject.GetComponent<TextImageBtn>();
                            if(textImageBtn.Init())
                            {
                                tabBtns.Add(tabSelectionBtns[n].gameObject.GetComponent<TextImageBtn>());
                                break;
                            }

                            else
                            {
                                UnityEngine.Debug.LogError("Init Show Tab TextImageBtn Fail");
                            }
                            
                        }

                    }

                    int index = i;
                    tabBtns[i].button.onClick.AddListener(() => OnTabClicked(index));

                }
            }
            
        }
    }

    public void ActiveShopTabList(int index)
    {
        HideAllPanel();

        if (index == 0)
        {
            canvasGroups[0].alpha = 1;
            activatedVecticScrollbar = tabsMovingVerticScrollBar[0];
        }

        else if(index == 1)
        {
            canvasGroups[1].alpha = 1;
            activatedVecticScrollbar = tabsMovingVerticScrollBar[1];
        }

        else if(index==2)
        {
            canvasGroups[2].alpha = 2;
            activatedVecticScrollbar = tabsMovingVerticScrollBar[2];
        }

        activatedVecticScrollbar.value = 1;

        pannelController.SetActiveVerticScroll(activatedVecticScrollbar);

        tabBtns[index].SetActive(true);
    }

    void HideAllPanel()
    {
        for(int i = 0; i < canvasGroups.Count; i++)
        {
            canvasGroups[i].alpha = 0;
            tabBtns[i].SetActive(false);
        }
    }

    void OnTabClicked(int index)
    {
        ActiveShopTabList(index);
    }

    void SetAllChildVerticScrollPadding()
    {
        VerticalLayoutGroup[] verticalLayoutGroups = GetComponentsInChildren<VerticalLayoutGroup>();

        for(int i = 0; i < verticalLayoutGroups.Length; i++)
        {
            verticalLayoutGroups[i].padding.top = (int)(viewportRectTransform.rect.size.y * 0.08f);
        }
    }

    public override void MoveTo()
    {
        ActiveShopTabList(0);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
