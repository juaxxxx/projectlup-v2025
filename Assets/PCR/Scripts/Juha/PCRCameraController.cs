using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using TouchPhase = UnityEngine.TouchPhase;

public class PCRCameraController : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 currPos, prePos;
    private Vector3 movePos;

    private bool isPanning;

    public Camera cam;

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam == null)
        {
            Debug.LogError("Empty Camera");
        }
    }

    private void Start()
    {
        isPanning = false;
        speed = 10f;
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                currPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - currPos) * Time.deltaTime * speed;
                cam.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            prePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isPanning)
        {
            currPos = (Vector2)Input.mousePosition;
            movePos = (Vector3)(prePos - currPos) * Time.deltaTime * speed; ;
            cam.transform.Translate(movePos);
            prePos = currPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

    }
}
