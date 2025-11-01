using UnityEngine;
using UnityEngine.InputSystem;

public class LeftJoystickKeyInput : MonoBehaviour
{
    private Joystick leftJoystick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftJoystick = GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (leftJoystick.isBeingDragged)
        {
            return;
        }
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return; 

        Vector2 keyboardInput = Vector2.zero;

        if (keyboard.wKey.isPressed)
        {
            keyboardInput.y = +1;
        }
        if (keyboard.sKey.isPressed)
        {
            keyboardInput.y = -1;
        }
        if (keyboard.aKey.isPressed)
        {
            keyboardInput.x = -1;
        }
        if (keyboard.dKey.isPressed)
        {
            keyboardInput.x = +1;
        }

        leftJoystick.Horizontal = keyboardInput.x;
        leftJoystick.Vertical = keyboardInput.y;
    }
}
