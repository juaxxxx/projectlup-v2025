using LUP.ES;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace LUP.RL
{
    public class PlayerAttackNode : PlayerLeafNode
    {
        private readonly PlayerBlackBoard bb;
        private readonly PlayerBehaviorTree bt;
        private Enemy target;
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

            if (Time.time - lastFireTime < bb.Shooter.fireSystem.fireDelay) return NodeState.Fail;
            Enemy findtarget = bb.FindClosestEnemy();
            target = findtarget;
            if (findtarget == null || findtarget.Equals(null))
            {
                Debug.Log("actio Node call NULL Target");
                return NodeState.Fail;
            }

            //bb.Shooter.TryShoot(findtarget.transform, bb.Health.Adata.currentData.Attack);
            //lastFireTime = Time.time;

            if (bt.GetCurrentAnimState().IsName("Attack") == false)
            {
                //bb.Shooter.TurnToTarget();
                bt.PlayAnimation(ActionState.Attack, this);
            }
            return NodeState.Success;
        }

        public override void OnPlayerAnimationEnd(AnimatorStateInfo animInfo)
        {

        }

        public override void OnAnimationInTargetRate()
        {
            Debug.Log("น฿ป็");
            bb.Shooter.TryShoot(target.transform, bb.Health.Adata.currentData.Attack);
            lastFireTime = Time.time;
        }
    }

}