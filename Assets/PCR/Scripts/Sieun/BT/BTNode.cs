using UnityEngine;

namespace PCR
{
    public abstract class BTNode
    {
        public enum WorkerNodeState
        {
            RUNNING,
            SUCCESS,
            FAILURE,
        }
        public abstract WorkerNodeState Evaluate();
    }
}