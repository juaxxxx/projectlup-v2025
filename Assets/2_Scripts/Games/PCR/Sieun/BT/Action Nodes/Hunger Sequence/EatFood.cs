using UnityEngine;

namespace LUP.PCR
{
    public class EatFood : WorkerBlackboardNode
    {
        public EatFood(WorkerBlackboard blackboard) : base(blackboard) { }
        float timer = 0f;
        float duration = 1f;
        int logLoopCount = 0;
        protected override NodeState OnUpdate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);
            
            if (timer < duration)
            {
                timer += Time.deltaTime;
                Debug.Log($"1-4. 식사 중... {timer:F1}/{duration}");
                return NodeState.RUNNING;
            }
            else
            {
                //if(logLoopCount == 0)
                //{
                //}

               currentHunger = 0f;
               timer = 0f;
               OwnerAI.Hunger = currentHunger;
               SetData<float>(BBKeys.Hunger, currentHunger);

               Debug.Log("1-4. 식사 완료!");
               return NodeState.SUCCESS;

            }
        }
    }

}
