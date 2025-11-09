using UnityEngine;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.2f;
    public float attackRate = 1f;
    public float damage = 10f;

    private float nextAttackTime;

    public void TryAttack(List<string> targetTags)
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + 1f / attackRate;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            // не атакуем самого себя
            if (hit.gameObject == this.gameObject)
                continue;

            if (targetTags.Contains(hit.tag) && hit.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
