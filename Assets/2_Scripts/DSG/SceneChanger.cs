using LUP.Define;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LUP.DSG
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeToDeckEdit()
        {
            LUP.StageManager.Instance.GetCurrentStage().LoadStage(StageKind.DSG, 1);
        }

        public void ChangeToBattle()
        {
            LUP.StageManager.Instance.GetCurrentStage().LoadStage(StageKind.DSG, 2);
        }

        public void ChangeToMain()
        {
            LUP.StageManager.Instance.GetCurrentStage().LoadStage(StageKind.DSG, 0);
        }

        public void ChangeToResult()
        {
            LUP.StageManager.Instance.GetCurrentStage().LoadStage(StageKind.DSG, 3);
        }
    }
}