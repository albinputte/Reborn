using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public PlayerInputManger Input;

    public IdleState idle {  get; private set; }
    public MoveState move { get; private set; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
