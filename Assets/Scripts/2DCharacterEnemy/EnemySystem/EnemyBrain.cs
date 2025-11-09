using UnityEngine;
using System.Collections.Generic;

public class EnemyBrain : MonoBehaviour, IAgentUpdatable
{
    public enum State { Patrol, Attack }

    [Header("Refs")]
    public RigidbodyMovement2D movement;
    public EnvironmentSensor2D env;
    public EnemyAttack attack;
    public EnemyBase health;

    [Header("Targeting")]
    public float detectRange = 1.5f;
    public List<string> targetTags = new() { "Player", "Enemy" };

    protected State current = State.Patrol;

    private void OnEnable() => AIManager.Register(this);
    private void OnDisable() => AIManager.Unregister(this);

    public virtual void TickAI()
    {
        if (health != null && health.isDead) return;

        switch (current)
        {
            case State.Patrol: TickPatrol(); break;
            case State.Attack: TickAttack(); break;
        }
    }

    protected virtual void TickPatrol()
    {
        if (DetectTargets())
        {
            movement.StopHorizontal();
            current = State.Attack;
            return;
        }

        bool ok = env.CanStepForward(movement.FacingRight);
        if (!ok)
        {
            movement.StopHorizontal();
            movement.Flip();
            return;
        }

        movement.SetDirection(movement.FacingRight ? 1 : -1);
    }

    protected virtual void TickAttack()
    {
        attack.TryAttack(targetTags);
        if (!DetectTargets())
            current = State.Patrol;
        else
            movement.StopHorizontal();
    }

    protected bool DetectTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange);
        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;
            if (targetTags.Contains(h.tag)) return true;
        }
        return false;
    }
}
