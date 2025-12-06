using UnityEngine;

namespace LUP.PCR
{
    public class PauseCurrentTask : WorkerBlackboardNode
    {
        public PauseCurrentTask(WorkerBlackboard bb) : base(bb) { }

        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.AssignedWorkplace);
            
            if (building != null) 
            {
                //building.StopProduction(); // @TODO : IsEnter() false 로 대체.
                Debug.Log($"1-2. 배고픔으로 인해 {building.buildingName} 작업을 취소했습니다.");
            }
            
            SetData(BBKeys.IsWorking, false);

            return NodeState.SUCCESS;
        }
    }
}

