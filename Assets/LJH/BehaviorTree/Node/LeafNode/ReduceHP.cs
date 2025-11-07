using UnityEngine;

namespace RL
{
    public class ReduceHP : LeafNode
    {
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Reduce HP");
            return NodeState.Success;
        }
    }
}

