using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RL
{
    public class EnemyBehaviorTree : BaseBehaviorTree
    {
        //private Animator Animator;

        //private RootNode rootnode;
        //private EnemyBlackBoard enemyBlackBoard;
        //private CompositeNode topCompositeNode;

        //private LeafNode currentRunningLeaf;

        //private void Awake()
        //{
        //    enemyBlackBoard = GetComponent<EnemyBlackBoard>();
        //    Animator = GetComponent<Animator>();
        //    enemyBlackBoard.HP = enemyBlackBoard.MaxHP;
        //}
        //// Start is called once before the first execution of Update after the MonoBehaviour is created
        //void Start()
        //{
        //    if(Animator != null)
        //    {
        //        Animator.GetBehaviour<AnimatorCallBack>().SetAnimEndCallBack(OnAnimationEnd);
        //    }

        //    InitBehaviorTree();
        //}

        public override void InitBehaviorTree()
        {
            List<Node> totalChildNodes = new List<Node>();

            //Die Action
            {
                ActionDie actionDie = new ActionDie(enemyBlackBoard, this);
                BlackboardConditionNode isAlive = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.ISALIVE, false, actionDie);

                totalChildNodes.Add(isAlive);
            }

            List<Node> midleNodes = new List<Node>();

            //Hitted Action
            {
                List<Node> childList = new List<Node>();

                ReduceHP reduceHP = new ReduceHP(enemyBlackBoard, this);

                ActionHitted actionHitted = new ActionHitted(enemyBlackBoard, this);

                //BlackboardConditionNode isAttackState = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INATKSTATE, false, actionHitted);
                //BlackboardConditionNode isRampagesTATE = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INRAMPAGE, false, actionHitted);

                List<(ConditionCheckEnum, bool)> whishConditions = new()
                {
                    (ConditionCheckEnum.INATKSTATE, false),
                    (ConditionCheckEnum.INRAMPAGE, false)
                };

                BlackboardMultiConditionNode inAttack_InRampage = new BlackboardMultiConditionNode(enemyBlackBoard, whishConditions, actionHitted);



                childList.Add(reduceHP);
                childList.Add(inAttack_InRampage);


                SequenceNode atkSequenceNode = new SequenceNode(childList);
                BlackboardConditionNode isHitted = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.OnHit, true, atkSequenceNode);


                midleNodes.Add(isHitted);
            }

            //Attack Action
            {
                List<Node> childList = new List<Node>();

                Wait wait = new Wait(enemyBlackBoard, this);
                BlackboardConditionNode isAtkCollTime = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INREADYTOATK, false, wait);

                ActionAttack actionAttack = new ActionAttack(enemyBlackBoard, this);
                BlackboardConditionNode isHitted = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INHITTEDSTATE, false, actionAttack);

                childList.Add(isAtkCollTime);
                childList.Add(actionAttack);

                SelectorNode selectorNode = new SelectorNode(childList);
                BlackboardConditionNode isTargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.TargetINATKRANGE, true, selectorNode);

                midleNodes.Add(isTargetInAtkRange);
            }

            //MoveTo Action
            {
                ActionMovTo actionMovTo = new ActionMovTo(enemyBlackBoard, this);
                BlackboardConditionNode islocallyControlled = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.ISLOCALLYCONTROLLED, false, actionMovTo);

                midleNodes.Add(islocallyControlled);
            }

            totalChildNodes.Add(new SelectorNode(midleNodes));

            topCompositeNode = new SelectorNode(totalChildNodes);

            rootnode = new RootNode(topCompositeNode);
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

        //public void PlayAnimation(string animName, LeafNode caller)
        //{
        //    if (Animator == null)
        //        return;

        //    currentRunningLeaf = caller;
        //    Animator.Play(animName);
        //}

        //public void OnAnimationEnd(AnimatorStateInfo info)
        //{
        //    if (currentRunningLeaf == null)
        //        return;

        //    currentRunningLeaf.OnAnimationEnd();
        //    currentRunningLeaf = null;
        //}
    }
}



