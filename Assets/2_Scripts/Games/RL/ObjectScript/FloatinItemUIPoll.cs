using DG.Tweening;
using Roguelike.Define;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.RL
{
    public class FloatinItemUIPoll : MonoBehaviour
    {
        [SerializeField]
        private int numObjectPool = 8;

        [SerializeField]
        private FloatingItemPopupImage floatingItemUIPrefab;

        private Queue<FloatingItemPopupImage> pool = new Queue<FloatingItemPopupImage>();

        private void Awake()
        {
            //InitializePool();
        }

        public void InitializePool()
        {
            for (int i = 0; i < numObjectPool; i++)
            {
                FloatingItemPopupImage popupItem = Instantiate(floatingItemUIPrefab, transform);
                popupItem.uiState = FloatingImageState.Sleep;
                popupItem.gameObject.SetActive(false);
                pool.Enqueue(popupItem);
            }
        }

        public FloatingItemPopupImage RequestUI()
        {
            FloatingItemPopupImage popupItem = pool.Dequeue();
            popupItem.gameObject.SetActive(true);
            return popupItem;
        }

        public void ReturnUI(FloatingItemPopupImage popupItem)
        {
            popupItem.transform.SetParent(transform);
            popupItem.uiState = FloatingImageState.Sleep;
            popupItem.gameObject.SetActive(false);
            popupItem.OnPopupDisApear.Invoke(popupItem.custumItemID, popupItem.totalGainningAmount);

            pool.Enqueue(popupItem);
        }

    }
}

