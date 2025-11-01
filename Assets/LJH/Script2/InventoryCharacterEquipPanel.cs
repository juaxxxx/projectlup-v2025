using Roguelike.Util;
using UnityEngine;

public class InventoryCharacterEquipPanel : MonoBehaviour, IPanelContentAble
{
    private Vector2 parentViewportSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    void Start()
    {
        Init();

        StartCoroutine(RoguelikeUtil.DelayOneFrame(() =>
        {
            showPanel();
        }
        ));
    }

    public bool Init()
    {
        return true;
    }

    void showPanel()
    {
        parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
        Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, parentViewportSize.y * 0.6f);
        gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
