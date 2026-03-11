using DG.Tweening;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using TMPro;

namespace LUP.ES
{
    public class ResultDisplayCenter : MonoBehaviour
    {
        private EventBroker eventBroker;
        private ItemCenter itemCenter; //테스트 용
        public GameObject resultPanel;
        public GameObject ItemDisplayContent;
        public GameObject itemSlotPrefab;
        public TextMeshProUGUI resultHeader;    
        //public Button lobbyButton;

        private Transform contentParent;
        private List<Item> items;
        private void Start()
        {
            resultPanel.SetActive(false);
            //lobbyButton.onClick.AddListener(LoadLobby);
            eventBroker = FindAnyObjectByType<EventBroker>();
            itemCenter = FindAnyObjectByType<ItemCenter>();
            contentParent = ItemDisplayContent.transform;
            if (eventBroker != null )
            {
                eventBroker.OnGameFinished += ShowResult;
            }
        }

        private void OnDestroy()
        {
            if (eventBroker != null)
            {
                eventBroker.OnGameFinished -= ShowResult;
            }
        }

        public void ShowInventoryItems(List<Item> items)
        {
            List<GameObject> createdSlots = new List<GameObject>();

            for (int i = 0; i < items.Count; i++)
            {
                GameObject newSlot = Instantiate(itemSlotPrefab, contentParent);

                ItemDisplaySlot slot = newSlot.GetComponent<ItemDisplaySlot>();

                if (slot != null)
                {
                    slot.SetItem(items[i], items);
                }

                CanvasGroup canvasGroup = newSlot.GetComponent<CanvasGroup>();
                if (canvasGroup == null) canvasGroup = newSlot.AddComponent<CanvasGroup>();

                canvasGroup.alpha = 0f;

                createdSlots.Add(newSlot);

               
            }

            Canvas.ForceUpdateCanvases();

            LayoutGroup layoutGroup = contentParent.GetComponent<LayoutGroup>();
            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
            }

            for (int i = 0; i < createdSlots.Count; i++)
            {
                GameObject slotObj = createdSlots[i];
                RectTransform rect = slotObj.GetComponent<RectTransform>();
                CanvasGroup canvasGroup = slotObj.GetComponent<CanvasGroup>();

                Vector2 originalPos = rect.anchoredPosition;
                rect.anchoredPosition = new Vector2(originalPos.x - 200f, originalPos.y);

                float delay = i * 0.1f;

                var moveTween = rect.DOAnchorPos(originalPos, 0.8f)
                    .SetDelay(delay)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);

                if (i == createdSlots.Count - 1)
                {
                    moveTween.OnComplete(() =>
                    {
                        if (layoutGroup != null) layoutGroup.enabled = true;
                    });
                }

                canvasGroup.DOFade(1f, 0.8f)
                    .SetDelay(delay)
                    .SetEase(Ease.Linear)
                    .SetUpdate(true);
            }
        }

        private void ShowResult(bool isSuccess)
        {
            Debug.Log("GameFinish");
            Time.timeScale = 0f;
            StringBuilder resultHeadrString = new StringBuilder();
            resultHeadrString.Append("Extraction ");
            resultPanel.SetActive(true);
            if (isSuccess)
            {
                SoundManager.Instance.PlaySFX("SuccessfulEscape");
                resultHeadrString.Append("Complete");
                resultHeader.color = Color.white;
                Inventory inventory = FindAnyObjectByType<Inventory>();
                if (inventory != null)
                {
                    items = inventory.GetItems();
                    ShowInventoryItems(items);
                    ExtractionShooterStage extractionShooterStage = StageManager.Instance.GetCurrentStage() as ExtractionShooterStage;

                    foreach (Item item in items)
                    {
                        if (item == null)
                            continue;
                        if (item.ItemID == 1 || item.ItemID == 4 || item.ItemID == 7)
                            continue;
                        extractionShooterStage.ESInven.AddItem(item);
                    }
                }
                
            }
            else
            {
                SoundManager.Instance.PlaySFX("Escape failed");
                resultHeadrString.Append("Failed");
                resultHeader.color = Color.red;
            }
            resultHeader.text = resultHeadrString.ToString();
        }
    }
}
