using System;
using UnityEngine;

namespace LUP.PCR
{

    public abstract class BuildingBase : StructureBase
    {
        // @TODO: 데이터 처리 너무 헷갈린다. 꼭 정리해야 한다.
        // SO 관리.. 인스턴스로 생성해서 사용해야 독립적으로 관리 가능한가?
        // 아니면 그냥 고정 값을 가져오는 용도로만 사용하자.
        // ex) 기본 건설, 생산 시간, 생산 가능 자원 등
        protected ProductionRuntimeData runtimeData;

        protected BuildingInfo buildingInfo;
        protected ConstructionInfo constructionInfo;

        public BuildingStaticData buildingStaticData;
        public PCRConstructionStaticData currentConstructionData;
        public BuildingEvents buildingEvents;

        public string buildingName;
        public Vector2Int entrancePos; // 작업자 도달 위치
        public PCRResourceCenter resourceCenter;
        public GameObject ConstructScreen;

        protected IBuildState currBuildState;
        protected bool hasWork;

        public abstract void Init(ProductionRuntimeData runtimeData);// 건물 정보랑 상태 가져올 매개변수 확장 필요

        public abstract void CompleteContruction();

        public abstract void Upgrade();

        public abstract void DeliverToInventory();

        public void OpenBuildingUI()
        {
            Debug.Log("OpenUI");
        }

        public void CloseBuildingUI()
        {
            Debug.Log("CloseUI");
        }

        public void ChangeState(IBuildState state)
        {
            currBuildState?.Exit(this);
            currBuildState = state;
            currBuildState.Enter(this);
        }

        public BuildingInfo GetBuildingInfo()
        {
            return buildingInfo;
        }

        public void SetBuildingInfo(BuildingInfo buildingInfo)
        {
            this.buildingInfo = buildingInfo;
        }

        public ConstructionInfo GetConstructionInfo()
        {
            return constructionInfo;
        }

        public void SetConstructionInfo(ConstructionInfo constructionInfo)
        {
            this.constructionInfo = constructionInfo;
        }

        // TODO: 임시 입구 설정. 입구가 왼쪽도 있어야하고 오른쪽도 있어야하는 느낌..
        public void SetEntrance(Vector2Int pivotPos)
        {
            entrancePos = pivotPos;
        }

        public void EnterWorker()
        {
            hasWork = true;
        }

        public void ExitWorker()
        {
            hasWork = false;
        }
    }
}