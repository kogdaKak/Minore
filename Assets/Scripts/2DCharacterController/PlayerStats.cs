using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public static PlayerStats Instance;

    public float maxHealth = 100f;
    public float currentHealth;
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    void Awake() => Instance = this;
    void Start() => currentHealth = maxHealth;

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }


    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        Debug.Log($"Level UP! Level: {level}");
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        // Тут можно вызывать событие, вырубать управление, включать UI и т.д.
    }
}
