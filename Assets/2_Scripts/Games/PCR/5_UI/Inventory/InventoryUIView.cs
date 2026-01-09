using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class InventoryUIView : MonoBehaviour, IInventoryUIView
    {
        [Header("버튼")]
        [SerializeField] private Button backBtn;

        [Header("자원 개수 텍스트")]
        [SerializeField] private Text stoneText;
        [SerializeField] private Text coalText;
        [SerializeField] private Text ironText;
        [SerializeField] private Text wheatText;
        [SerializeField] private Text meatText;
        [SerializeField] private Text mushroomText;
        [SerializeField] private Text foodText;
        [SerializeField] private Text powerText;
        [SerializeField] private Text diamondText;

        public event Action OnClickBack;

        private void Awake()
        {
            backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());

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
            int stone = resourceCenter.GetResourceAmount(ResourceType.Stone);
            int coal = resourceCenter.GetResourceAmount(ResourceType.Coal);
            int iron = resourceCenter.GetResourceAmount(ResourceType.Iron);
            int wheat = resourceCenter.GetResourceAmount(ResourceType.Wheat);
            int mushroom = resourceCenter.GetResourceAmount(ResourceType.Mushroom);
            int food = resourceCenter.GetResourceAmount(ResourceType.Food);
            int power = resourceCenter.GetResourceAmount(ResourceType.Power);
            int diamond = resourceCenter.GetResourceAmount(ResourceType.Diamond);

            stoneText.text = stone.ToString();
            coalText.text = coal.ToString();
            ironText.text = iron.ToString();
            wheatText.text = wheat.ToString();
            mushroomText.text = mushroom.ToString();
            foodText.text = food.ToString();
            powerText.text = power.ToString();
            diamondText.text = diamond.ToString();
        }
    }
}

