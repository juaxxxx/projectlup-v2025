using LUP.ST;
using Roguelike.Define;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.RL
{
    public class FloatItemUICenter : MonoBehaviour
    {
        [SerializeField]
        private GameObject floatingUIPanel;

        [SerializeField]
        private FloatinItemUIPoll floatinItemUIPoll;

        [SerializeField]
        private Sprite EquipIcon;

        private ItemSpawner itemSpawner;
        //private RoguelikeStage roguelikeStage;

        //0은 장비 품복 , 1 2 3 나무 고기 동전
        private Dictionary<int, int> customgainItem = new Dictionary<int, int>();

        private Dictionary<int, FloatingItemPopupImage> activatedPopupImageDict;
        List<int> popupImageOrder = new List<int>();

        private Vector2[] spanwedPositionArray = new Vector2[]
        {
            new Vector2(0f, -50f),
            new Vector2(0f, -175f),
            new Vector2(0f, -300f),
            new Vector2(0f, -425f)
        };

        private void Awake()
        {
            activatedPopupImageDict = new Dictionary<int, FloatingItemPopupImage>();

            for (int i = 0; i < 10; i++)
                popupImageOrder.Add(-1);
        }

        void Start()
        {
            if(floatinItemUIPoll != null)
            {
                floatinItemUIPoll.InitializePool();
            }

            itemSpawner = FindFirstObjectByType<ItemSpawner>();
            if (itemSpawner)
            {
                itemSpawner.OnItemGained += OnGainSpawnableItem;
            }
        }

        void OnGainSpawnableItem(int itemID, int gainedAmount)
        {
            Sprite displayedIcon;
            IItemable GainedItem = ItemManager.Instance.GetItem(itemID);

            int customId = itemID - 10000 > 10000 ? 0 : itemID - 10000;

            if (customId == 0)
            {
                displayedIcon = EquipIcon;
            }

            else
            {
                displayedIcon = GainedItem.Icon;
            }


            int ownedItemAmount = 0;

            if(customgainItem.ContainsKey(customId))
            {
                ownedItemAmount = customgainItem[customId];
            }

            //활설화 중인 Popup 일 경우
            if (activatedPopupImageDict.ContainsKey(customId) && popupImageOrder.Contains(customId))
            {
                activatedPopupImageDict[customId].itemGainedAmount += gainedAmount;
                activatedPopupImageDict[customId].OnGainedAmountChanged(gainedAmount);
            }

            //새로운 Popup 일 경우
            else
            {
                int availidIndex = FindEmptySlot();

                if (availidIndex == -1)
                    return;
                FloatingItemPopupImage floatingItemPopupImage = floatinItemUIPoll.RequestUI();

                Vector2 UISize = floatingItemPopupImage.InitFloatingItemImage(
                        GainedItem.Type == LUP.Define.ItemType.Material ? RLDropItemType.Commodities : RLDropItemType.equipment,
                        customId,
                        displayedIcon,
                        ownedItemAmount,
                        gainedAmount,
                        floatinItemUIPoll
                    );
                floatingItemPopupImage.transform.SetParent(floatingUIPanel.transform, false);

                popupImageOrder[availidIndex] = customId;

                int assingedIndex = popupImageOrder.IndexOf(customId);
                floatingItemPopupImage.rect.anchoredPosition = new Vector2(spanwedPositionArray[assingedIndex].x + UISize.x, spanwedPositionArray[assingedIndex].y);

                floatingItemPopupImage.OnPopupDisApear += OnItemPullyGained;

                activatedPopupImageDict.Add(customId, floatingItemPopupImage);

                floatingItemPopupImage.MoveLeft();
            }



        }

        // Update is called once per frame
        void Update()
        {

        }

        //0 : 장비 / 1 나무 2 고기 3 코인
        void OnItemPullyGained(int customID, int amount)
        {
            if (customgainItem.ContainsKey(customID))
            {
                customgainItem[customID] += amount;
            }

            else
            {
                customgainItem.Add(customID, amount);
            }

            activatedPopupImageDict.Remove(customID);

            int removedIndex = popupImageOrder.FindIndex(id => id == customID);

            popupImageOrder[removedIndex] = -1;
        }

        int FindEmptySlot()
        {
            for (int i = 0; i < popupImageOrder.Count; i++)
            {
                if (popupImageOrder[i] == -1)
                    return i;
            }
            return -1;
        }
    }

}
