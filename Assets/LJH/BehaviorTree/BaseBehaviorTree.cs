using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RL
{
    public class BaseBehaviorTree : MonoBehaviour
    {
        protected Animator Animator;

        protected RootNode rootnode;
        protected EnemyBlackBoard enemyBlackBoard;
        protected CompositeNode topCompositeNode;

        private LeafNode currentRunningLeaf;

        private void Awake()
        {
            enemyBlackBoard = GetComponent<EnemyBlackBoard>();
            Animator = GetComponent<Animator>();
            enemyBlackBoard.HP = enemyBlackBoard.MaxHP;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if(Animator != null)
            {
                Animator.GetBehaviour<AnimatorCallBack>().SetAnimEndCallBack(OnAnimationEnd);
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

        public void PlayAnimation(string animName, LeafNode caller)
        {
            if (Animator == null)
                return;

            currentRunningLeaf = caller;
            Animator.Play(animName);
        }

        public void OnAnimationEnd(AnimatorStateInfo info)
        {
            if (currentRunningLeaf == null)
                return;

            currentRunningLeaf.OnAnimationEnd();
            currentRunningLeaf = null;
        }
    }
}



