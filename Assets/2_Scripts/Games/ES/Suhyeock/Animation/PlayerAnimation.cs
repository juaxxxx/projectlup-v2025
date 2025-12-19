using LUP.PCR;
using UnityEngine;

namespace LUP.ES
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Transform spine;
        [HideInInspector]
        public PlayerBlackboard blackboard;
        public Vector3 spineOffset;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            blackboard = GetComponent<PlayerBlackboard>();
            blackboard.animator.SetLayerWeight(1, 1);
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            if (blackboard.healthComponent.isDead == true)
                return;
            UpdateAnimation();
            //if (blackboard.weapon.state == WeaponState.ATTACKING ||
            //    blackboard.weapon.state == WeaponState.RELOADING ||
            //    blackboard.moveState == MoveState.IDLE)
            //{
            //}
            
            Vector3 targetPosition = spine.transform.position + transform.forward * 10f;
            spine.transform.LookAt(targetPosition);
            spine.Rotate(spineOffset);
        }

        void UpdateAnimation()
        {
            Vector3 moveInput = new Vector3(blackboard.leftJoystick.Horizontal, 0, blackboard.leftJoystick.Vertical);
            if (moveInput.magnitude > 0.01f)
            {
                moveInput = moveInput.normalized;
            }

            Vector3 localMove = transform.InverseTransformDirection(moveInput);
            blackboard.animator.SetFloat("InputX", localMove.x, 0.1f, Time.deltaTime);
            blackboard.animator.SetFloat("InputY", localMove.z, 0.1f, Time.deltaTime);

            if (blackboard.weapon.state == WeaponState.ATTACKING ||
                blackboard.weapon.state == WeaponState.RELOADING ||
                blackboard.moveState == MoveState.MOVING ||
                blackboard.moveState == MoveState.IDLE)
            {
                blackboard.animator.SetLayerWeight(1, 1);
            }
            else
            {
                blackboard.animator.SetLayerWeight(1, 0);
            }
        }
    }

}
