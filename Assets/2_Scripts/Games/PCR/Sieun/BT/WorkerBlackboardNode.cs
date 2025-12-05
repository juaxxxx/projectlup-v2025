using UnityEngine;

namespace LUP.PCR
{
    // 1. 이 클래스가 생성될 때 Blackboard를 받음
    // 2. : base() 를 통해 부모(BTNode)의 생성자를 먼저 실행함
    public abstract class WorkerBlackboardNode : BTNode
    {
        protected readonly WorkerBlackboard BB;
        private WorkerAI ownerAI;
        private Worker workerComp;
        private UnitMover mover;
        protected WorkerBlackboardNode(WorkerBlackboard blackboard) : base()
        {
            this.BB = blackboard;
        }
        protected WorkerAI OwnerAI
        {
            get
            {
                if (ownerAI == null)
                    BB.TryGetValue(BBKeys.OwnerAI, out ownerAI);
                return ownerAI;
            }
        }
        protected Worker WorkerComp
        {
            get
            {
                if (workerComp == null)
                    BB.TryGetValue(BBKeys.Self, out workerComp);
                return workerComp;
            }
        }
        protected UnitMover Mover
        {
            get
            {
                if (mover == null)
                    BB.TryGetValue(BBKeys.UnitMover, out mover);
                return mover;
            }
        }

        // 자식 노드들이 편하게 쓰라고 만든 헬퍼 함수들
        // (굳이 Blackboard.GetValue 하지 않고 Node.GetData 로 짧게 쓰기 위함)
        protected T GetData<T>(string key) => BB.GetValue<T>(key);
        protected void SetData<T>(string key, T value) => BB.SetValue(key, value);
        protected bool HasData(string key) => BB.HasKey(key);
    }
}

