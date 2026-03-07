using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class InventoryUIView : MonoBehaviour
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

        private readonly CompositeDisposable cd = new();
        private InventoryUIViewModel invenVM;

        public void Bind(InventoryUIViewModel vm)
        {
            cd.Clear();
            invenVM = vm;

            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(() => invenVM.ClickBack.OnNext(Unit.Default));

            // 값 바인딩
            vm.Stone.DistinctUntilChanged().Subscribe(v => stoneText.text = v.ToString()).AddTo(cd);
            vm.Coal.DistinctUntilChanged().Subscribe(v => coalText.text = v.ToString()).AddTo(cd);
            vm.Iron.DistinctUntilChanged().Subscribe(v => ironText.text = v.ToString()).AddTo(cd);
            vm.Wheat.DistinctUntilChanged().Subscribe(v => wheatText.text = v.ToString()).AddTo(cd);
            vm.Mushroom.DistinctUntilChanged().Subscribe(v => mushroomText.text = v.ToString()).AddTo(cd);
            vm.Meat.DistinctUntilChanged().Subscribe(v => meatText.text = v.ToString()).AddTo(cd);
            vm.Food.DistinctUntilChanged().Subscribe(v => foodText.text = v.ToString()).AddTo(cd);
            vm.Power.DistinctUntilChanged().Subscribe(v => powerText.text = v.ToString()).AddTo(cd);
            vm.Diamond.DistinctUntilChanged().Subscribe(v => diamondText.text = v.ToString()).AddTo(cd);
        }

        private void OnDestroy()
        {
            cd.Dispose();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

