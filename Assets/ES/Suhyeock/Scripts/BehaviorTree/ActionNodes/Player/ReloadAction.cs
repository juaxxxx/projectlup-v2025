using System.Collections;
using UnityEngine;

namespace ES
{
    public class ReloadAction : BTNode
    {
        PlayerBlackboard blackboard;
        private bool isReloadingStarted = false;

        public ReloadAction(PlayerBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override NodeState Evaluate()
        {
            if (blackboard.gun.state == GunState.RELOADING)
            {
                return NodeState.Running;
            }
            else if (blackboard.gun.state == GunState.READY && isReloadingStarted)
            {
                isReloadingStarted = false;
                return NodeState.Success;
            }
            blackboard.gun.Reload();
            isReloadingStarted = true;
            blackboard.isReloadButtonPressed = false;
            return NodeState.Running;
        }

        public override void Reset()
        {

        }
    }
}
