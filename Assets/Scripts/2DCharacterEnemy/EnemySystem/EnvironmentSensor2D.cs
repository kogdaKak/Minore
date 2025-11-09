using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnvironmentSensor2D : MonoBehaviour
{
    [Header("Probes")]
    public float toeAhead = 0.25f;        // вынос носка вперёд от края коллайдера
    public float groundProbeDown = 0.55f; // глубина луча вниз
    public float wallProbeDistance = 0.25f;
    public float wallProbeHeight = 0.5f;
    public LayerMask groundLayer;

    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    /// <summary>Проверяет, можно ли безопасно шагать вперёд (по направлению facingRight).</summary>
    public bool CanStepForward(bool facingRight)
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;

        Vector2 ext = col.bounds.extents;
        Vector2 feet = (Vector2)transform.position + Vector2.down * (ext.y - 0.02f);
        Vector2 toe = feet + dir * (ext.x + toeAhead);
        Vector2 chest = (Vector2)transform.position + Vector2.up * wallProbeHeight;

        // Есть ли стена прямо перед нами?
        bool wall = Physics2D.Raycast(chest, dir, wallProbeDistance, groundLayer);

        // Есть ли земля впереди под "носом"?
        bool groundAhead = Physics2D.Raycast(toe, Vector2.down, groundProbeDown, groundLayer);

        return !wall && groundAhead;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!TryGetComponent(out Collider2D c)) return;
        // Для визуализации в редакторе
        // NB: Направление отрисовки — предполагаем, что смотрим вправо (только для гизмо),
        // реальную логику даёт CanStepForward(facingRight)
        Vector2 ext = c.bounds.extents;
        Vector2 feet = (Vector2)transform.position + Vector2.down * (ext.y - 0.02f);
        Vector2 toeR = feet + Vector2.right * (ext.x + toeAhead);
        Vector2 chest = (Vector2)transform.position + Vector2.up * wallProbeHeight;

        Gizmos.color = Color.yellow; Gizmos.DrawLine(toeR, toeR + Vector2.down * groundProbeDown);
        Gizmos.color = Color.cyan; Gizmos.DrawLine(chest, chest + Vector2.right * wallProbeDistance);
    }
#endif
}
