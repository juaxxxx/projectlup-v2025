using UnityEngine;
namespace LUP.RL
{
    public class IsAliveNode : Node
    {
        private readonly PlayerBlackBoard bb;
   
        public IsAliveNode(PlayerBlackBoard blackboard)
        {
            bb = blackboard;
        }
        public override NodeState Evaluate()
        {
            if(bb.isAlive)
            {
              
                return NodeState.Success;
                
            }
            else
            {
           
                return NodeState.Fail;
            }
     
        }
    }

    public class IsHittedNode : Node
    {
        private readonly PlayerBlackBoard bb;
        private float hitDuration = 0.5f; 
        private float timer = 0f;
        private float slowRatio = 0.5f;    
        private bool speedReduced = false; 

        public IsHittedNode(PlayerBlackBoard blackboard)
        {
            bb = blackboard;
        }

        public override NodeState Evaluate()
        {
            // 피격 중일 때
            if (bb.OnHit)
            {
                Debug.Log("피격");
                // 처음 피격 시에만 감속 적용
                if (!speedReduced)
                {
                    bb.Move.speed *= slowRatio;
                    speedReduced = true;
                    timer = 0f;
                }

                timer += Time.deltaTime;

                // 일정 시간이 지나면 원래 속도로 복구
                if (timer >= hitDuration)
                {
                    bb.Move.speed = bb.Move.baseSpeed;
                    bb.OnHit = false;
                    speedReduced = false;
                    timer = 0f;
                    return NodeState.Success;
                }

                return NodeState.Running; // 감속 중
            }

            return NodeState.Success;
        }
    }

    public class IsMovingNode : Node
    {
        private readonly PlayerBlackBoard bb;
        public IsMovingNode(PlayerBlackBoard blackboard)
        {
            bb = blackboard;
        }
        public override NodeState Evaluate()
        {
            return bb.Move.isMoving ? NodeState.Success : NodeState.Fail;
        }
    }
}