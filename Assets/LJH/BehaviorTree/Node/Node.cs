using UnityEngine;

namespace RL
{
    public abstract class Node
    {


        protected NodeState nodeState;
        public abstract NodeState Evaluate();



        public void CallWrontState()
        {
            UnityEngine.Debug.LogError("Wrong State Detected");
        }
    }
}

