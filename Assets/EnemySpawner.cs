using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float timeBetweenSpawns = 3f;
    public int maxEnemiesAlive = 20;
    
    public float spawnCheckRadius = 0.6f;
    
    public LayerMask occupancyMask;
    
    public int enemiesPerDifficultyStep = 7;
    
    public int healthIncreasePerStep = 1;
    
    public float moveSpeedIncreasePerStep = 0.2f;

    private float nextSpawnTime;
    private int totalEnemiesSpawned = 0;
    public Transform player;

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }
    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        int alive = FindObjectsByType<EnemyAI>(0, 0).Length;
        if (alive >= maxEnemiesAlive)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns;
            return;
        }

        Transform spawnPoint = GetFreeSpawnPoint();
        if (spawnPoint == null)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns * 0.5f;
            return;
        }
        
        int difficultyStep = totalEnemiesSpawned / Mathf.Max(1, enemiesPerDifficultyStep);
        
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        totalEnemiesSpawned++;
        
        EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            float baseSpeed = enemyAI.moveSpeed;
            enemyAI.moveSpeed = baseSpeed + difficultyStep * moveSpeedIncreasePerStep;
        }
        
        Health health = enemyObj.GetComponent<Health>();
        if (health != null)
        {
            int baseHealth = health.maxHealth;
            int newMaxHealth = baseHealth + difficultyStep * healthIncreasePerStep;
            health.InitializeHealth(newMaxHealth);
        }
        
        nextSpawnTime = Time.time + timeBetweenSpawns;
    }
    
    private Transform GetFreeSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return null;
        
        if (player == null)
        {
            foreach (Transform sp in spawnPoints)
            {
                if (sp == null) continue;
                if (!IsOccupied(sp.position))
                    return sp;
            }
            return null;
        }

        Transform bestSpawn = null;
        float bestDistSqr = float.MaxValue;

        foreach (Transform sp in spawnPoints)
        {
            if (sp == null) continue;
            
            if (IsOccupied(sp.position))
                continue;

            float distSqr = (sp.position - player.position).sqrMagnitude;
            if (distSqr < bestDistSqr)
            {
                bestDistSqr = distSqr;
                bestSpawn = sp;
            }
        }

        return bestSpawn;
    }

    private bool IsOccupied(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, spawnCheckRadius, occupancyMask) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.green;
        foreach (Transform sp in spawnPoints)
        {
            if (sp == null) continue;
            Gizmos.DrawWireSphere(sp.position, spawnCheckRadius);
        }
    }
}

