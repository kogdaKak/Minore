using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    void Awake() => Instance = this;

    public float maxHealth = 100f;
    public float currentHealth;
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) Debug.Log("Player Dead");
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
        Debug.Log($"Level UP! Level: {level}");
    }
}
