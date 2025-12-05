using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LUP.RL
{

    public enum ActionState
    {
        MoveTo,
        Attack,
        Hitted,
        Idle,
        Die
    }

    public class BaseBehaviorTree : MonoBehaviour
    {
        protected Animator Animator;
        protected AnimatorStateInfo stateInfo;

        protected RootNode rootnode;
        protected EnemyBlackBoard enemyBlackBoard;
        protected CompositeNode topCompositeNode;

        private LeafNode currentRunningLeaf;

        public GameObject thisCharacter;

        private void Awake()
        {
            enemyBlackBoard = GetComponent<EnemyBlackBoard>();
            Animator = GetComponent<Animator>();

            enemyBlackBoard.HP = enemyBlackBoard.MaxHP;

            thisCharacter = gameObject;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (Animator != null)
            {
                AnimatorCallBack[] animatorCallBacks = Animator.GetBehaviours<AnimatorCallBack>();
                for (int i = 0; i < animatorCallBacks.Length; i++)
                {
                    animatorCallBacks[i].SetAnimEndCallBack(OnAnimationEnd);

                    if(animatorCallBacks[i].animTargetCallBackRate != 1.0f)
                    {
                        animatorCallBacks[i].SetAnimCallBBack(OnAnimationInTargetRate);
                    }
                }

                //StateMachineBehaviour를 애니메이션 상태(State) 단위로 “복제”해서 관리해.
                //즉, Attack, Hitted, Idle 각각이 전부 자기 전용의 AnimatorCallBack 인스턴스를 갖고 있는 거야.
                //하나만 찾아서 가져오면, 첫번째 놈 만 등록이 되어 진다.
                //Animator.GetBehaviour<AnimatorCallBack>().SetAnimEndCallBack(OnAnimationEnd);
            }

            InitBehaviorTree();
        }

        public virtual void InitBehaviorTree()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (enemyBlackBoard.Alive == false)
                return;

            if (rootnode != null)
            {
                rootnode.Evaluate();
            }

        }

        public void PlayAnimation(ActionState actionState, LeafNode caller)
        {
            if (Animator == null)
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
                    currentRunningLeaf = caller;
                    Animator.Play(calledAnimName, 0, 0.0f);
                    return;

                default:
                    break;
            }

            stateInfo = GetCurrentAnimState();

            if (stateInfo.IsName("Idle") || stateInfo.IsName("MoveTo"))
            {
                currentRunningLeaf = caller;
                Animator.Play(calledAnimName);
            }

            
        }

        public void OnAnimationEnd(AnimatorStateInfo info)
        {
            if (currentRunningLeaf == null)
                return;

            currentRunningLeaf.OnAnimationEnd(info);
            currentRunningLeaf = null;
        }

        public void OnAnimationInTargetRate(AnimatorStateInfo info)
        {
            if (currentRunningLeaf == null)
                return;

            currentRunningLeaf.OnAnimationInTargetRate();
        }

        public AnimatorStateInfo GetCurrentAnimState()
        {
            stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo;
        }

        public void ResetWorkingNodeIndex()
        {
            SelectorNode firstSelectorNode= (SelectorNode)rootnode.topChildNode;
            firstSelectorNode.ResetWorkingNodeIndex();

        }
    }
}



