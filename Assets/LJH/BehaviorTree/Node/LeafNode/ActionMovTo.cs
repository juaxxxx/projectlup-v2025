using UnityEngine;

namespace RL
{
    public class ActionMovTo : LeafNode
    {
        Transform myTransform;
        StageController stageController;
        public ActionMovTo(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
        {
            myTransform = behaviorTree.gameObject.GetComponent<Transform>();
        }


        public override NodeState Evaluate()
        {

            if (blackBoard.targetPos == null)
            {
                UnityEngine.Debug.LogError("TargetPos is Missing");
                return NodeState.Fail;
            }

            UnityEngine.Debug.Log("Action MoveTo");

            myTransform.position = Vector3.MoveTowards(myTransform.position, blackBoard.targetPos.position, blackBoard.Speed * Time.deltaTime);

            return NodeState.Success;
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

