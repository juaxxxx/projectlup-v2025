using System.Collections;
using UnityEngine;
namespace LUP.PCR
{
    public class Restaurant : MonoBehaviour
    {
        public RestaurantStatus data;
        private float cookingTimer = 0f;
        private float cookingInterval => 10f / (1 + data.cookingSpeedPercent); // 속도에 따라 간격 조정
        private bool isCooking = false;

        // Memento 패턴용
        private RestaurantMemento memento;

        void Start()
        {
            memento = new RestaurantMemento(data.level, data.foodStorage, data.waterStorage, data.cookingSpeedPercent, data.eatingEfficiency);
        }

        void Update()
        {
            if (isCooking)
            {
                cookingTimer += Time.deltaTime;
                if (cookingTimer >= cookingInterval)
                {
                    cookingTimer = 0f;
                    ProduceFood(1); // 1씩 생산
                }
            }
        }

        void ProduceFood(int amount)
        {
            data.currentFood += amount;
            if (data.currentFood > data.foodStorage)
                data.currentFood = data.foodStorage;
        }

        // Command 패턴: 요리 시작/중지
        public void StartCooking() => isCooking = true;
        public void StopCooking() => isCooking = false;

        // 업그레이드
        public void LevelUp()
        {
            if (data.level >= data.maxLevel) return;

            data.level++;
            int levelIndex = Mathf.Min(data.level - 1, data.levelUpFoodStorage.Length - 1);
            data.foodStorage += data.levelUpFoodStorage[levelIndex];
            data.waterStorage += data.levelUpWaterStorage[levelIndex];

            // 요리 속도 증가 (예시)
            if (data.level <= 19) data.cookingSpeedPercent += 0.02f;
            else if (data.level <= 29) data.cookingSpeedPercent += 0.04f;
            else data.cookingSpeedPercent += 0.06f;

            // 취식 효율
            if (data.level <= 10) data.eatingEfficiency += 0.05f;
            else if (data.level <= 15) data.eatingEfficiency += 0.15f;
            else if (data.level <= 20) data.eatingEfficiency += 0.2f;
            else if (data.level <= 29) data.eatingEfficiency += 0.4f;
            else data.eatingEfficiency += 0.8f;

            SaveState();
        }

        // Memento 패턴: 상태 저장
        public void SaveState()
        {
            memento = new RestaurantMemento(data.level, data.foodStorage, data.waterStorage, data.cookingSpeedPercent, data.eatingEfficiency);
        }

        public void RestoreState()
        {
            if (memento == null) return;
            data.level = memento.level;
            data.foodStorage = memento.foodStorage;
            data.waterStorage = memento.waterStorage;
            data.cookingSpeedPercent = memento.cookingSpeedPercent;
            data.eatingEfficiency = memento.eatingEfficiency;
        }

        // Client

        // OnGUI 테스트용
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 500));
            GUILayout.Label($"[건물] {data.buildingName} (Lv {data.level})");
            GUILayout.Label($"음식: {data.currentFood}/{data.foodStorage}");
            GUILayout.Label($"물: {data.currentWater}/{data.waterStorage}");
            GUILayout.Label($"요리 속도: {data.cookingSpeedPercent * 100:F1}%");
            GUILayout.Label($"취식 효율: {data.eatingEfficiency * 100:F1}%");

            if (GUILayout.Button(isCooking ? "요리 중지" : "요리 시작"))
            {
                if (isCooking) StopCooking();
                else StartCooking();
            }

            if (GUILayout.Button("레벨 업"))
            {
                LevelUp();
            }

            if (GUILayout.Button("상태 복원"))
            {
                RestoreState();
            }

            GUILayout.EndArea();
        }
    }
}
