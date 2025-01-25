using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManger : MonoBehaviour
{

    private Vector3 rawInput;
    public float normInputX;
    public float normInputY;
    public bool isSprinting;
    public bool IsAttacking;
    public bool ActionPefromed;


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();

        normInputX = (int)(rawInput * Vector2.right).normalized.x;
        normInputY = (int)(rawInput * Vector2.up).normalized.y;

    }

    public void OnSprintInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(isSprinting)
            {
                isSprinting = false;
            }
            else
            {
                isSprinting = true;
            }

        }

    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed && !IsAttacking)
        {
            ActionPefromed = true;
            IsAttacking = true;
           
        }
    }

}
