using UnityEngine;

namespace RL
{
    public abstract class Node
    {


        protected NodeState nodeState = NodeState.None;
        public abstract NodeState Evaluate();



        public void CallWrontState()
        {
            UnityEngine.Debug.LogError("Wrong State Detected");
        }
    }
}

