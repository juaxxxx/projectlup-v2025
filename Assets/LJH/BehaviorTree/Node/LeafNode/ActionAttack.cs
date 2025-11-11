using UnityEngine;

namespace LUP.RL
{
    public class ActionAttack : LeafNode
    {
        private readonly EnemyBlackBoard bb;
        public ActionAttack(EnemyBlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base( blackBoard, behaviorTree )
        {
            bb = blackBoard;

        }
        public override NodeState Evaluate()
        {
            //UnityEngine.Debug.Log("Action Attack");
            //if(bb.EShooter == null)
            //{
            //    Debug.Log("½´ÅÍ °ª ¾øÀ½");
            //    return NodeState.Fail;
            //}
            //bb.EShooter.ShootArrow(bb.Target, bb.targetPos);
            return NodeState.Success;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

