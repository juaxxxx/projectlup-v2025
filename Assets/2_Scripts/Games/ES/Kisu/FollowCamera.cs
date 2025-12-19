using UnityEngine;
using UnityEngine.UIElements;

namespace LUP.ES
{
    public class FollowCamera : MonoBehaviour
    { 
        [SerializeField] 
        private Joystick aimJoystick;
        [SerializeField] 
        private bool followX = true, followY = true, followZ = true;
        [SerializeField] 
        private Vector3 defaultOffset = new Vector3(0, 6, -6);

        [SerializeField] 
        private float smoothTime = 0.12f; // 값이 클수록 더 천천히 따라갑니다
        [SerializeField] 
        private float maxSpeed = 20f;


        [SerializeField] 
        private float lookAheadDistance = 3.0f; // 조준 시 카메라가 이동하는 최대 거리
        [SerializeField] 
        private float lookAheadSmoothTime = 0.2f; // 조준 이동의 부드러움 정도
        [SerializeField] 
        private float deadZone = 0.1f; // 조이스틱 입력 무시 임계값

        private Transform target;
        private Vector3 currentVelocity;
        private Vector3 currentAimOffset;
        private Vector3 aimVelocity;

        private float shakeDuration = 0f;
        private float shakeMagnitude = 0f;
        private Vector3 shakeOffset = Vector3.zero;

        private void Start()
        {
            target = FindAnyObjectByType<PlayerBlackboard>().transform;
        }
        private void LateUpdate()
        {
            if (target == null) return;

            //float targetX = followX ? target.position.x + defaultOffset.x : transform.position.x;
            //float targetY = followY ? target.position.y + defaultOffset.y : transform.position.y;
            //float targetZ = followZ ? target.position.z + defaultOffset.z : transform.position.z;

            //Vector3 baseTargetPos = new Vector3(targetX, targetY, targetZ);

            Vector3 targetAimOffset = Vector3.zero;

            if (aimJoystick != null)
            {
                // 조이스틱의 방향 (x, y)를 읽어옴
                Vector2 input = aimJoystick.Direction;

                // 입력값이 데드존(살짝 건드림)보다 클 때만 카메라 이동
                if (input.magnitude > deadZone)
                {
                    // 쿼터뷰(3D)이므로 조이스틱 Y를 월드 Z로 변환
                    targetAimOffset = new Vector3(input.x, 0, input.y) * lookAheadDistance;
                }
            }

            currentAimOffset = Vector3.SmoothDamp(currentAimOffset, targetAimOffset, ref aimVelocity, lookAheadSmoothTime);

            Vector3 idealCenterPos = target.position + defaultOffset;

            Vector3 desiredPosition = new Vector3(
                followX ? idealCenterPos.x : transform.position.x,
                followY ? idealCenterPos.y : transform.position.y,
                followZ ? idealCenterPos.z : transform.position.z
            );

            // 4. 최종 목표 위치 (플레이어 기준 위치 + 조준 오프셋)
            Vector3 finalTargetPos = desiredPosition + currentAimOffset;

            // 5. 카메라 본체 이동 (SmoothDamp)
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, finalTargetPos, ref currentVelocity, smoothTime, maxSpeed);

            // 6. 쉐이크 효과 적용
            UpdateShake();

            // 최종 위치 반영
            transform.position = smoothedPosition + shakeOffset;

            //transform.position = new Vector3(
            //    (x ? target.position.x : transform.position.x),
            //    (y ? target.position.y : transform.position.y),
            //    (z ? target.position.z - 6 : transform.position.z));
        }

        // 총을 쏠 때 호출할 함수
        public void Shake(float duration, float magnitude)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }

        private void UpdateShake()
        {
            if (shakeDuration > 0)
            {
                Vector2 randomCircle = Random.insideUnitCircle * shakeMagnitude;
                shakeOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

                shakeDuration -= Time.deltaTime;
            }
            else
            {
                shakeOffset = Vector3.zero;
            }
        }
    }
}