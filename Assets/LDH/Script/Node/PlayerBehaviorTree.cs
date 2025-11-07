using System.Collections.Generic;
using UnityEngine;

namespace LUP.RL
{
    public class PlayerBehaviorTree : MonoBehaviour
    {
        private Node root;
        [SerializeField] private PlayerBlackBoard bb;
        [SerializeField] private JoyStickSC joystick;
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
    }
}
