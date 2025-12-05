using LUP;
using System.Collections.Generic;
using UnityEngine;
namespace LUP
{
    public class QuestManager : Singleton<QuestManager>
    {
        private List<BaseQuest> activeQuests = new();

        private void Awake()
        {
            base.Awake();
        }

        public void AddQuest(BaseQuest quest)
        {
            activeQuests.Add(quest);
            quest.Initialize();
        }

        public void Trigger(int questTargetId, int value)
        {
            foreach (var quest in activeQuests)
            {
                quest.OnTargetTriggered(questTargetId, value);
            }
        }
        public void CompleteQuest(BaseQuest quest)
        {
            Debug.Log($"퀘스트 종료 처리: {quest.name}");
            activeQuests.Remove(quest);
        }
    }
}

