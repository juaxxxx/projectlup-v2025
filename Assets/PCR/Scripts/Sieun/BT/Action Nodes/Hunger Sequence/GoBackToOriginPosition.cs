//using UnityEngine;

//namespace LUP.PCR
//{
//    public class GoBackToOriginPosition : BTNode
//    {
//        Worker worker;
//        bool arrived = false;
    
    
//        public override WorkerNodeState Evaluate(WorkerAI worker)
//        {
//            {
//            if (!arrived)
//                Debug.Log("원래 자리로 복귀 중...");
//                worker.MoveTo(worker.originSpot);
    
//                if (!worker.IsAt(worker.originSpot))
//                    return WorkerNodeState.RUNNING;
    
//                arrived = true;
//                Debug.Log("원래 자리 복귀 완료.");
//            }
//            return WorkerNodeState.SUCCESS;
//        }
//    }
//}