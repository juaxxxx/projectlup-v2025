using System.Collections.Generic;
using UnityEngine;

namespace RL
{
    public class PlayerBehaviorTree : MonoBehaviour
    {
        private Node root;
        [SerializeField] private PlayerBlackBoard bb;
        [SerializeField] private JoyStickSC joystick;
        [SerializeField] private Archer playerArcher;
        private void Awake()
        {
            if (bb == null) bb = new PlayerBlackBoard();
            if (joystick == null) joystick = FindFirstObjectByType<JoyStickSC>();
            bb.Health = playerArcher;
     
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

            var actionSelector = new SelectorNode(new List<Node> { attackNode, moveNode });
            var mainSequence = new SequenceNode(new List<Node> { isAlive, isHitted, actionSelector });

            root = mainSequence;
        }

        private void Update()
        {
            root?.Evaluate();
        }
    }
}
