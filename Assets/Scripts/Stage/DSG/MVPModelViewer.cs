using UnityEngine;

namespace LUP.DSG
{
    public class MVPModelViewer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform modelPoint;
        [SerializeField] private Camera modelCamera;

        private GameObject currentModel;

        /// <summary>
        /// MVP 캐릭터 프리팹을 화면에 표시
        /// </summary>
        public void ShowMVPModel(GameObject modelPrefab)
        {
            Clear();
            DataCenter dataCenter = FindFirstObjectByType<DataCenter>();
            if (modelPrefab == null)
            {
                Debug.LogWarning("MVP 모델 프리팹이 없습니다!");
                return;
            }

            var mvp = dataCenter.mvpData;

            // 모델 생성
            currentModel = Instantiate(modelPrefab, modelPoint);
            currentModel.transform.localPosition = Vector3.zero;
            currentModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            currentModel.transform.localScale = Vector3.one * 1.2f;

            Character character = currentModel.GetComponent<Character>();
            if(character != null)
            {
                character.SetSkinColor(mvp.char1Color);
            }


            // 전용 레이어 적용 (카메라에만 보이게)
            int layer = LayerMask.NameToLayer("MVPDisplayModel");
            SetLayerRecursively(currentModel, layer);

            // 선택적으로 애니메이션 Idle 실행 (있을 경우)
            Animator anim = currentModel.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("Idle", 0, 0f);
            }
        }

        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        public void Clear()
        {
            if (currentModel != null)
            {
                Destroy(currentModel);
                currentModel = null;
            }
        }
    }
}