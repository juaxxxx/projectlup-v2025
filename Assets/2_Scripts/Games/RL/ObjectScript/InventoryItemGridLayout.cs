using LUP.ST;
using Roguelike.Define;
using Roguelike.Util;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    [System.Serializable]
    public struct TierColorData
    {
        public Color BorderColor;
        public Color BaseColor;
    }

    public class InventoryItemGridLayout : MonoBehaviour, IPanelContentAble
    {

        [SerializeField]
        private TierColorData[] tierColors;

        private Vector2 parentViewportSize;
        private GridLayoutGroup gridLayoutGroup;
        public GameObject EquipBoxPrefab;
        public int holizonConstrain;

        private EquipData[] InventoryItmes;

        private PlatformAdapter platformAdapter = null;

        private PannelController pannelController = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        }

        public bool Init()
        {
            if (EquipBoxPrefab == null)
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

            float prefabWidth = (ItemBoxSize.x / holizonConstrain) - 1.4f * spacing.x;

            gridLayoutGroup.constraintCount = holizonConstrain;
            gridLayoutGroup.cellSize = new Vector2(prefabWidth, prefabWidth);

            LoadInventoryItemData(false);
        }

        public void LoadInventoryItemData(bool bisAlined)
        {
            if(platformAdapter == null)
            {
                platformAdapter = new PlatformAdapter();
                platformAdapter.LinkToPlatform();
            }

            else
            {
                platformAdapter = pannelController.lobbyGameCenter.platformAdapter;
            }

                ClearInventoryGrid();

            RWeaponType playerWeaponType = RWeaponType.None;

            if(pannelController != null)
                playerWeaponType = pannelController.lobbyGameCenter.GetselectedCharacter().weaponType;

            //ItemData[] InventoryItmes = platformAdapter.GetInventoryItems();
            InventoryItmes = platformAdapter.GetInventoryEquips().OrderByDescending(item => (RLItemTier)item.GetExtraInfo()).ThenBy(item => item.equipPos).ToArray();
            for (int i = 0; i < InventoryItmes.Length; i++)
            {
                int index = i;

                if(bisAlined && playerWeaponType != RWeaponType.None)
                {
                    RWeaponType WeaponType = InventoryItmes[i].weaponType;

                    if (playerWeaponType != RWeaponType.None &&
                        playerWeaponType != WeaponType)
                        continue;
                }

                EquipData equipItem = InventoryItmes[i];

                RLItemTier equipTier = (RLItemTier)equipItem.GetExtraInfo();
                RLEquipPos equipPos = equipItem.equipPos;

                GameObject Itembox = Instantiate(EquipBoxPrefab, gameObject.transform);
                InventoryEquipBtn itemTextImageBtn = Itembox.GetComponent<InventoryEquipBtn>();


                //itemTextImageBtn.SetUseDefaultInteractColor(false);

                //if (itemTextImageBtn.Init())
                //{
                //    int index = i;

                //    itemTextImageBtn.btnBackGroundImage.sprite = equipItem.GetDisplayableImage();
                //    itemTextImageBtn.button.onClick.AddListener(() => OnEquipItemBtnClicked(index));
                //}

                itemTextImageBtn.SetEquipButton(equipItem.GetDisplayableImage(), equipPos, equipTier, tierColors[(int)equipTier - 1]);
                itemTextImageBtn.button.onClick.AddListener(() => OnEquipItemBtnClicked(index));
            }

            StartCoroutine(RoguelikeUtil.DelayOneFrame(ReArrnageGidPanle));

        }

        void ClearInventoryGrid()
        {
            Transform gridTransform = gameObject.transform;

            for (int i = gridTransform.childCount - 1; i >= 0; i--)
            {
                GameObject child = gridTransform.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }

            gameObject.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.UpperLeft;
        }

        void ReArrnageGidPanle()
        {
            gameObject.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        }

        void OnEquipItemBtnClicked(int index)
        {
            if (InventoryItmes.Length == 0)
                return;

            if (pannelController == null)
            {
                pannelController = FindFirstObjectByType<PannelController>();
            }

            pannelController.PopEquipPanel(InventoryItmes[index], true);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

