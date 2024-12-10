using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    public GameObject planePrefab; // Enemy plane prefab
    public Transform[] spawnPoints; // Spawn points around the map
    public float spawnInterval = 5f; // Time between spawns

    void Start()
    {
        InvokeRepeating(nameof(SpawnPlane), 0f, spawnInterval);
    }

    void SpawnPlane()
    {
        if (spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(planePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
