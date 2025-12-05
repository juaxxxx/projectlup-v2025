using UnityEngine;

namespace LUP
{
    public class QuestSample : MonoBehaviour, IQuestTarget
    {
        public int QuestTargetId { get; private set; } = 1000;

        public void Trigger(int value)
        {
            QuestManager.Instance.Trigger(QuestTargetId, value);
        }
    }
}

