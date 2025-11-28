using System.Collections.Generic;
namespace LUP.ST
{

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public abstract class BaseNode
    {
        protected List<BaseNode> children = new List<BaseNode>();
        protected NodeState state;

        public BaseNode runningChild = null;
        public BaseNode() { }
        public BaseNode(List<BaseNode> children)
        {
            this.children = children;
        }

        public abstract NodeState Evaluate();
        public virtual void Reset()
        {
            runningChild = null;
        }
    }

}