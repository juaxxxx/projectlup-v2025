using UnityEngine;

namespace RL
{
    public class ActionAttack : LeafNode
    {
        public ActionAttack(BlackBoard blackBoar, BaseBehaviorTree behaviorTree) : base( blackBoar, behaviorTree )
        {

        }
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Attack");
            return NodeState.Success;
        }

        public override void OnAnimationEnd()
        {

        }
    }
}

