using UnityEngine;

namespace LUP.PCR
{
    // 1. 이 클래스가 생성될 때 Blackboard를 받음
    // 2. : base() 를 통해 부모(BTNode)의 생성자를 먼저 실행함
    public abstract class WorkerBlackboardNode : BTNode
    {
        protected readonly WorkerBlackboard BB;
        protected WorkerAI OwnerAI { get; private set; }
        protected Worker WorkerComp { get; private set; }
        protected IUnitMoveable Mover { get; private set; }
        protected WorkerBlackboardNode(WorkerBlackboard blackboard) : base() // 기존 BTNode 생성자 호출
        {
            this.BB = blackboard;

            // 지연 초기화: 생성 시점에 아직 참조할 컴포넌트가 없을 수 있으므로 TryGet로 안전하게 캐싱
            if (BB.TryGetValue<WorkerAI>(BBKeys.OwnerAI, out var ai)) OwnerAI = ai;
            if (BB.TryGetValue<Worker>(BBKeys.Self, out var w)) WorkerComp = w;
            if (BB.TryGetValue<IUnitMoveable>(BBKeys.UnitMover, out var m)) Mover = m;
        }

        // 만약 런타임 도중 OwnerAI/Worker/Mover가 등록될 수 있으니 필요 시 재로딩 가능
        protected void RefreshCachedReferences()
        {
            if (OwnerAI == null && BB.TryGetValue<WorkerAI>(BBKeys.OwnerAI, out var ai)) OwnerAI = ai;
            if (WorkerComp == null && BB.TryGetValue<Worker>(BBKeys.Self, out var w)) WorkerComp = w;
            if (Mover == null && BB.TryGetValue<IUnitMoveable>(BBKeys.UnitMover, out var m)) Mover = m;
        }

        // 자식 노드들이 편하게 쓰라고 만든 헬퍼 함수들
        // (굳이 Blackboard.GetValue 하지 않고 Node.GetData 로 짧게 쓰기 위함)
        protected T GetData<T>(string key) => BB.GetValue<T>(key);
        protected void SetData<T>(string key, T value) => BB.SetValue(key, value);
        protected bool HasData(string key) => BB.HasKey(key);
    }
}

