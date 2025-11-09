using UnityEngine;
using System.Collections.Generic;

public class EnemyBrain : MonoBehaviour
{
    public enum State { Patrol, Attack }

    [Header("Refs")]
    public RigidbodyMovement2D movement;
    public EnvironmentSensor2D env;
    public EnemyAttack attack;                 // твой компонент атаки
    public EnemyBase health;                   // твой компонент HP/смерть

    [Header("Targeting")]
    public float detectRange = 1.5f;
    public List<string> targetTags = new() { "Player", "Enemy" };

    public State current = State.Patrol;

    void Update()
    {
        if (health != null && health.isDead) return;

        switch (current)
        {
            case State.Patrol:
                TickPatrol();
                break;
            case State.Attack:
                TickAttack();
                break;
        }
    }

    void TickPatrol()
    {
        // 1) провер€ем цель р€дом
        if (DetectTargets())
        {
            movement.StopHorizontal();
            current = State.Attack;
            return;
        }

        // 2) решаем, можно ли шагать вперЄд (по текущему FacingRight)
        bool ok = env.CanStepForward(movement.FacingRight);

        if (!ok)
        {
            // Ќ≈ шагаем в пропасть/стену Ч разворачиваемс€ сразу
            movement.StopHorizontal();
            movement.Flip();
            return;
        }

        // 3) даЄм "интенцию" идти вперЄд
        movement.SetDirection(movement.FacingRight ? 1 : -1);
    }

    void TickAttack()
    {
        // атакуем всех в радиусе по тегам
        attack.TryAttack(targetTags);

        // если никого нет Ч обратно в патруль
        if (!DetectTargets())
        {
            current = State.Patrol;
        }
        else
        {
            // в атаке не подходим Ч это тво€ текуща€ модель.
            // захочешь преследование Ч добавим State.Chase и подводку к цели.
            movement.StopHorizontal();
        }
    }

    bool DetectTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange);
        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue; // не видим себ€
            if (targetTags.Contains(h.tag)) return true;
        }
        return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
#endif
}
