using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace LUP.DSG
{
    public class BattleCameraDirector : MonoBehaviour
    {
        private Vector3 originPosition;

        [SerializeField]
        private Vector3 friendlyIntroCamPosition;

        [SerializeField]
        private Vector3 friendlyIntroCamRotation;

        [SerializeField]
        private Vector3 enemyIntroCamPosition;

        [SerializeField]
        private Vector3 enemyIntroCamRotation;

        private void Awake()
        {
            originPosition = transform.position;
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPos + new Vector3(x, y, 0f);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }

        public Tween PlayBattleIntroSequence()
        {
            Vector3 originPos = transform.position;
            Quaternion originRot = transform.rotation;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(friendlyIntroCamPosition, 2f).SetEase(Ease.OutQuint));
            seq.Join(transform.DORotate(friendlyIntroCamRotation, 2f).SetEase(Ease.OutQuint));
            seq.AppendInterval(0.5f);

            seq.Append(transform.DOMove(enemyIntroCamPosition, 2f));
            seq.Join(transform.DORotate(enemyIntroCamRotation, 1.5f));
            seq.AppendInterval(1.0f);

            seq.Append(transform.DOMove(originPos, 1f));
            seq.Join(transform.DORotateQuaternion(originRot, 1f));

            return seq;
        }

        public void FocusOnTarget(Vector3 targetPosition)
        {
            transform.DOMoveX(targetPosition.x, 0.5f);
        }

        public void BackToOriginPos(float delay = 0f)
        {
            transform.DOMoveX(originPosition.x, 0.5f).SetDelay(delay);
        }

        public Tween FocusOnSkillCaster(Transform targetTransform, Transform cameraOriginPos)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(targetTransform.position, 1f));
            seq.Join(transform.DORotateQuaternion(targetTransform.rotation, 1f));
            seq.AppendInterval(0.5f);

            seq.Append(transform.DOMove(cameraOriginPos.position, 1f));
            seq.Join(transform.DORotateQuaternion(cameraOriginPos.rotation, 1f));

            return seq;
        }
    }
}