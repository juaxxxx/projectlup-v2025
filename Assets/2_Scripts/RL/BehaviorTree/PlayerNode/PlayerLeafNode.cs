using UnityEngine;

namespace LUP.RL
{
    public abstract class PlayerLeafNode : Node
    {
        public abstract void OnPlayerAnimationEnd(AnimatorStateInfo animInfo);
        public virtual void OnAnimationInTargetRate() { }
    }
}

