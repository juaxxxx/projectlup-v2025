using UnityEngine;

namespace RL
{
    public class ActionAttack : LeafNode
    {
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Attack");
            return NodeState.Success;
        }
    }
}

