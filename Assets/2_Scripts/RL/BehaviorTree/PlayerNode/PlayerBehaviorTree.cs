using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LUP.RL
{
    public class PlayerBehaviorTree : MonoBehaviour
    {
        private Node root;
        [SerializeField] private PlayerBlackBoard bb;
        [SerializeField] private JoyStickSC joystick;


        [SerializeField] private Animator animator;
        protected AnimatorStateInfo stateInfo;
        protected Node currentRunningActionNode;

        //[SerializeField] private Archer playerArcher;
        //[SerializeField] private PlayerMove move;
        //[SerializeField] private PlayerArrowShooter move;
        private void Awake()
        {
            if (bb == null) bb = new PlayerBlackBoard();
            bb.Initialize(gameObject);
            if (joystick == null) joystick = FindFirstObjectByType<JoyStickSC>();


            BuildTree();
        }

        private void BuildTree()
        {
            // 조건 노드
            var isAlive = new IsAliveNode(bb);
            var isHitted = new IsHittedNode(bb);

            // 액션 노드
            var moveNode = new PlayerMoveNode(bb, joystick);
            var attackNode = new PlayerAttackNode(bb);

            // 행동 트리 구성

            var actionSelector = new SelectorNode(new List<Node> {  moveNode, attackNode });
            var mainSequence = new SequenceNode(new List<Node> { isAlive, isHitted, actionSelector });

            root = mainSequence;
        }

        private void Update()
        {
                root.Evaluate();
        }

        public void PlayAnimation(ActionState actionState, LeafNode caller)
        {
            if (animator == null)
                return;

            string calledAnimName = "None";

            switch (actionState)
            {
                case ActionState.MoveTo:
                    calledAnimName = "MoveTo";
                    break;

                case ActionState.Attack:
                    calledAnimName = "Attack";
                    break;

                case ActionState.Hitted:
                    calledAnimName = "Hitted";
                    break;

                case ActionState.Wait:
                    calledAnimName = "Wait";
                    break;

                case ActionState.Die:
                    calledAnimName = "Die";
                    break;

                default:
                    break;
            }

            stateInfo = GetCurrentAnimState();

            if (stateInfo.IsName("Wait") || stateInfo.IsName("MoveTo"))
            {
                currentRunningActionNode = caller;
                animator.Play(calledAnimName);
            }


        }

        public void OnAnimationEnd(AnimatorStateInfo info)
        {
            if (currentRunningActionNode == null)
                return;

            //currentRunningActionNode.OnAnimationEnd(info);
            currentRunningActionNode = null;
        }

        public AnimatorStateInfo GetCurrentAnimState()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo;
        }
    }
}
