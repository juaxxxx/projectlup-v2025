using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ES
{
    public class GameTimerUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI TimerText;
        [SerializeField]
        private int TimeLimitMinutes = 10;

        private float RenamingTimer = 0.0f;
        private bool TimeIsRunning = true;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            RenamingTimer = TimeLimitMinutes * 60.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (TimeIsRunning)
            {
                if (RenamingTimer > 0.0f)
                {
                    RenamingTimer -= Time.deltaTime;
                }
                else
                {
                    RenamingTimer = 0.0f;
                    TimeIsRunning = false;
                }
                DisplayTime();
            }    
        }

        void DisplayTime()
        {
            float Minutes = Mathf.FloorToInt(RenamingTimer / 60.0f);
            float Seconds = Mathf.FloorToInt(RenamingTimer % 60.0f);

            TimerText.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
        }
    }
}
