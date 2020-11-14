using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 12f;
    public float runSpeed = 24f;
    public float jumpHeight = 3f;

    [Header("Physics")]
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController cc;
    private Vector3 velocity;
    private bool isRunning, isJumping;
    float speed;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Move();

        if (InputManager.jump && IsGrounded())
        {
            Jump();
        }

        ApplyGravity();
    }

    private void Move()
    {
        Vector3 move = transform.right * InputManager.hLeftAxis + transform.forward * InputManager.vLeftAxis;

        speed = isRunning ? runSpeed : walkSpeed;

        cc.Move(move.normalized * speed * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
}
