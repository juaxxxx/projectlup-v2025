using UnityEngine;

namespace LUP.PCR
{
    public class EatFood : WorkerBlackboardNode
    {
        public EatFood(WorkerBlackboard bb) : base(bb) { }
        private float timer = 0f;
        private float duration = 1f;
        protected override void OnStart()
        {
            timer = 0f;

            if (WorkerComp != null)
            {
                WorkerComp.SetActionState(WorkerActionState.Eating);
            }
        }

        protected override NodeState OnUpdate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);

            if (timer < duration)
            {
                timer += Time.deltaTime;
                
                if (WorkerComp != null)
                {
                    WorkerComp.SetActionState(WorkerActionState.Eating);
                }

                return ReturnAndLog(NodeState.RUNNING, $"1-4. 식사 중... {timer:F1}/{duration}");
            }
            else
            {
                // 식사 완료 처리
                OwnerAI.Hunger = 0f;

                if (WorkerComp != null)
                {
                    WorkerComp.SetActionState(WorkerActionState.Idle);
                }

                return ReturnAndLog(NodeState.SUCCESS, "1-4. 식사 완료!");
            }
        }
    }
}