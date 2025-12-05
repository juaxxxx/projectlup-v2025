using UnityEngine;

namespace LUP.PCR
{ 
    public class IsNewTaskChecker : WorkerBlackboardNode
    {
        public IsNewTaskChecker(WorkerBlackboard blackboard) : base(blackboard) { }
        protected override NodeState OnUpdate()
        {
            ProductableBuilding building = GetData<ProductableBuilding>(BBKeys.TargetBuilding);
            bool hasNewTask = GetData<bool>(BBKeys.HasNewTask);

            return building != null && hasNewTask ?
            LogAndReturn(NodeState.SUCCESS, "2-1. 새 작업 발생!")
            : LogAndReturn(NodeState.FAILURE, "2-1. 할당된 작업이 없습니다.");
        }

        T LogAndReturn<T>(T value, string message)
        {
            Debug.Log(message + $" (값: {value})");
            return value;
        }
    }
}