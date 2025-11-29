using UnityEngine;

namespace LUP.ES
{
    public abstract class BTNode
    {
        public abstract NodeState Evaluate();

        public abstract void Reset();
    }
}
