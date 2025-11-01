using Roguelike.Util;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemGridLayout : MonoBehaviour, IPanelContentAble
{
    private Vector2 parentViewportSize;
    private GridLayoutGroup gridLayoutGroup;
    public GameObject ItemBoxPrefab;
    public int holizonConstrain;

    private PlatformAdapter platformAdapter;

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
        if(ItemBoxPrefab == null)
        {
            UnityEngine.Debug.LogError("Assin Item Box Prefab");
        }

        gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            UnityEngine.Debug.LogError("Item Panel Grid Layout Group Find Fail");
        }
        return true;
    }

    void FitToParentSize()
    {
        parentViewportSize = gameObject.transform.parent.transform.parent.GetComponent<RectTransform>().rect.size;
        Vector2 ItemBoxSize = new Vector2(parentViewportSize.x, 100);
        gameObject.GetComponent<RectTransform>().sizeDelta = ItemBoxSize;

        Vector2 spacing = gridLayoutGroup.spacing;

        float prefabWidth = (ItemBoxSize.x / holizonConstrain) - 2 * spacing.x;

        gridLayoutGroup.constraintCount = holizonConstrain;
        gridLayoutGroup.cellSize = new Vector2(prefabWidth, prefabWidth);

        LoadPlatFormData();
    }

    void LoadPlatFormData()
    {
        platformAdapter = new PlatformAdapter();

        if (platformAdapter != null)
        {
            platformAdapter.LinkToPlatform();
        }

        ItemData[] InventoryItmes = platformAdapter.inventoryItmeDatas;

        for(int i = 0; i < InventoryItmes.Length; i++)
        {
            GameObject Itembox = Instantiate(ItemBoxPrefab, gameObject.transform);
            Itembox.GetComponent<Image>().sprite = InventoryItmes[i].GetDisplayableImage();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
