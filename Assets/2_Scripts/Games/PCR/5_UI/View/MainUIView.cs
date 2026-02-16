using UnityEngine;
using System;
using UnityEngine.UI;
using R3;

namespace LUP.PCR
{
    public class MainUIView : MonoBehaviour
    {
        [Header("작업 버튼")]
        [SerializeField] private Button digBtn;
        [SerializeField] private Button constructBtn;
        [SerializeField] private Button inventoryBtn;

        [Header("자원 표시")]
        [SerializeField] private Text foodText;
        [SerializeField] private Text powerText;
        [SerializeField] private Text stoneText;
        [SerializeField] private Text ironText;

        private readonly CompositeDisposable cd = new();
        private MainUIViewModel mainVM;

        // Event
        //public event Action OnClickDig;
        //public event Action OnClickConstruct;
        //public event Action OnClickInventory;

        public void Bind(MainUIViewModel vm)
        {
            cd.Clear();
            mainVM = vm;
            
            // 버튼 입력 → VM Command로 전달
            digBtn.onClick.AddListener(() => mainVM.ClickDig.OnNext(Unit.Default));
            constructBtn.onClick.AddListener(() => mainVM.ClickConstruct.OnNext(Unit.Default));
            inventoryBtn.onClick.AddListener(() => mainVM.ClickInventory.OnNext(Unit.Default));

            // ViewModel 값 구독 -> UI 반영
            vm.Food.DistinctUntilChanged().Subscribe(value => foodText.text = value.ToString()).AddTo(cd);
            vm.Power.DistinctUntilChanged().Subscribe(value => powerText.text = value.ToString()).AddTo(cd);
            vm.Stone.DistinctUntilChanged().Subscribe(value => stoneText.text = value.ToString()).AddTo(cd);
            vm.Iron.DistinctUntilChanged().Subscribe(value => ironText.text = value.ToString()).AddTo(cd);
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
