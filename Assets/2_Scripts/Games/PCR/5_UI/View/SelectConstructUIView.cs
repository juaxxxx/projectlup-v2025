using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class SelectConstructUIView : MonoBehaviour
    {
        [SerializeField] private Button wheatFarmBtn;
        [SerializeField] private Button moleFarmBtn;
        [SerializeField] private Button powerStationBtn;
        [SerializeField] private Button stoneMineBtn;
        [SerializeField] private Button workStationBtn;
        [SerializeField] private Button restaurantBtn;
        [SerializeField] private Button ladderBtn;

        [SerializeField] private Button backBtn;

        private readonly CompositeDisposable cd = new();


        //public event Action OnClickSelectedBuilding;
        //public event Action<BuildingType> OnBuildingTypeChanged;
        //public event Action OnClickBack;

        public void Bind(SelectConstructViewModel vm)
        {
            cd.Clear();

            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(() => vm.ClickBack.OnNext(Unit.Default));

            wheatFarmBtn.onClick.RemoveAllListeners();
            wheatFarmBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.WHEATFARM));

            moleFarmBtn.onClick.RemoveAllListeners();
            moleFarmBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.MOLEFARM));

            powerStationBtn.onClick.RemoveAllListeners();
            powerStationBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.POWERSTATION));

            stoneMineBtn.onClick.RemoveAllListeners();
            stoneMineBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.STONEMINE));

            workStationBtn.onClick.RemoveAllListeners();
            workStationBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.WORKSTATION));

            restaurantBtn.onClick.RemoveAllListeners();
            restaurantBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.RESTAURANT));

            ladderBtn.onClick.RemoveAllListeners();
            ladderBtn.onClick.AddListener(() => vm.SelectBuildType.OnNext(BuildingType.LADDER));

        }

        //private void Awake()
        //{
        //    wheatFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.WHEATFARM));
        //    moleFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.MOLEFARM));
        //    powerStationBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.POWERSTATION));
        //    stoneMineBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.STONEMINE));
        //    workStationBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.WORKSTATION));
        //    restaurantBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.RESTAURANT));
        //    ladderBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.LADDER));

        //    backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());
        //    wheatFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    moleFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    powerStationBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    stoneMineBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    workStationBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    restaurantBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //    ladderBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        //}

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
