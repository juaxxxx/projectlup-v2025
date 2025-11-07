using UnityEngine;

namespace LUP.RL
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
            if (!bb.isAlive) return NodeState.Fail;
            if (bb.Move.isMoving) return NodeState.Fail;

            if (Time.time - lastFireTime < bb.Shooter.fireDelay)  return NodeState.Fail;

            Debug.Log("น฿ป็");
                bb.Shooter.ShootArrow();
                lastFireTime = Time.time;
                return NodeState.Success;
        }
    }
  
}