using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab; // Prefab for this enemy type
        public float spawnWeight = 1f; // Relative probability of spawning this type
        public bool useSpecificSpawnPoint = false; // Whether this type has a dedicated spawn point
        public Transform specificSpawnPoint; // The dedicated spawn point for this type
    }

    [SerializeField] private EnemyType[] enemyTypes; // Array of different enemy types
    [SerializeField] private Transform[] generalSpawnPoints; // Array of general spawn points
    [SerializeField] private float waveInterval = 10f; // Time between waves
    [SerializeField] private int initialEnemiesPerWave = 3; // Initial number of enemies per wave
    [SerializeField] private int waveIncrement = 2; // Number of enemies to add per wave
    [SerializeField] private float spawnInterval = 1f; // Time between individual spawns within a wave
    [SerializeField] private int maxEnemies = 20; // Maximum number of enemies allowed at one time

    private int currentWave = 0;
    private int currentEnemyCount = 0;

    void Start()
    {
        // Start spawning waves
        InvokeRepeating(nameof(SpawnWave), waveInterval, waveInterval);
    }

    void SpawnWave()
    {
        if (currentEnemyCount >= maxEnemies) return;

        currentWave++;
        Debug.Log($"Starting Wave {currentWave}!");

        int enemiesToSpawn = initialEnemiesPerWave + (currentWave - 1) * waveIncrement;
        StartCoroutine(SpawnEnemiesGradually(enemiesToSpawn));
    }

    System.Collections.IEnumerator SpawnEnemiesGradually(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (currentEnemyCount >= maxEnemies) break;

            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyTypes.Length == 0 || generalSpawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemies or spawn points defined!");
            return;
        }

        // Choose an enemy type based on weighted probability
        EnemyType chosenType = ChooseEnemyType();

        // Determine the spawn point
        Transform spawnPoint;
        if (chosenType.useSpecificSpawnPoint && chosenType.specificSpawnPoint != null)
        {
            spawnPoint = chosenType.specificSpawnPoint;
        }
        else
        {
            spawnPoint = generalSpawnPoints[Random.Range(0, generalSpawnPoints.Length)];
        }

        // Spawn the enemy
        Instantiate(chosenType.enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Increment the enemy count
        currentEnemyCount++;
    }

    public void EnemyDestroyed()
    {
        // Decrement the current enemy count
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }

    private EnemyType ChooseEnemyType()
    {
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (EnemyType type in enemyTypes)
        {
            totalWeight += type.spawnWeight;
        }

        // Choose a random value within the total weight range
        float randomValue = Random.Range(0f, totalWeight);

        // Select an enemy type based on the random value
        foreach (EnemyType type in enemyTypes)
        {
            if (randomValue < type.spawnWeight)
            {
                return type;
            }
            randomValue -= type.spawnWeight;
        }

        // Default to the first type if no match (unlikely)
        return enemyTypes[0];
    }
}
