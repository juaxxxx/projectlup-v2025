using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 3f;

        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            
            if (building == null)
            {
                return NodeState.FAILURE;
            }
            else if(timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"2-3. 할당받은 작업 진행중... {timer:F1}/{duration}");
                SetData(BBKeys.IsWorking, true);

                return NodeState.RUNNING;
            }
            else
            {
                timer = 0f;
                SetData(BBKeys.HasNewTask, false);
                SetData(BBKeys.IsWorking, true);
                BB.Remove(BBKeys.TargetBuilding);
                Debug.Log("2-3. 작업 완료.");
                return NodeState.SUCCESS;
            }
        }
    }
}

