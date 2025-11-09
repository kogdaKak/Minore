using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerAiming))]
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public float fireCooldown = 0.25f;

    private IPlayerInput input;
    private PlayerAiming aiming;
    private float nextFireTime;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    public int poolSize = 20;

    public void Initialize(IPlayerInput playerInput)
    {
        input = playerInput;
    }

    private void Awake()
    {
        aiming = GetComponent<PlayerAiming>();

        // Создаём пул пуль
        for (int i = 0; i < poolSize; i++)
        {
            var b = Instantiate(bulletPrefab);
            b.SetActive(false);
            bulletPool.Enqueue(b);
        }
    }

    public void Tick()
    {
        if (input.FirePressed && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireCooldown;
        }
    }


    private void Fire()
    {
        if (bulletPool.Count == 0) return;

        var bullet = bulletPool.Dequeue();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = aiming.AimDirection * bulletSpeed;

        StartCoroutine(DeactivateAfterTime(bullet, 3f));
        bulletPool.Enqueue(bullet);
    }

    private System.Collections.IEnumerator DeactivateAfterTime(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        if (obj != null)
            obj.SetActive(false);
    }
}
