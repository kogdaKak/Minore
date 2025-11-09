using UnityEngine;
using System;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public int xpReward = 20;
    public bool isDead { get; private set; }

    public event Action<EnemyBase> OnEnemyDied;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
            currentHealth -= amount;
        if (currentHealth <= 0) 
            Die();
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        PlayerStats.Instance.AddXP(xpReward);
        // событие для спавнера
        OnEnemyDied?.Invoke(this);

        // эффект смерти
        gameObject.SetActive(false);
    }
}
