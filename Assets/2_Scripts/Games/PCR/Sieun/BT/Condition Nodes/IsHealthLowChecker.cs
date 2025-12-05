using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace LUP.PCR
{
    public class IsHealthLowChecker : WorkerBlackboardNode
    {
        public IsHealthLowChecker(WorkerBlackboard blackboard) : base(blackboard) { }

        int logLoopCount = 0;

       protected override NodeState OnUpdate()
        {
            float currentHunger = GetData<float>(BBKeys.Hunger);
            bool isHungry = currentHunger >= HungerRules.HungryThreshold;
            SetData(BBKeys.IsHunger, isHungry);
            
            if (isHungry)
            {
                Debug.Log("1-1. 배고픔 감지됨.");
                return NodeState.SUCCESS;
            }
            else
            {
                if (logLoopCount == 0)
                {
                    Debug.Log("1-1. 아직 배고프지 않음.");
                    logLoopCount += 1;
                }
                return NodeState.FAILURE;
            }

        }
    }


}

