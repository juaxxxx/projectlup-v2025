using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("설정")]
    public List<Transform> points = new List<Transform>();
    public float moveTime = 2f;

    private bool isMoving = false;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private float currentTime = 0f;

    void Start()
    {
        if (points.Count < 2)
        {
            Debug.LogError("포인트가 2개 이상 필요합니다.");
            enabled = false;
            return;
        }

        transform.position = points[0].position;
    }

    void Update()
    {
        if (!isMoving) return;

        currentTime += Time.deltaTime;
        float t = currentTime / moveTime;

        Vector3 startPos = points[currentIndex].position;
        Vector3 endPos = points[nextIndex].position;

        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (t >= 1f)
            Arrive();
    }

    public void StartMove()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentTime = 0f;
            Debug.Log($"엘리베이터 이동 시작! {currentIndex} → {nextIndex}");
        }
    }

    private void Arrive()
    {
        isMoving = false;
        Debug.Log("엘리베이터 도착!");

        currentIndex = nextIndex;
        nextIndex = (nextIndex + 1) % points.Count;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            other.transform.SetParent(null, true);
        }
    }
}