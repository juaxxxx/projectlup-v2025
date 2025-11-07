using UnityEngine;

namespace RL
{
    public class ActionMovTo : LeafNode
    {
        public override NodeState Evaluate()
        {
            UnityEngine.Debug.Log("Action MoveTo");
            return NodeState.Success;
        }
    }
}

