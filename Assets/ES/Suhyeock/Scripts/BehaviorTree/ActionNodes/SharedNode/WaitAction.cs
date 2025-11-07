using UnityEngine;

namespace ES
{
    public class WaitAction : BTNode
    {
        private float waitTime;
        private float currentTime = 0;
        public WaitAction(float duration)
        {
            waitTime = duration;
        }

        public override NodeState Evaluate()
        {
            if (currentTime >= waitTime)
            {
                currentTime = 0;
                return NodeState.Success;
            }
            
            currentTime += Time.deltaTime;
            return NodeState.Running;
        }

        public override void Reset()
        {
            currentTime = 0;
        }
    }
}

