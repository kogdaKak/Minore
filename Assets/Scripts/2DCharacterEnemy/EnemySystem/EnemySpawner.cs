using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Список префабов врагов, которые может спавнить этот спавнер.")]
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Tooltip("Список точек, где могут появляться враги.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("Сколько врагов одновременно должно быть живыми.")]
    public int activeEnemyLimit = 3;

    [Tooltip("Задержка между спавном врагов (сек).")]
    public float spawnDelay = 2f;

    [Tooltip("Если включено — спавн завершится, когда все враги будут убиты.")]
    public bool stopAfterAllKilled = false;

    private readonly List<EnemyBase> activeEnemies = new();
    private bool isSpawning;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Проверяем условие "спавним ли новых"
            if (!stopAfterAllKilled)
            {
                // пока живых меньше лимита — дозапускаем новых
                while (activeEnemies.Count < activeEnemyLimit)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
            else
            {
                // если включено "остановиться после убийства всех" — спавним 1 волну
                if (activeEnemies.Count == 0)
                {
                    // если ещё никто не спавнился — создаём волну
                    SpawnWave();
                }
                else if (AllEnemiesDead())
                {
                    Debug.Log($"{name}: Все боты погибли, спавн завершён.");
                    yield break;
                }
            }

            yield return null;
        }
    }

    private void SpawnWave()
    {
        for (int i = 0; i < activeEnemyLimit; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnPoints.Count == 0) return;

        // случайный префаб и точка
        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        var point = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject enemyObj = Instantiate(prefab, point.position, Quaternion.identity);
        EnemyBase enemy = enemyObj.GetComponent<EnemyBase>();

        if (enemy == null)
        {
            Debug.LogWarning($"{prefab.name} не имеет EnemyBase!");
            Destroy(enemyObj);
            return;
        }

        // подписываемся на смерть
        enemy.OnEnemyDied += HandleEnemyDeath;
        activeEnemies.Add(enemy);
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        enemy.OnEnemyDied -= HandleEnemyDeath;
        activeEnemies.Remove(enemy);
    }

    private bool AllEnemiesDead()
    {
        return activeEnemies.All(e => e == null || e.isDead);
    }
}
