using UnityEngine;

namespace LUP.PCR
{
    public class PauseCurrentTask : WorkerBlackboardNode
    {
        public PauseCurrentTask(WorkerBlackboard bb) : base(bb) { }

        protected override NodeState OnUpdate()
        {
            bool isWorking = GetData<bool>(BBKeys.IsWorking);
            bool hasNewTask = GetData<bool>(BBKeys.HasNewTask);
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);

            if (isWorking) 
            {
                Debug.Log("1-2. 일하던 중이었음...");
                
                if (building == null)
                {
                    Debug.Log("1-2. 할당된 건물 없음!");
                    return NodeState.FAILURE; // nothing to pause
                }
                else
                {
                    Debug.Log($"1-2. 작업 중이던 {building.buildingName}의 작업을 취소했습니다.");
                    building.StopProduction();

                    BB.Remove(BBKeys.TargetBuilding);
                    SetData(BBKeys.IsWorking, false);

                    return NodeState.SUCCESS;
                }
            }
            else if(hasNewTask)
            {
                Debug.Log($"1-2. 작업 예정인 {building.buildingName}의 작업을 취소했습니다.");
                return NodeState.SUCCESS;
            }

            Debug.Log("1-2. 예약되거나 진행중인 작업이 없습니다.");
            return NodeState.FAILURE;
        }
    }
}

