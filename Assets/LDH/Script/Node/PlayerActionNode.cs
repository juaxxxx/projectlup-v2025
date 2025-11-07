using UnityEngine;

namespace RL
{
    public class PlayerAttackNode : Node
    {
        private readonly PlayerBlackBoard bb;
        private float lastFireTime = 0f;

        public PlayerAttackNode(PlayerBlackBoard blackboard)
        {
            bb = blackboard;
        }

        public override NodeState Evaluate()
        {
            // 조건: 생존 + 피격 X + 이동 X
            if (!bb.isAlive || bb.OnHit || bb.Move.isMoving)
                return NodeState.Fail;

            // 쿨타임 검사
            if (Time.time - lastFireTime < bb.Shooter.fireDelay)
                return NodeState.Fail;

            // 실제 공격 실행
            Debug.Log("Node에서 실행");
            bb.Shooter.ShootArrow();
            lastFireTime = Time.time;

            return NodeState.Success;
        }
    }
  
}