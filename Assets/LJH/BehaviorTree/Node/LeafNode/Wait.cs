using UnityEngine;

namespace RL
{
    public class Wait : LeafNode
    {
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action Wait");
            return NodeState.Success;
        }
    }
}

