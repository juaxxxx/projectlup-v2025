using UnityEngine;

namespace LUP.PCR
{
    public class EatFood : BTNode
    {
        Worker worker;
        float timer = 0f;
        float duration = 3f;

        public EatFood(Worker worker) { this.worker = worker; }

        public override WorkerNodeState Evaluate()
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"식사 중... {timer:F1}/{duration}");
                return WorkerNodeState.RUNNING;
            }

            worker.hunger = 0f;
            Debug.Log("식사 완료!");
            return WorkerNodeState.SUCCESS;
        }
    }

}
