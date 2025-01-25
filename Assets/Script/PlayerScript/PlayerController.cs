using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    

    public IdleState idle {  get; private set; }
    public MoveState move { get; private set; }
    public RunState run { get; private set; }
    public BaseAttackState baseAttack { get; private set; }
    public PlayerState playerState { get; private set; }
    void Start()
    {
        stateMachine = new PlayerStateMachine();
        move = new MoveState(stateMachine,playerData,"Move_anim", this);
        idle = new IdleState(stateMachine, playerData, "Idle_anim", this);
        run = new RunState(stateMachine, playerData, "Run_anim", this);
        baseAttack = new BaseAttackState(stateMachine, playerData, "", this);
        playerState = new PlayerState(stateMachine,playerData, "Base",this );
        stateMachine.InisiateState(idle);
       
      
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
