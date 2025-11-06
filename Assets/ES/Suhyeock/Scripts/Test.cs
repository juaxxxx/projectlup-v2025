using System.Text;
using UnityEngine;

namespace ES
{
    public class Test : MonoBehaviour
    {
        public PlayerBlackboard blackboard;
        StringBuilder HPString = new StringBuilder();
        StringBuilder isCastingInteraction = new StringBuilder();
        StringBuilder isDead = new StringBuilder();

        private void OnGUI()
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 35;
            if (GUI.Button(new Rect(10, 200, 300, 100), "Take Damage", buttonStyle))
            {
                blackboard.healthComponent.HP -= 10.0f;
                blackboard.healthComponent.isHit = true;
            }

            if (GUI.Button(new Rect(10, 310, 300, 100), "Escape", buttonStyle))
            {
                blackboard.eventBroker.ReportGameFinish(true);
            }
            //GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            //labelStyle.fontSize = 50;
            //HPString.Clear();
            //HPString.Append("HP: ").Append(blackboard.HP);
            //GUI.Label(new Rect(10, 200, 200, 100), HPString.ToString(), labelStyle);

            //isCastingInteraction.Clear();
            //if (blackboard.interactingObject != null)
            //{
            //    isCastingInteraction.Append("CastingInteraction");
            //}
            //else
            //{
            //    isCastingInteraction.Append("Not CastingInteraction");
            //}
            //GUI.Label(new Rect(10, 250, 600, 100), isCastingInteraction.ToString(), labelStyle);

            //isDead.Clear();
            //if (blackboard.isDead == true)
            //{
            //    isDead.Append("Die");
            //}
            //else
            //{
            //    isDead.Append("Alive");
            //}
            //GUI.Label(new Rect(10, 300, 600, 100), isDead.ToString(), labelStyle);
        }
    }
}
