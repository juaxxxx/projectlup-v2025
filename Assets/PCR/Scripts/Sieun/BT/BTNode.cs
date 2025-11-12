using UnityEngine;

namespace LUP.PCR
{
    public abstract class BTNode
    {
        public enum WorkerNodeState
        {
            RUNNING,
            SUCCESS,
            FAILURE,
        }
        public abstract WorkerNodeState Evaluate(WorkerAI worker);
    }
}