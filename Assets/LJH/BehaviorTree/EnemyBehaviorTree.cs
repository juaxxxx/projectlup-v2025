using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RL
{
    public class EnemyBehaviorTree : MonoBehaviour
    {

        private RootNode rootnode;
        private EnemyBlackBoard enemyBlackBoard;
        private CompositeNode topCompositeNode;

        private void Awake()
        {
            enemyBlackBoard = GetComponent<EnemyBlackBoard>();
            enemyBlackBoard.HP = enemyBlackBoard.MaxHP;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InitBehaviorTree();
        }

        void InitBehaviorTree()
        {
            List<Node> totalChildNodes = new List<Node>();

            //Die Action
            {
                ActionDie actionDie = new ActionDie();
                BlackboardConditionNode isAlive = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.ISALIVE, false, actionDie);

                totalChildNodes.Add(isAlive);
            }

            List<Node> midleNodes = new List<Node>();

            //Hitted Action
            {
                List<Node> childList = new List<Node>();

                ReduceHP reduceHP = new ReduceHP();

                ActionHitted actionHitted = new ActionHitted();

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

                Wait wait = new Wait();
                BlackboardConditionNode isAtkCollTime = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INREADYTOATK, false, wait);

                ActionAttack actionAttack = new ActionAttack();
                BlackboardConditionNode isHitted = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INHITTEDSTATE, false, actionAttack);

                childList.Add(isAtkCollTime);
                childList.Add(actionAttack);

                SelectorNode selectorNode = new SelectorNode(childList);
                BlackboardConditionNode isTargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.TargetINATKRANGE, true, selectorNode);

                midleNodes.Add(isTargetInAtkRange);
            }

            //MoveTo Action
            {
                ActionMovTo actionMovTo = new ActionMovTo();
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
    }
}



