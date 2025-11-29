using UnityEngine;

namespace LUP.PCR
{
    public class GlobalBlackboard : MonoBehaviour
    {
        public static GlobalBlackboard Instance { get; private set; }
        public WorkerBlackboard BB { get; private set; } = new WorkerBlackboard();

        // 전역 이벤트 예 : 낮/밤 변경 등
        // public event Action<string> OnGloablEvent

        //public void RaiseGlobalEvent(string evt)
        //{
        //   // OnGlobalEvent?.Invoke(evt);
        //}

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(this); return; }
            Instance = this;
        }

        











    }

}
