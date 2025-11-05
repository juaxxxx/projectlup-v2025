using UnityEngine;

namespace RL
{
    public abstract class Node
    {
        protected NodeState nodeState;
        public abstract NodeState Evaluate();
    }
}

