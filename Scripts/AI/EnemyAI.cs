using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

=
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Dane wroga")]
    public Enemy enemyData;

    [Header("Nawigacja")]
    [Tooltip("Co ile sekund odświeżać ścieżkę do gracza")]
    public float pathUpdateInterval = 0.2f;

    [Tooltip("Promień szukania NavMesh przy spawnowaniu (jeśli pozycja jest poza siatką)")]
    public float navMeshSnapRadius = 5f;

    
    [HideInInspector] public ObjectPool<GameObject> ObjectPool;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private float pathTimer;
    private float currentHealth;
    private float dmgAmount;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void Initialize(Vector3 spawnPosition)
    {
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogWarning($"[EnemyAI] Nie znaleziono obiektu z tagiem 'Player'! ({gameObject.name})");

       
        if (enemyData != null)
        {
            currentHealth = enemyData.health;
            dmgAmount = enemyData.damage;

            if (agent != null)
                agent.speed = enemyData.speed;
        }
        else
        {
            Debug.LogWarning($"[EnemyAI] Brak przypisanego EnemyData na {gameObject.name}!");
        }

        PlaceOnNavMesh(spawnPosition);

        pathTimer = 0f;
    }

    private void PlaceOnNavMesh(Vector3 targetPosition)
    {
        if (agent == null) return;

       
        agent.enabled = false;
        transform.position = targetPosition;
        agent.enabled = true;

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, navMeshSnapRadius, NavMesh.AllAreas))
            {
                agent.enabled = false;
                transform.position = hit.position;
                agent.enabled = true;
            }
            else
            {
                Debug.LogWarning($"[EnemyAI] Nie można umieścić wroga na NavMesh w pobliżu {targetPosition}. " +
                                 "Upewnij się że NavMesh jest zbakowany!");
            }
        }
    }

    private void Update()
    {
        if (playerTransform == null || agent == null || !agent.isOnNavMesh)
            return;

        pathTimer -= Time.deltaTime;
        if (pathTimer <= 0f)
        {
            agent.SetDestination(playerTransform.position);
            pathTimer = pathUpdateInterval;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log($"[EnemyAI] {gameObject.name} zginął - powrót do puli");

        if (ObjectPool != null)
            ObjectPool.Release(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>()?.sendDamage(dmgAmount);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, navMeshSnapRadius);
    }
}
