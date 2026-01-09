using UnityEngine;
using System;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class MainUIView : MonoBehaviour, IMainUIView
    {
        [Header("작업 버튼")]
        [SerializeField]
        private Button digBtn;
        [SerializeField]
        private Button constructBtn;
        [SerializeField]
        private Button inventoryBtn;

        [Header("자원 표시")]
        [SerializeField]
        private Text foodText;
        [SerializeField]
        private Text powerText;
        [SerializeField]
        private Text stoneText;
        [SerializeField]
        private Text ironText;

        // Event
        public event Action OnClickDig;
        public event Action OnClickConstruct;
        public event Action OnClickInventory;

        private void Awake()
        {
            digBtn?.onClick.AddListener(() => OnClickDig?.Invoke());
            constructBtn?.onClick.AddListener(() => OnClickConstruct?.Invoke());
            inventoryBtn?.onClick.AddListener(() => OnClickInventory?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateResourceText(PCRResourceCenter resourceCenter)
        {
            int food = resourceCenter.GetResourceAmount(ResourceType.Food);
            int power = resourceCenter.GetResourceAmount(ResourceType.Power);
            int stone = resourceCenter.GetResourceAmount(ResourceType.Stone);
            int iron = resourceCenter.GetResourceAmount(ResourceType.Iron);

            foodText.text = food.ToString();
            powerText.text = power.ToString();
            stoneText.text = stone.ToString();
            ironText.text = iron.ToString();
        }
    }
}
