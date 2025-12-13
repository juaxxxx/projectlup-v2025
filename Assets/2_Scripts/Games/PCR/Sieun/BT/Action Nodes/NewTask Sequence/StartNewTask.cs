using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : WorkerBlackboardNode
    {
        public StartNewTask(WorkerBlackboard bb) : base(bb) { }
        float timer = 0f;
        float duration = 3f;

        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.AssignedWorkplace);
            //OwnerAI.HasTask = GetData<bool>(BBKeys.HasTask);
            
            if (building == null)
            {
                return NodeState.FAILURE;
            }
            else if(timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"2-3. 할당받은 작업 진행중... {timer:F1}/{duration}");
                OwnerAI.HasTask = true;

                return NodeState.RUNNING;
            }
            else
            {
                timer = 0f;
                BB.Remove(BBKeys.AssignedWorkplace);
                OwnerAI.HasTask = false;
                
                Debug.Log("2-3. 작업 완료.");
                return NodeState.SUCCESS;
            }
        }
    }
}

