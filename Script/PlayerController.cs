using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [Header("Collision Player")]
    [SerializeField] float cirleCollisionRadius = 0f;
    [SerializeField] Transform foot;
    [SerializeField] LayerMask groundMask;
    bool isGround;
    bool faceRight = true;
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        CollisionCheck();
        FlipController();
        Run();
        AnimationController();
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue inputValue)
    {
        if(!isGround) { return; }
        if(inputValue.isPressed)
        {
            rb.velocity += new Vector2(0f, jumpForce);
        }
    }
    void Run()
    {
        Vector2 velocityPlayer = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = velocityPlayer;
    }
    void FlipController()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < transform.position.x && faceRight)
        {
            Flip();
        }
        else if(mousePos.x > transform.position.x && !faceRight)
        {
            Flip();
        }
    }
    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0 , 180, 0);

    }
    void AnimationController()
    {
        animator.SetFloat("xVelocity", rb.velocity.x);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isGround", isGround);
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foot.position, cirleCollisionRadius);
    }
    void CollisionCheck()
    {
        isGround = Physics2D.OverlapCircle(foot.position, cirleCollisionRadius, groundMask);
    }
}
