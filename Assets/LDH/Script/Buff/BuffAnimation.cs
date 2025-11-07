using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Roguelike.Define;
namespace RL
{
    public class BuffRouletteManager : MonoBehaviour
    {
        public BuffSlot[] slots;          // 슬롯 3개
        public Archer archer;

        [Header("버프 풀")]
        public List<BuffData> allBuffs;

        public void StartRoulette(Archer owner)
        {
            archer = owner;
            allBuffs = owner.allBuffs;
            StartCoroutine(SpinAll());
        }

        IEnumerator SpinAll()
        {
            // 슬롯 돌리기 시작
            foreach (var slot in slots)
                StartCoroutine(slot.Spin(allBuffs));

            // 다 멈출 때까지 대기
            yield return new WaitUntil(() => AllStopped());

            // 룰렛이 멈춘 후 실제 버프 확정 생성 로직 호출
            //archer.CreateBuffOptions(3);
        }

        bool AllStopped()
        {
            foreach (var slot in slots)
                if (slot.isSpinning) return false;
            return true;
        }
    }
}