using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInputManger Input;
    public PlayerData Data;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementXY(Data);
    }

    public void MovementXY(PlayerData data)
    {
        rb.velocity = new Vector2(Input.normInputX * data.MoveSpeed, Input.normInputY * data.MoveSpeed);
    }
}
