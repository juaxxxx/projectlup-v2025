using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RawImage))]
public class MovingImage : MonoBehaviour
{
    [Header("Inspector에서 직접 할당")]
    public Texture2D[] frames;   // 여기에 PNG 80장 드래그

    [Header("재생 설정")]
    public float frameRate = 24f;
    public bool loop = true;
    public bool playOnAwake = true;

    private RawImage rawImage;
    private Coroutine playCoroutine;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();

        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning("frames 배열에 PNG를 넣어주세요!");
        }
    }

    void Start()
    {
        if (playOnAwake) Play();
    }

    public void Play()
    {
        if (frames == null || frames.Length == 0) return;

        Stop();
        playCoroutine = StartCoroutine(PlayLoop());
    }

    public void Stop()
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
    }

    private IEnumerator PlayLoop()
    {
        int idx = 0;
        float delay = 1f / Mathf.Max(0.0001f, frameRate);

        while (true)
        {
            rawImage.texture = frames[idx];
            idx = (idx + 1) % frames.Length;

            if (!loop && idx == 0) yield break;

            yield return new WaitForSeconds(delay);
        }
    }
}
