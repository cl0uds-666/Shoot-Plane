using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

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
