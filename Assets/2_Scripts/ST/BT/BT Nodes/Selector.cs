using System.Collections.Generic;
namespace LUP.ST
{

    public class Selector : BaseNode
    {
        public Selector(List<BaseNode> children) : base(children) { }

        public override NodeState Evaluate()
        {
            // 이전에 RUNNING 상태였던 자식 노드부터 평가를 시작합니다.
            int startIndex = (runningChild != null) ? children.IndexOf(runningChild) : 0;
            if (startIndex == -1) startIndex = 0; // 안전 장치

            for (int i = startIndex; i < children.Count; i++)
            {
                BaseNode child = children[i];
                NodeState result = child.Evaluate();

                if (result == NodeState.SUCCESS)
                {
                    // 성공 시 RUNNING 상태 초기화
                    runningChild = null;
                    state = NodeState.SUCCESS;
                    return state;
                }
                if (result == NodeState.RUNNING)
                {
                    // RUNNING 반환 시 해당 자식을 저장하고 반환
                    runningChild = child;
                    state = NodeState.RUNNING;
                    return state;
                }
            }

            // 모든 자식이 실패하면 RUNNING 상태 초기화
            runningChild = null;
            state = NodeState.FAILURE;
            return state;
        }

        // 초기화 시 자식 노드에게도 전파
        public override void Reset()
        {
            base.Reset();
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }

}