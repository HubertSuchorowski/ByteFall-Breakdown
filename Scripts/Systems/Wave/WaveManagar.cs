using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Konfiguracja Fal")]
    public List<WaveData> allWaves;
    private int currentWaveIndex = 0;
    public int CurrentWaveIndex => currentWaveIndex;
    public int TotalWaves => allWaves != null ? allWaves.Count : 0;

    [Header("Ustawienia Spawnera")]
    public int maxPoolSize = 50;
    public float spawnRadius = 50f;
    public float spawnProtection = 20f;

    [Header("NavMesh - Spawn")]
    [Tooltip("Ile razy spróbować znaleźć punkt na NavMesh przed pominięciem spawnu")]
    public int navMeshMaxAttempts = 30;
    [Tooltip("Promień w którym szukamy najbliższego punktu na NavMesh")]
    public float navMeshSampleRadius = 15f;
    [Tooltip("Wysokość od której rzucamy raycast w dół szukając podłogi")]
    public float raycastHeight = 50f;

    private Dictionary<GameObject, ObjectPool<GameObject>> enemyPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private int activeEnemiesCount = 0;

    void Start()
    {
        if (allWaves != null && allWaves.Count > 0)
            StartCoroutine(WaitForNavMeshThenSpawn());
        else
            Debug.LogWarning("[WaveManager] Nie przypisałeś żadnych fal (WaveData) do listy allWaves!");
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N))
            NextWaveAuto();
    }


    public void NextWaveAuto()
    {
        if (currentWaveIndex < allWaves.Count - 1)
        {
            currentWaveIndex++;
            Debug.Log($"[WaveManager] Fala {currentWaveIndex + 1} / {allWaves.Count}");
        }
        else
        {
            Debug.Log("[WaveManager] Osiągnięto ostatnią falę!");
        }
    }

    private IEnumerator WaitForNavMeshThenSpawn()
    {
        Debug.Log("[WaveManager] Czekam na NavMesh...");
        while (NavMeshBaker.Instance == null || !NavMeshBaker.Instance.IsReady)
            yield return new WaitForSeconds(0.2f);

        Debug.Log("[WaveManager] NavMesh gotowy - startuje spawning!");
        StartCoroutine(SpawnSystem());
    }

    private IEnumerator SpawnSystem()
    {
        while (true)
        {
            WaveData currentWaveData = allWaves[currentWaveIndex];

            if (activeEnemiesCount < maxPoolSize)
            {
                Vector3 spawnPos;
                if (TryGetNavMeshSpawnPosition(out spawnPos))
                {
                    GameObject selectedPrefab = GetRandomEnemyFromWave(currentWaveData);
                    if (selectedPrefab != null)
                    {
                        GameObject enemyObj = GetPoolForPrefab(selectedPrefab).Get();

                        EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();
                        if (enemyAI != null)
                        {
                            enemyAI.ObjectPool = enemyPools[selectedPrefab];
                            enemyAI.Initialize(spawnPos);
                        }
                        else
                        {
                            NavMeshAgent agent = enemyObj.GetComponent<NavMeshAgent>();
                            if (agent != null) agent.Warp(spawnPos);
                            else enemyObj.transform.position = spawnPos;
                            Debug.LogWarning($"[WaveManager] Prefab '{selectedPrefab.name}' nie ma komponentu EnemyAI!");
                        }
                    }
                }
            }

            yield return new WaitForSeconds(currentWaveData.spawnRate);
        }
    }

    /// <summary>
    /// Rzuca raycast w dół żeby znaleźć podłogę, potem szuka punktu na NavMesh.
    /// </summary>
    private bool TryGetNavMeshSpawnPosition(out Vector3 result)
    {
        for (int i = 0; i < navMeshMaxAttempts; i++)
        {
            Vector3 candidate = GetRandomSpawnPosition();

            Vector3 rayOrigin = new Vector3(candidate.x, candidate.y + raycastHeight, candidate.z);
            RaycastHit rayHit;
            if (Physics.Raycast(rayOrigin, Vector3.down, out rayHit, raycastHeight * 2f))
                candidate = rayHit.point;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(candidate, out navHit, navMeshSampleRadius, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        Vector3 direction = new Vector3(randomCircle.x, 0f, randomCircle.y);
        direction *= Random.Range(spawnProtection, spawnRadius);
        return transform.position + direction;
    }

    private GameObject GetRandomEnemyFromWave(WaveData data)
    {
        if (data.enemiesInWave == null || data.enemiesInWave.Count == 0) return null;

        float totalWeight = 0f;
        foreach (var config in data.enemiesInWave)
            totalWeight += config.spawnChance;

        float randomValue = Random.Range(0f, totalWeight);
        float currentSum = 0f;

        foreach (var config in data.enemiesInWave)
        {
            currentSum += config.spawnChance;
            if (randomValue <= currentSum)
                return config.enemyPrefab;
        }

        return data.enemiesInWave[0].enemyPrefab;
    }

    private ObjectPool<GameObject> GetPoolForPrefab(GameObject prefab)
    {
        if (!enemyPools.ContainsKey(prefab))
        {
            enemyPools[prefab] = new ObjectPool<GameObject>(
                createFunc:      () => Instantiate(prefab),
                actionOnGet:     (obj) => { obj.SetActive(true);  activeEnemiesCount++; },
                actionOnRelease: (obj) => { obj.SetActive(false); activeEnemiesCount--; },
                actionOnDestroy: (obj) => Destroy(obj),
                maxSize:         maxPoolSize
            );
        }
        return enemyPools[prefab];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnProtection);
    }
}
