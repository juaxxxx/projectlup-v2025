using UnityEngine;

namespace LUP.PCR
{
    public abstract class BTNode
    {
        public BTNode()
        {
            // 추상클래스 생성자 = 자식 클래스가 초기화될 때 공통적으로 실행해야 할 로직
            // 예: 노드 생성 시 고유 ID 부여나 디버깅용 초기화가 필요하면 여기에 작성
        }

        public enum NodeState
        {
            RUNNING,
            SUCCESS,
            FAILURE,
        }
        public abstract NodeState Evaluate();
    }
}