using UnityEngine;

public class MenuPlaneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] planePrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnPlane), 0f, spawnInterval);
    }

    void SpawnPlane()
    {
        if (spawnPoints.Length > 0 && planePrefabs.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject planePrefab = planePrefabs[Random.Range(0, planePrefabs.Length)];
            Instantiate(planePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
