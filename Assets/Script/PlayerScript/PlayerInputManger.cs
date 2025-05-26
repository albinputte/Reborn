using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class PlayerInputManger : MonoBehaviour
{
    [SerializeField] private float bufferTime = 0.2f;
    private Vector3 rawInput;
    public float normInputX;
    public float normInputY;
    public Vector2 ScrollInput;
    public int HotbarIndex;
    private int  Scrolloffset;
    public bool isSprinting;
    public bool isInteracting;
    public bool IsAttacking;
    public bool ActionPefromed;
    private bool CanPefromKeyBuffer;
    public GameObject[] Ui;
    private PlayerController controller;
    private InventoryController inventoryController;
    [SerializeField] private Dictionary<string, KeyBuffer> BufferList = new Dictionary<string, KeyBuffer>();
    [SerializeField] private PlayerWeaponAgent agent;
    [SerializeField] public Vector2 moveDir;
    [SerializeField] private LayerMask UILayer;

    public void Start()
    {
        agent = FindAnyObjectByType<PlayerWeaponAgent>();
        agent.OnExit += () => CheckBufferedInput("Fire");
        inventoryController = FindAnyObjectByType<InventoryController>();
        controller = FindAnyObjectByType<PlayerController>();
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
                //isSprinting = false;
            }
            else
            {
                //isSprinting = true;
            }
           
        }

    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (!IsPointerOverUI() && !Respawn.instance.isRespawning) { 
            if (context.performed && !IsAttacking && !ActionPefromed)
            {
                ActionPefromed = true;
                IsAttacking = true;
           
            }
        }
        if (context.canceled)
        {
            CanPefromKeyBuffer = true;
        }
    }


    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed && !Respawn.instance.isRespawning)
        {
            ActionPefromed = true;
            isInteracting = true;
           
        }
        
    }
    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed && !Respawn.instance.isRespawning)
        {
            inventoryController.InventoryInput();
          
        }
    }

    public void OnScrollInput(InputAction.CallbackContext context)
    {
        ScrollInput = context.ReadValue<Vector2>();
        if(HotbarIndex == -1)
        {
            Scrolloffset++;
            HotbarIndex = 1;
            inventoryController.ScrollInHotbar(HotbarIndex);
        }
           
        if(ScrollInput.y > 0)
        {
            Scrolloffset++;
            if (Scrolloffset == 2)
            {
                Scrolloffset = 0;
                return;
            }
            if (HotbarIndex <= 0)
                HotbarIndex = 9;
            else
                HotbarIndex--;

        }
        else if (ScrollInput.y < 0)
        {

            Scrolloffset++;
            if (Scrolloffset == 2)
            {
                Scrolloffset = 0;
                return;
            }
               
            if (HotbarIndex >= 9)
                HotbarIndex = 0;
            else
                HotbarIndex++;
        }
       
        inventoryController.ScrollInHotbar(HotbarIndex);
    }

    public void OnhotkeyHotbar(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // Get the key that was actually pressed
        string keyPressed = context.control.displayName; // or context.control.name

        int index;
        if (int.TryParse(keyPressed, out index))
        {
            HotbarIndex = index;
            inventoryController.ScrollInHotbar(index);
         
        }
        
    }

    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (Ui[0].activeSelf)
            inventoryController.InventoryInput();
        if (Ui[1].activeSelf)
        {
            Ui[1].gameObject.SetActive(false);
            controller.OnUiOpenInvoke();

        }
        if (Ui[2].activeSelf)
        {
            controller.OnUiOpenInvoke();
            Ui[2].gameObject.SetActive(false);
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

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}

public struct KeyBuffer
{
    public float TimePressed;
}
