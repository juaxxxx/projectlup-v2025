using UnityEngine;

namespace LUP.PCR
{

    public class GoToNewTaskLocation : WorkerBlackboardNode
    {
        private BuildingBase taskPlace;

        public GoToNewTaskLocation(WorkerBlackboard blackboard) : base(blackboard) { }

        protected override void OnStart()
        {
            if (HasData(BBKeys.TargetBuilding))
            {
                taskPlace = GetData<BuildingBase>(BBKeys.TargetBuilding);

                if (taskPlace == null)
                {
                    Debug.Log("2-2. 이동할 작업 장소가 없습니다.");
                }
                if (Mover != null)
                {
                    Mover.SetDestination(taskPlace.entrancePos);
                }
            }

        }
        protected override NodeState OnUpdate()
        {
            if (Mover == null || taskPlace == null) { return NodeState.FAILURE; }

            if (Mover.IsArrived())
            {
                Debug.Log("2-2. 새 작업지 도착.");
                //@TODO : 작업건물에 작업자가 도착했음을 어떻게 알릴지 고민하기
                return NodeState.SUCCESS;
            }
            else
            {
                Mover.MoveAlongPath();
                Debug.Log("2-2. 새 작업지로 이동 중...");

                return NodeState.RUNNING;
            }
        }
    }
}