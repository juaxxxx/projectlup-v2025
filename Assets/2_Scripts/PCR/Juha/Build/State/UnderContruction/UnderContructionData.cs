using UnityEngine;

namespace LUP.PCR
{
    public struct UnderContructionData
    {
        public float elapsedTime;       // 누적 진행 시간
        public float totalTime;         // 총 건설 시간
        public float progressRatio;     // 진행률 (누적 진행 시간 / 총 건설 시간)
        public bool isCompledted;       // 완료 여부

        public void Reset(float totalTime)
        {
            elapsedTime = 0f;
            this.totalTime = totalTime;
            progressRatio = 0f;
            isCompledted = false;
        }

        public void Tick(float deltaTime)
        {
            if (isCompledted)
            {
                return;
            }

            elapsedTime += deltaTime;
            progressRatio = Mathf.Clamp01(elapsedTime / totalTime);

            if (progressRatio >= 1f)
            {
                isCompledted = true;
            }
        }

        public bool IsCompleted()
        {
            return isCompledted;
        }
    }

}

