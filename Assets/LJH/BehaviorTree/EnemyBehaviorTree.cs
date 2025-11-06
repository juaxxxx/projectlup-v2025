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

                ActionHitted actionHitted = new ActionHitted();

                BlackboardConditionNode isAttackState = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INATKSTATE, false, actionHitted);
                BlackboardConditionNode isRampagesTATE = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INRAMPAGE, false, actionHitted);

                ReduceHP reduceHP = new ReduceHP();


                childList.Add(isAttackState);
                childList.Add(isRampagesTATE);
                childList.Add(reduceHP);


                SequenceNode atkSequenceNode = new SequenceNode(childList);
                BlackboardConditionNode isHitted = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INHITTEDSTATE, true, atkSequenceNode);


                midleNodes.Add(isHitted);
            }

            //Attack Action
            {
                List<Node> childList = new List<Node>();

                Wait wait = new Wait();
                BlackboardConditionNode isAtkCollTime = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INATKREADY, true, wait);

                ActionAttack actionAttack = new ActionAttack();
                BlackboardConditionNode isHitted = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.INHITTEDSTATE, false, actionAttack);

                childList.Add(isAtkCollTime);
                childList.Add(actionAttack);

                SelectorNode selectorNode = new SelectorNode(childList);
                BlackboardConditionNode isTargetInAtkRange = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.TARTINATKRANGE, false, actionAttack);

                midleNodes.Add(isTargetInAtkRange);
            }

            //MoveTo Action
            {
                ActionMovTo actionMovTo = new ActionMovTo();
                BlackboardConditionNode islocallyControlled = new BlackboardConditionNode(enemyBlackBoard, ConditionCheckEnum.ISLOCALLYCONTROLLED, false, actionMovTo);

                midleNodes.Add(islocallyControlled);
            }

            totalChildNodes.Add(new SequenceNode(midleNodes));

            topCompositeNode = new SelectorNode(totalChildNodes);

            rootnode = new RootNode(topCompositeNode);
        }

        // Update is called once per frame
        void Update()
        {
            if (enemyBlackBoard.Alive == false)
                return;

            if(rootnode != null)
            {
                rootnode.Evaluate();
            }
            
        }
    }
}



