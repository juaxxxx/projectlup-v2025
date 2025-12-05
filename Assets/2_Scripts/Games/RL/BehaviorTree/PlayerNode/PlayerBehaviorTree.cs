using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LUP.RL
{
    public class PlayerBehaviorTree : MonoBehaviour
    {
        private Node root;
        private PlayerBlackBoard bb;
        [SerializeField] private JoyStickSC joystick;

        //[SerializeField] private Archer playerArcher;
        //[SerializeField] private PlayerMove move;
        //[SerializeField] private PlayerArrowShooter move;

        [SerializeField] private Animator animator;
        protected AnimatorStateInfo stateInfo;
        protected PlayerLeafNode currentRunningLeafNode;

        private void Awake()
        {
            bb = GetComponent<PlayerBlackBoard>();
            if(!bb)
            {
                Debug.LogError("PlayerBlackBoard가 Player에 붙어 있지 않습니다!");
                return;
            }

            bb.Initialize(gameObject);
            if (joystick == null) joystick = FindFirstObjectByType<JoyStickSC>();


            BuildTree();
        }

        private void Start()
        {
            if (animator != null)
            {
                AnimatorCallBack[] animatorCallBacks = animator.GetBehaviours<AnimatorCallBack>();
                for (int i = 0; i < animatorCallBacks.Length; i++)
                {
                    animatorCallBacks[i].SetAnimEndCallBack(OnAnimationEnd);

                    if (animatorCallBacks[i].animTargetCallBackRate != 1.0f)
                    {
                        animatorCallBacks[i].SetAnimCallBBack(OnAnimationInTargetRate);
                    }
                }
            }
        }

        private void BuildTree()
        {
            // 조건 노드
            var isAlive = new IsAliveNode(bb);
            var isHitted = new IsHittedNode(bb);

            // 액션 노드
            var moveNode = new PlayerMoveNode(bb, joystick, this);
            var attackNode = new PlayerAttackNode(bb, this);

            // 행동 트리 구성

            var actionSelector = new SelectorNode(new List<Node> {  moveNode, attackNode });
            var mainSequence = new SequenceNode(new List<Node> { isAlive, isHitted, actionSelector });

            root = mainSequence;
        }

        private void Update()
        {
                root.Evaluate();
        }

        public void PlayAnimation(ActionState actionState, PlayerLeafNode caller, float palySpeed = 1.0f)
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

                case ActionState.Idle:
                    calledAnimName = "Idle";
                    break;

                case ActionState.Die:
                    calledAnimName = "Die";
                    break;

                default:
                    break;
            }

            stateInfo = GetCurrentAnimState();

            bool Name = stateInfo.IsName("Idle");
            bool Name1 = stateInfo.IsName("MoveTo");
            bool Name2 = stateInfo.IsName("Attack");


            if (stateInfo.IsName("Idle") || stateInfo.IsName("MoveTo") || stateInfo.IsName("Attack"))
            {
                currentRunningLeafNode = caller;
                animator.speed = palySpeed;
                animator.Play(calledAnimName);
            }


        }

        public void OnAnimationInTargetRate(AnimatorStateInfo info)
        {
            if (currentRunningLeafNode == null)
                return;

            currentRunningLeafNode.OnAnimationInTargetRate();
        }

        public void OnAnimationEnd(AnimatorStateInfo info)
        {
            if (currentRunningLeafNode == null)
                return;

            currentRunningLeafNode.OnPlayerAnimationEnd(info);
            //currentRunningLeafNode = null;

        }

        public AnimatorStateInfo GetCurrentAnimState()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo;
        }
    }
}
