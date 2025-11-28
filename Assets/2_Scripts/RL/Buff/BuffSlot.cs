using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Roguelike.Define;  // BuffData 사용하려면 네 프로젝트 네임스페이스 맞게 수정

public class BuffSlot : MonoBehaviour
{
    [Header("UI 구성요소")]
    public RectTransform content;    // 슬롯 안에 들어있는 아이콘 묶음
    public Image[] buffIcons;        // content 아래에 있는 아이콘들 (Image 컴포넌트)
    public float scrollSpeed = 800f;
    public float resetY = -600f;
    public float startY = 600f;

    [HideInInspector] public bool isSpinning = false;
    [HideInInspector] public BuffData currentBuff; // 최종적으로 선택된 버프 저장

    // 슬롯 회전 코루틴
    public IEnumerator Spin(List<BuffData> allBuffs)
    {
        isSpinning = true;

        float spinTime = Random.Range(2f, 3.5f); // 각 슬롯별 랜덤 회전시간
        float timer = 0f;

        while (timer < spinTime)
        {
            // 아래로 이동
            content.anchoredPosition -= new Vector2(0, scrollSpeed * Time.unscaledDeltaTime);

            // 범위 벗어나면 위로 리셋
            if (content.anchoredPosition.y < resetY)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, startY);

                // 아이콘 이미지 랜덤 변경
                foreach (var icon in buffIcons)
                {

                    BuffData randBuff = allBuffs[Random.Range(0, allBuffs.Count)];
                    icon.sprite = randBuff.GetDisplayableImage();
                }
            }

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // 감속 구간
        float speed = scrollSpeed;
        while (speed > 0)
        {
            content.anchoredPosition -= new Vector2(0, speed * Time.unscaledDeltaTime);
            if (content.anchoredPosition.y < resetY)
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, startY);
            speed -= 500f * Time.unscaledDeltaTime;
            yield return null;
        }

        // 멈출 때 최종 버프 확정
        currentBuff = allBuffs[Random.Range(0, allBuffs.Count)];
        isSpinning = false;
    }
}
