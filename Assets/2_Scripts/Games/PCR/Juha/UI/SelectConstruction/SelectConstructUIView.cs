using System;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.PCR
{
    public class SelectConstructUIView : MonoBehaviour, ISelectConstructUIView
    {
        // 지금은 건물 몇개만 테스트
        [SerializeField]
        private Button wheatFarmBtn;
        [SerializeField]
        private Button moleFarmBtn;
        [SerializeField]
        private Button powerStationBtn;
        [SerializeField]
        private Button stoneMineBtn;
        [SerializeField]
        private Button workStationBtn;
        [SerializeField]
        private Button restaurantBtn;

        // Back Button
        [SerializeField]
        private Button backBtn;

        public event Action OnClickSelectedBuilding;
        public event Action<BuildingType> OnBuildingTypeChanged;
        public event Action OnClickBack;

        private void Awake()
        {
            wheatFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.WHEATFARM));
            moleFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.MOLEFARM));
            powerStationBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.POWERSTATION));
            stoneMineBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.STONEMINE));
            workStationBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.WORKSTATION));
            restaurantBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.RESTAURANT));

            backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());
            wheatFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
            moleFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
            powerStationBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
            stoneMineBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
            workStationBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
            restaurantBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());



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
