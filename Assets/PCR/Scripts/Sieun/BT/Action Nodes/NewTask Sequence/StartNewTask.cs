using UnityEngine;

namespace LUP.PCR
{
    public class StartNewTask : BTNode
    {
        Worker worker;
        float timer = 0f;
        float duration = 3f;


        public override WorkerNodeState Evaluate(WorkerAI worker)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"새 작업 수행 중... {timer:F1}/{duration}");

                return WorkerNodeState.RUNNING;
            }
            
            worker.hasNewTask = false;
            Debug.Log("새 작업 완료!");
            timer = 0f;

            return WorkerNodeState.SUCCESS;
        }
    }
}

