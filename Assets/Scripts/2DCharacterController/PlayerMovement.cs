using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private IPlayerInput input;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;

    public void Initialize(IPlayerInput playerInput)
    {
        input = playerInput;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Tick()
    {
        HandleGroundCheck();
        HandleTimers();
        HandleFlip();
        HandleJump();
    }

    public void FixedTick()
    {
        Move();
    }


    private void HandleGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;
    }

    private void HandleTimers()
    {
        if (input.JumpPressed)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0;
        }

        if (input.JumpReleased && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(input.Move * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if (input.Move != 0)
            sprite.flipX = input.Move < 0;
    }
}
