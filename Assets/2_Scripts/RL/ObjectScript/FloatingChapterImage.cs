using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class FloatingChapterImage : MonoBehaviour
    {
        public float moveDistance = 50f;
        public float moveSpeed = 2f;

        private RectTransform rectTransform;
        private Vector2 startPos;

        private Sprite ChapterImage;
        private void Awake()
        {
            ChapterImage = gameObject.GetComponent<Image>().sprite;
        }

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            startPos = rectTransform.anchoredPosition;
        }

        void Update()
        {

            float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
        }

        public void SetChapterImage(Sprite chapterImage)
        {
            ChapterImage = chapterImage;
        }
    }
}

