using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LUP.RL
{
    public class ActionDie : LeafNode
    {
        public ActionDie(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base( blackBoard, behaviorTree )
        {
            
        }
        public override NodeState Evaluate()
        {
            blackBoard.Alive = false;
            return NodeState.Success;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

