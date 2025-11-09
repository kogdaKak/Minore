using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Можно добавить проверку на врага
        if (other.CompareTag("Enemy"))
        {
            // Damage logic
            // other.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
