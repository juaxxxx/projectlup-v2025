using Manager;
using UnityEngine;

namespace Manager
{
    public class DebugStage : BaseStage
    {
        public Define.StageKind TargetStage = Define.StageKind.Main;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //StageManager.Instance.currentStage = this;

            StageManager.Instance.LoadStage(TargetStage);
        }
        protected override void LoadResources()
        {
            //resource = ResourceManager.Instance.Load...
        }

        protected override void GetDatas()
        {
            //data = DataManager.Instance.GetData...
        }

        protected override void SaveDatas()
        {
            
        }
    }
}

