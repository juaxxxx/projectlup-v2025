using UnityEngine;
using UnityEngine.AI;

namespace LUP.RL
{
    public class ActionMoveTo : LeafNode
    {
        Transform myTransform;
        StageController stageController;
        public ActionMoveTo(BlackBoard blackBoard, BaseBehaviorTree behaviorTree) : base(blackBoard, behaviorTree)
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

            //UnityEngine.Debug.Log("Action MoveTo");

            if (behaviorTree.GetCurrentAnimState().IsName("MoveTo") == true)
            {
                //myTransform.position = Vector3.MoveTowards(myTransform.position, blackBoard.targetPos.position, blackBoard.Speed * Time.deltaTime);

                if(blackBoard.agent)
                {

                    SetNavAgentDeActivate(false);

                    NavMeshAgent AIagent = blackBoard.agent;

                    AIagent.speed = blackBoard.Speed;
                    AIagent.SetDestination(blackBoard.targetPos.position);
                }


                return NodeState.Success;
            }

            else
            {
                behaviorTree.PlayAnimation(ActionState.MoveTo, this);

                return NodeState.Running;
            }
        }

        public override void OnAnimationEnd(AnimatorStateInfo animInfo)
        {

        }
    }
}

