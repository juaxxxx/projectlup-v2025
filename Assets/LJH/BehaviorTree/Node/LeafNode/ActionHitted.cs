using UnityEngine;

namespace RL
{
    public class ActionHitted : LeafNode
    {
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Hitted");
            return NodeState.Success;
        }
    }
}

