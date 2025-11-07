using UnityEngine;

namespace PCR
{
    public class StartNewTask : BTNode
    {
        Worker worker;
        float timer = 0f;
        float duration = 3f;

        public StartNewTask(Worker worker) { this.worker = worker; }

        public override WorkerNodeState Evaluate()
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"새 작업 수행 중... {timer:F1}/{duration}");
                return WorkerNodeState.RUNNING;
            }

            worker.hasNewTask = false;
            Debug.Log("새 작업 완료!");
            return WorkerNodeState.SUCCESS;
        }
    }
}

