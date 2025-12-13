//using UnityEditor.Experimental.GraphView;
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
            if(!behaviorTree.GetCurrentAnimState().IsName("Die"))
            {
                blackBoard.Alive = false;
             
                behaviorTree.PlayAnimation(ActionState.Die, this);
                SetNavAgentDeActivate(true);
            }
            

            return NodeState.Success;
        }

        public override void OnAnimationInTargetRate()
        {
            if (behaviorTree.currentRunningLeaf != this)
                return;

            MonoBehaviour.Destroy(behaviorTree.thisCharacter);
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {
            if (behaviorTree.currentRunningLeaf != this)
                return;

            MonoBehaviour.Destroy(behaviorTree.thisCharacter);
        }
    }
}

