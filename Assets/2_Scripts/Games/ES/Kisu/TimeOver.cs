using UnityEngine;

namespace LUP.ES
{
    public class TimeOver : MonoBehaviour
    {
        [SerializeField] private GameTimerUI gameTimerUI;

        private EventBroker eventBroker;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();   
        }

        // Update is called once per frame
        void Update()
        {
            if(gameTimerUI == null || eventBroker == null) return;

            if(gameTimerUI.RemainingTime <= 0 )
            {
                eventBroker.ReportGameFinish(false);

                enabled = false;
            }
        }
    }
}