using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInputManger : MonoBehaviour
{
    [SerializeField] private float bufferTime = 0.2f;
    private Vector3 rawInput;
    public float normInputX;
    public float normInputY;
    public bool isSprinting;
    public bool IsAttacking;
    public bool ActionPefromed;
    private bool CanPefromKeyBuffer;
    [SerializeField]private Dictionary<string, KeyBuffer> BufferList = new Dictionary<string, KeyBuffer>();
    [SerializeField] private PlayerWeaponAgent agent;
    [SerializeField] public Vector2 moveDir;

    public void Start()
    {
        agent = FindAnyObjectByType<PlayerWeaponAgent>();
        agent.OnExit += () => CheckBufferedInput("Fire");
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();

        normInputX = (int)(rawInput * Vector2.right).normalized.x;
        normInputY = (int)(rawInput * Vector2.up).normalized.y;
        moveDir = new Vector2(normInputX, normInputY).normalized;

    }

    public void FixedUpdate()
    {
        if (!ActionPefromed && CanPefromKeyBuffer)
        {
            CanPefromKeyBuffer = false;
        } 
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
        if (context.canceled)
        {
            CanPefromKeyBuffer = true;
        }
    }

    public void OnKeyBuffer(InputAction.CallbackContext context) 
    {
        if (ActionPefromed && CanPefromKeyBuffer)
        {
            KeyBuffer buffer = new KeyBuffer();
            buffer.TimePressed = Time.time;
            BufferList[context.action.name] = buffer;
            Debug.Log(context.action.name);

        }

    }

    public void CheckBufferedInput(string actionName)
    {
        if (BufferList.ContainsKey(actionName))
        {
 
            float timePressed = BufferList[actionName].TimePressed;

            if (Time.time <= timePressed + bufferTime)
            {
              
                BufferList.Remove(actionName); // Optionally consume it
                StartCoroutine(PeformAttack());
             
            }
            else
            {
                BufferList.Remove(actionName); // Expired
            }
        }

     
    }




    public IEnumerator PeformAttack()
    {

        yield return new WaitForSeconds(0.1f);
        IsAttacking = true;
        ActionPefromed = true;
  
    }

}

public struct KeyBuffer
{
    public float TimePressed;
}
