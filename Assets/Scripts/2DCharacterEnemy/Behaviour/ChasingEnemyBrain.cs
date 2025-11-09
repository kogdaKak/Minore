using UnityEngine;

public class ChasingEnemyBrain : EnemyBrain
{
    private enum ExtraState { Patrol, Chase, Attack }
    private ExtraState state = ExtraState.Patrol;

    private Transform target;
    public float chaseRange = 5f;

    public override void TickAI()
    {
        if (health != null && health.isDead) return;

        switch (state)
        {
            case ExtraState.Patrol:
                TickChasePatrol();
                break;
            case ExtraState.Chase:
                TickChase();
                break;
            case ExtraState.Attack:
                TickAttack();
                break;
        }
    }

    private void TickChasePatrol()
    {
        // ищем цель
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, chaseRange);
        foreach (var h in hits)
        {
            if (targetTags.Contains(h.tag) && h.gameObject != gameObject)
            {
                target = h.transform;
                state = ExtraState.Chase;
                return;
            }
        }

        // обычный патруль, если цели нет
        base.TickAI();
    }

    private void TickChase()
    {
        if (target == null)
        {
            state = ExtraState.Patrol;
            return;
        }

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist <= detectRange)
        {
            // цель в радиусе атаки
            state = ExtraState.Attack;
            return;
        }
        else if (dist > chaseRange)
        {
            // цель убежала Ч возвращаемс€ к патрулю
            target = null;
            state = ExtraState.Patrol;
            return;
        }

        // идЄм к цели
        int dir = target.position.x > transform.position.x ? 1 : -1;

        // заставл€ем движение
        movement.SetDirection(dir);

        // если направление помен€лось Ч флип
        bool wantRight = dir > 0;
        if (movement.FacingRight != wantRight)
            movement.Flip();

    }

    protected override void TickAttack()
    {
        if (target == null)
        {
            state = ExtraState.Patrol;
            return;
        }

        float dist = Vector2.Distance(transform.position, target.position);
        if (dist > detectRange)
        {
            state = ExtraState.Chase;
            return;
        }

        // вызываем базовую атаку
        attack.TryAttack(targetTags);
        if (!DetectTargets())
            state = ExtraState.Chase;
        else
            movement.StopHorizontal();
    }
}
