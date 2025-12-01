using UnityEngine;
namespace LUP.RL
{
    public static class RotateHelper
    {
        public static void LookAtTarget(Transform self, Transform target, float speed = 1.5f)
        {
            if (target == null) return;

            Vector3 dir = (target.position - self.position).normalized;
            dir.y = 0f;

            if (dir.sqrMagnitude < 0.01f) return;

            Quaternion lookRot = Quaternion.LookRotation(dir);
            self.rotation = Quaternion.Slerp(self.rotation, lookRot, speed);
        }
    }

}