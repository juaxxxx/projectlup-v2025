using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace LUP.RL
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
                ActionHitted actionHitted = new ActionHitted(enemyBlackBoard, this);

                List<(ConditionCheckEnum, bool)> whishConditions = new()
                {
                    (ConditionCheckEnum.INATKSTATE, false),
                    (ConditionCheckEnum.INRAMPAGE, false)
                };

                BlackboardMultiConditionNode inAttack_InRampage = new BlackboardMultiConditionNode(enemyBlackBoard, whishConditions, actionHitted);

                BlackboardConditionNode isRampage_TargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INRAMPAGE, true, MakeAtkDefaultNode());

                List<Node> bottomSelectorChildNodes = new List<Node>();

                bottomSelectorChildNodes.Add(inAttack_InRampage);
                bottomSelectorChildNodes.Add(isRampage_TargetInAtkRange);

                SelectorNode bottomSelector = new SelectorNode(bottomSelectorChildNodes);




                List<Node> topSequenceChildNodes = new List<Node>();

                ReduceHP reduceHP = new ReduceHP(enemyBlackBoard, this);

                topSequenceChildNodes.Add(reduceHP);
                topSequenceChildNodes.Add(bottomSelector);

                SequenceNode topSequence = new SequenceNode(topSequenceChildNodes);
                BlackboardConditionNode isOnHit = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.OnHit, true, topSequence);


                midleNodes.Add(isOnHit);
            }

            //Attack Action
            {
                //List<Node> childList = new List<Node>();

                //Wait wait = new Wait(enemyBlackBoard, this);
                //BlackboardConditionNode isAtkCollTime = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INREADYTOATK, false, wait);

                //ActionAttack actionAttack = new ActionAttack(enemyBlackBoard, this);

                //List<(ConditionCheckEnum, bool)> whishConditions = new()
                //{
                //    (ConditionCheckEnum.INHITTEDSTATE, false),
                //    (ConditionCheckEnum.INREADYTOATK, true)
                //};
                //BlackboardMultiConditionNode inHittedState_InReadyToAtk = new BlackboardMultiConditionNode(enemyBlackBoard, whishConditions, actionAttack);

                //childList.Add(isAtkCollTime);
                //childList.Add(inHittedState_InReadyToAtk);

                //SelectorNode selectorNode = new SelectorNode(childList);
                //BlackboardConditionNode isTargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.TargetINATKRANGE, true, selectorNode);

                //midleNodes.Add(isTargetInAtkRange);

                midleNodes.Add(MakeAtkDefaultNode());
            }

            //MoveTo Action
            {
                ActionMoveTo actionMovTo = new ActionMoveTo(enemyBlackBoard, this);

                List<(ConditionCheckEnum, bool)> whishConditions = new()
                {
                    (ConditionCheckEnum.ISLOCALLYCONTROLLED, false),
                    (ConditionCheckEnum.INATKSTATE, false)
                };

                //BlackboardConditionNode islocallyControlled = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.ISLOCALLYCONTROLLED, false, actionMovTo);

                BlackboardMultiConditionNode isLocallyControlled_InAtkState = new BlackboardMultiConditionNode(enemyBlackBoard, whishConditions, actionMovTo);

                midleNodes.Add(isLocallyControlled_InAtkState);
            }

            totalChildNodes.Add(new SelectorNode(midleNodes));

            topCompositeNode = new SelectorNode(totalChildNodes);

            rootnode = new RootNode(topCompositeNode);
        }

        // Update is called once per frame
        void Update()
        {
            //if (enemyBlackBoard.Alive == false)
            //    return;

            if (rootnode != null)
            {
                rootnode.Evaluate();
            }

            enemyBlackBoard.UpdateBlackBoard();
        }


        Node MakeAtkDefaultNode()
        {
            List<Node> childList = new List<Node>();

            Wait wait = new Wait(enemyBlackBoard, this);
            BlackboardConditionNode isAtkCollTime = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INREADYTOATK, false, wait);

            ActionAttack actionAttack = new ActionAttack(enemyBlackBoard, this);

            List<(ConditionCheckEnum, bool)> whishConditions = new()
                {
                    //(ConditionCheckEnum.INHITTEDSTATE, false),
                    (ConditionCheckEnum.INREADYTOATK, true)
                };
            BlackboardMultiConditionNode inHittedState_InReadyToAtk = new BlackboardMultiConditionNode(enemyBlackBoard, whishConditions, actionAttack);

            childList.Add(isAtkCollTime);
            childList.Add(inHittedState_InReadyToAtk);

            SelectorNode selectorNode = new SelectorNode(childList);
            BlackboardConditionNode isTargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.TargetINATKRANGE, true, selectorNode);

            return isTargetInAtkRange;
        }
    }
}



