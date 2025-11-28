using LUP.ES;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IInteractable
{
    [Header("설정")]
    public float moveTime = 2f;
    public Vector3 endOffset = new Vector3(0, 5, 0);

    [Header("참조")]
    private InteractionUIController interactionUI;

    private bool isMoving = false;
    private bool isArrived = false;
    private float currentTime = 0f;
    private Vector3 startPos;
    private Vector3 endPos;

    private Vector3 lastPosition;
    private Vector3 deltaMovement;

    // 플랫폼 위에 있는 CharacterController 저장
    private List<CharacterController> passengers = new List<CharacterController>();

    public bool InterruptsOnMove => true;
    public bool CanInteract() => !isMoving && !isArrived;

    void Start()
    {
        interactionUI = GetComponent<InteractionUIController>();

        startPos = transform.position;
        endPos = startPos + endOffset;

        lastPosition = transform.position;

        HideInteractionPrompt();
    }

    void Update()
    {
        // 플랫폼 이동 계산
        deltaMovement = transform.position - lastPosition;
        lastPosition = transform.position;

        if (isMoving)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / moveTime;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (t >= 1f)
            {
                Arrive();
            }

            // 플랫폼 위에 있는 플레이어 이동시키기
            MovePassengers(deltaMovement);
        }
    }

    private void MovePassengers(Vector3 delta)
    {
        foreach (var cc in passengers)
        {
            if (cc != null)
            {
                cc.Move(delta); // 플레이어를 플랫폼 이동량만큼 밀어줌
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("탑승!!");
            var cc = other.GetComponent<CharacterController>();
            if (cc != null && !passengers.Contains(cc))
                passengers.Add(cc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("내림!!");
            var cc = other.GetComponent<CharacterController>();
            if (cc != null)
                passengers.Remove(cc);
        }
    }

    public void Interact()
    {
        isMoving = true;
        currentTime = 0f;

        HideInteractionPrompt();
        ShowInteractionTimerUI();

        Debug.Log("엘리베이터 이동 시작!");
    }

    private void Arrive()
    {
        isMoving = false;
        isArrived = true;

        HideInteractionTimerUI();
        Debug.Log("엘리베이터 도착!");
    }

    public bool TryStartInteraction(float deltaTime)
    {
        if (!isMoving && !isArrived)
        {
            Interact();
            return true;
        }
        return false;
    }

    public void ResetInteraction() { }

    public void ShowInteractionPrompt()
    {
        if (!isArrived)
            interactionUI.ShowInteractionPrompt();
    }

    public void HideInteractionPrompt()
    {
        interactionUI.HideInteractionPrompt();
    }

    public void ShowInteractionTimerUI()
    {
        interactionUI.ShowInteractionTimerUI();
    }

    public void HideInteractionTimerUI()
    {
        interactionUI.HideInteractionTimerUI();
    }
}