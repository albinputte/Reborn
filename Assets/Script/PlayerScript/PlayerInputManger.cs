using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManger : MonoBehaviour
{

    private Vector3 rawInput;
    public float normInputX;
    public float normInputY;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();

        normInputX = (int)(rawInput * Vector2.right).normalized.x;
        normInputY = (int)(rawInput * Vector2.up).normalized.y;

    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("hej");
        }
    }

}
