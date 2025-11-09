using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private Transform firePoint; // ссылка на точку выстрела
    private Vector3 initialLocalPos;

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (firePoint != null)
            initialLocalPos = firePoint.localPosition;
    }

    public void Initialize()
    {
        sprite = GetComponent<SpriteRenderer>();
        if (firePoint != null)
            initialLocalPos = firePoint.localPosition;
    }

    public void Tick()
    {
        AimDirection = sprite.flipX ? Vector2.left : Vector2.right;

        if (firePoint != null)
        {
            var pos = initialLocalPos;
            pos.x = sprite.flipX ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
            firePoint.localPosition = pos;
        }
    }

}
