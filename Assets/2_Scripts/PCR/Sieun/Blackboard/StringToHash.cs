using UnityEngine;

/// <summary>
/// FNV-1a (Fowler–Noll–Vo) 알고리즘
// 글자를 한 자 한 자 읽으면서 숫자를 섞고(^) 곱해서(*)
// 아주 엉뚱해 보이는 고유한 숫자를 만듦.
/// </summary>

/// 컴퓨터 입장에서 문자열 비교는 매우 느린 작업. 그래서 고유한 숫자 번호표로 대신하려는 목적.
/// "PlayerHealth" -> 이 함수 통과 -> -12938471 (예시)
/*
 // 예시 사용법
int healthKey = "PlayerHealth".ComputeFNV1aHash(); // 미리 ID로 변환

// 나중에 게임 루프 안에서 사용할 때 (훨씬 빠름)
blackboard.SetInt(healthKey, 100);
 */

namespace LUP.PCR
{
    /// 문자열을 고유한 숫자 ID(int)로 변환하는 해시 함수
    public static class StringExtensions
    {
        public static int ComputeFNV1aHash(this string str)
        {
            uint hash = 2166136261; // 수학적으로 계산된 아주 큰 소수

            foreach (char c in str)
            {
                // 현재 hash에 글자 하나(c)를 섞는다
                // XOR 연산(^)은 비트를 뒤집어서 값을 섞는 역할
                hash = (hash ^ c) * 16777619; // 또다른 소수를 곱해서 값을 흩어지게 한다.
                                              // 결과적으로 글자가 비슷해도 결과값이 완전히 달라진다.

            }

            // int 범위(약 -21억 ~ 21억)를 넘어가도 에러 내지 말고 그냥 자르라는 뜻
            return unchecked((int)hash);
        }
    }
}