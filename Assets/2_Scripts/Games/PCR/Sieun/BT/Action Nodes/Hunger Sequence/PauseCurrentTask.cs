using UnityEngine;

namespace LUP.PCR
{
    public class PauseCurrentTask : WorkerBlackboardNode
    {
        public PauseCurrentTask(WorkerBlackboard bb) : base(bb) { }

        protected override NodeState OnUpdate()
        {
            bool isWorking = GetData<bool>(BBKeys.IsWorking);
            
            if (isWorking) 
            {
                Debug.Log("1-2. 일하던 중이었음...");

                ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
                
                if (building == null)
                {
                    Debug.Log("1-2. 할당된 건물 없음!");
                    return NodeState.FAILURE; // nothing to pause
                }
                else
                {
                    Debug.Log($"1-2. {building.buildingName}의 작업을 취소했습니다.");
                    building.StopProduction();

                    BB.Remove(BBKeys.TargetBuilding);
                    SetData(BBKeys.IsWorking, false);
                }
            }

            return NodeState.SUCCESS;
        }
    }
}

