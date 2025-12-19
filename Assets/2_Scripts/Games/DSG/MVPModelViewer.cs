using UnityEngine;

namespace LUP.DSG
{
    public class MVPModelViewer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform modelPoint;
        [SerializeField] private Camera modelCamera;

        private GameObject currentModel;

        public void ShowMVPModel(GameObject modelPrefab)
        {
            Clear();

            if (modelPrefab == null)
            {
                Debug.LogWarning("[MVPModelViewer] modelPrefab is null");
                return;
            }

            if (modelPoint == null)
            {
                Debug.LogError("[MVPModelViewer] modelPoint is not assigned (Inspector).");
                return;
            }

            currentModel = Instantiate(modelPrefab, modelPoint);
            currentModel.transform.localPosition = Vector3.zero;
            currentModel.transform.localRotation = Quaternion.Euler(0, 180, 0);
            currentModel.transform.localScale = Vector3.one * 1.2f;

            int layer = LayerMask.NameToLayer("MVPDisplayModel");
            if (layer == -1)
            {
                Debug.LogError("[MVPModelViewer] Layer 'MVPDisplayModel' does not exist.");
            }
            else
            {
                SetLayerRecursively(currentModel, layer);
            }

            Animator anim = currentModel.GetComponentInChildren<Animator>(true);
            if (anim != null)
            {
                if (anim.runtimeAnimatorController != null)
                {
                    anim.Play("Buff", 0, 0f);
                }
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