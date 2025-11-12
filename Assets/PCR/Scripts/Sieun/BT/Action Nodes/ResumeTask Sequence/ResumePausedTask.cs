using UnityEngine;

namespace LUP.PCR
{
    public class ResumePausedTask : BTNode
    {
        Worker worker;
        float timer = 0f;
        float duration = 2f;


        public override WorkerNodeState Evaluate(WorkerAI worker)
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"작업 재개 중... {timer:F1}/{duration}");
                return WorkerNodeState.RUNNING;
            }

            worker.hasPausedWork = false;
            Debug.Log("작업 재개 완료!");
            timer = 0f;

            return WorkerNodeState.SUCCESS;
        }
    }
}

