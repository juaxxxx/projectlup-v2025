using UnityEngine;

namespace LUP.PCR
{
    public class EatFood : WorkerBlackboardNode
    {
        public EatFood(WorkerBlackboard bb) : base(bb) { }
        float timer = 0f;
        float duration = 1f;
        protected override NodeState OnUpdate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);
            
            if (timer < duration)
            {
                timer += Time.deltaTime;
                return ReturnAndLog(NodeState.RUNNING, $"1-4. 식사 중... {timer:F1}/{duration}");
            }
            else
            {
               currentHunger = 0f;
               timer = 0f;
               OwnerAI.Hunger = currentHunger;
               SetData<float>(BBKeys.Hunger, currentHunger);

               return ReturnAndLog(NodeState.SUCCESS, $"1-4. 식사 중... {timer:F1}/{duration}");
            }
        }
    }

}
