using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterPreviewAnimation : MonoBehaviour
{
    public Image targetImage;
    public float frameRate = 30f;  // 초당 프레임 수 (fps)
    public bool loop = true;

    private Sprite[] frames;
    private int currentFrame = 0;
    private float timer = 0f;

    private void Start()
    {
        ChangeSpriteSheet("short");
    }

    public void ChangeSpriteSheet(string type)
    {
        string resourcePath = type switch
        {
            "long" => "Image/RL/ChracterImage/PreviewIdleImage/long",
            "middle" => "Image/RL/ChracterImage/PreviewIdleImage/middle",
            "short" => "Image/RL/ChracterImage/PreviewIdleImage/short",
            _ => null
        };

        if (string.IsNullOrEmpty(resourcePath))
        {
            Debug.LogError($"알 수 없는 스프라이트 타입 '{type}' 입니다.");
            return;
        }

        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(resourcePath);

        if (loadedSprites == null || loadedSprites.Length == 0)
        {
            Debug.LogError($"Sprite sheet '{resourcePath}'을(를) 불러오지 못했습니다.");
            return;
        }

        frames = loadedSprites;
        currentFrame = 0;
        timer = 0f;

        // 첫 프레임 즉시 표시
        if (targetImage && frames.Length > 0)
        {
            targetImage.sprite = frames[0];
        }
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;

        float frameDuration = 1f / frameRate;

        if (timer >= frameDuration)
        {
            timer -= frameDuration;
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                    currentFrame = frames.Length - 1;
            }

            targetImage.sprite = frames[currentFrame];
        }
    }
}
