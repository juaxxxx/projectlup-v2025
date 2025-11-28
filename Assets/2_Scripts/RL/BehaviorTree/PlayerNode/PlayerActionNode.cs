using UnityEngine;

namespace LUP.RL
{
    public class PlayerAttackNode : PlayerLeafNode
    {
        private readonly PlayerBlackBoard bb;
        private readonly PlayerBehaviorTree bt;
        private float lastFireTime = 0f;

        public PlayerAttackNode(PlayerBlackBoard blackboard, PlayerBehaviorTree behaviorTree)
        {
            bb = blackboard;
            bt = behaviorTree;
        }

        public override NodeState Evaluate()
        {
            if (!bb.isAlive) return NodeState.Fail;
            if (bb.Move.isMoving) return NodeState.Fail;

            if (Time.time - lastFireTime < bb.Shooter.fireDelay) return NodeState.Fail;

            bt.PlayAnimation(ActionState.Attack, this);

            Debug.Log("น฿ป็");
            bb.Shooter.ShootArrow();
            lastFireTime = Time.time;
            return NodeState.Success;
        }

        public override void OnPlayerAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }

}