using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RigidbodyMovement2D : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool FacingRight { get; private set; } = true;

    private Rigidbody2D rb;
    private float desiredDirX = 0f; // -1..0..1

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public void SetDirection(int dir) // -1 или 1
    {
        desiredDirX = Mathf.Clamp(dir, -1, 1);
    }

    public void StopHorizontal()
    {
        desiredDirX = 0f;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void Flip()
    {
        FacingRight = !FacingRight;
        var s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    void FixedUpdate()
    {
        float vx = desiredDirX * moveSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }
}
