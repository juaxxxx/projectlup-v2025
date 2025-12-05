using UnityEngine;

namespace LUP
{
    [System.Serializable]
    public struct QuestGoal
    {
        public int TargetId;     
        public int GoalAmount;   
    }

    [CreateAssetMenu(menuName = "Quest/BaseQuest", fileName = "NewBaseQuest")]
    public class BaseQuest : ScriptableObject
    {
        public string QuestName;
        public QuestGoal[] Goals;

        [System.NonSerialized]
        public int[] CurrentAmounts;

        [System.NonSerialized]
        public bool IsCompleted = false;
        public virtual void Initialize()
        {
            CurrentAmounts = new int[Goals.Length];
            for (int i = 0; i < CurrentAmounts.Length; i++)
                CurrentAmounts[i] = 0;
        }

        public virtual void OnTargetTriggered(int targetId, int value)
        {
            for (int i = 0; i < Goals.Length; i++)
            {
                if (Goals[i].TargetId == targetId)
                {
                    CurrentAmounts[i] += value;
                    Debug.Log($"[{name}] 목표({targetId}) 진행도: {CurrentAmounts[i]} / {Goals[i].GoalAmount}");

                    if (CheckAllCompleted())
                    {
                        CompleteQuest();
                    }
                }
            }
        }
        private bool CheckAllCompleted()
        {
            for (int i = 0; i < Goals.Length; i++)
            {
                if (CurrentAmounts[i] < Goals[i].GoalAmount)
                    return false;
            }

            return true;
        }
        protected virtual void CompleteQuest()
        {
            IsCompleted = true;
            Debug.Log($"퀘스트 완료: {name}");
            QuestManager.Instance.CompleteQuest(this);
        }
    }
}

