using Roguelike.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class InventoryItemGridLayout : MonoBehaviour, IPanelContentAble
    {
        private Vector2 parentViewportSize;
        private GridLayoutGroup gridLayoutGroup;
        public GameObject ItemBoxPrefab;
        public int holizonConstrain;

        private EquipData[] InventoryItmes;

        private PlatformAdapter platformAdapter;

        private PannelController pannelController = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        public bool Init()
        {
            if (ItemBoxPrefab == null)
            {
                UnityEngine.Debug.LogError("Assin Item Box Prefab");
            }

            gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup == null)
            {
                UnityEngine.Debug.LogError("Item Panel Grid Layout Group Find Fail");
            }

            FitToParentSize();

            pannelController = FindFirstObjectByType<PannelController>();

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

            LoadInventoryItemData();
        }

        void LoadInventoryItemData()
        {
            platformAdapter = new PlatformAdapter();

            if (platformAdapter != null)
            {
                platformAdapter.LinkToPlatform();
            }

            ClearInventoryGrid();

            //ItemData[] InventoryItmes = platformAdapter.GetInventoryItems();
            InventoryItmes = platformAdapter.GetInventoryEquips();
            for (int i = 0; i < InventoryItmes.Length; i++)
            {
                GameObject Itembox = Instantiate(ItemBoxPrefab, gameObject.transform);
                TextImageBtn itemTextImageBtn = Itembox.GetComponent<TextImageBtn>();
                itemTextImageBtn.SetUseDefaultInteractColor(false);

                if (itemTextImageBtn.Init())
                {
                    int index = i;

                    itemTextImageBtn.btnBackGroundImage.sprite = InventoryItmes[i].GetDisplayableImage();
                    itemTextImageBtn.button.onClick.AddListener(() => OnEquipItemBtnClicked(index));
                }

            }

        }

        void ClearInventoryGrid()
        {
            Transform gridTransform = gameObject.transform;

            for (int i = gridTransform.childCount - 1; i >= 0; i--)
            {
                GameObject child = gridTransform.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }
        }

        void OnEquipItemBtnClicked(int index)
        {
            if (InventoryItmes.Length == 0)
                return;

            if (pannelController == null)
            {
                pannelController = FindFirstObjectByType<PannelController>();
            }

            pannelController.PopEquipPanel(InventoryItmes[index]);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

