using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerInputManger Input;
    public GameObject Parrent;
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerData playerData;
    public PlayerWeaponAgent weaponAgent;
    public LayerMask InteractionLayer;
    public IInteractable CurrentNearest;

    public InventoryController inventoryController;
    public event Action OnUiOpen; 
    public event Action OnAnimationDone, OnAnimationEvent;

    [SerializeField]
    public static Directions[] FacingDirection = new Directions[2];


    public IdleState idle {  get; private set; }
    public MoveState move { get; private set; }
    public RunState run { get; private set; }
    public BaseAttackState baseAttack { get; private set; }
    public PlayerState playerState { get; private set; }
    public InteractState interactState { get; private set; }
    public InteractBush interactBush { get; private set; }
    public InteractCrafting interactCrafting { get; private set; }
    public ConsumeState consumeState { get; private set; }
    public InteractMinning MinningState { get; private set; }
    public RebornStartState StartState { get; private set; }

    void Start()
    {
        stateMachine = new PlayerStateMachine();
        move = new MoveState(stateMachine,playerData, "PlayerRunAnim", this);
        idle = new IdleState(stateMachine, playerData, "Idle", this);
        run = new RunState(stateMachine, playerData, "PlayerRunAnim", this);
        baseAttack = new BaseAttackState(stateMachine, playerData, "LightAttack", this);
        playerState = new PlayerState(stateMachine,playerData, "Base",this );
        interactState = new InteractState(stateMachine, playerData, "InteractAnim", this);
        interactBush = new InteractBush(stateMachine, playerData, "Idle", this );
        interactCrafting = new InteractCrafting(stateMachine, playerData, "Idle", this);
        consumeState = new ConsumeState(stateMachine, playerData,"", this );
        StartState = new RebornStartState(stateMachine, playerData, "RebornStartIntro", this);
        MinningState = new InteractMinning(stateMachine, playerData, "MiningAnim", this);
        stateMachine.InisiateState(StartState);
       
     
    }

    public void OnUiOpenInvoke()
    {
        OnUiOpen?.Invoke();
    }
    public void OnAnimationTrigger()
    {
        OnAnimationDone?.Invoke();
    }
    public void OnAnimationEventTrigger()
    {
        OnAnimationEvent?.Invoke();
    }

    public void Update()
    {
        stateMachine.CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
      stateMachine.CurrentState.PhysicsUpdate();
      
    }
 





}
