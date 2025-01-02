using UnityEngine;

public class PlaneSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PlaneType
    {
        public GameObject planePrefab; // Prefab for this plane type
        public float spawnWeight = 1f; // Relative probability of spawning this type
        public bool useSpecificSpawnPoint = false; // Whether this type has a dedicated spawn point
        public Transform specificSpawnPoint; // The dedicated spawn point for this type
    }

    [SerializeField] private PlaneType[] planeTypes; // Array of different plane types
    [SerializeField] private Transform[] generalSpawnPoints; // Array of general spawn points
    [SerializeField] private float spawnInterval = 5f; // Time between spawns
    [SerializeField] private int maxPlanes = 10; // Maximum number of planes allowed at one time

    private int currentPlaneCount = 0;

    void Start()
    {
        // Start spawning planes at regular intervals
        InvokeRepeating(nameof(SpawnPlane), 0f, spawnInterval);
    }

    void SpawnPlane()
    {
        // Check if the maximum number of planes has been reached
        if (currentPlaneCount >= maxPlanes) return;

        if (generalSpawnPoints.Length > 0 && planeTypes.Length > 0)
        {
            // Choose a plane type based on weighted probability
            PlaneType chosenType = ChoosePlaneType();

            // Determine the spawn point
            Transform spawnPoint;
            if (chosenType.useSpecificSpawnPoint && chosenType.specificSpawnPoint != null)
            {
                // Use the dedicated spawn point for this plane type
                spawnPoint = chosenType.specificSpawnPoint;
            }
            else
            {
                // Use a random general spawn point
                spawnPoint = generalSpawnPoints[Random.Range(0, generalSpawnPoints.Length)];
            }

            // Instantiate the plane at the chosen spawn point
            Instantiate(chosenType.planePrefab, spawnPoint.position, spawnPoint.rotation);

            // Increment the current plane count
            currentPlaneCount++;
        }
    }

    public void PlaneDestroyed()
    {
        // Decrement the current plane count when a plane is destroyed
        currentPlaneCount = Mathf.Max(0, currentPlaneCount - 1);
    }

    private PlaneType ChoosePlaneType()
    {
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (PlaneType type in planeTypes)
        {
            totalWeight += type.spawnWeight;
        }

        // Choose a random value between 0 and totalWeight
        float randomValue = Random.Range(0f, totalWeight);

        // Select a plane type based on the random value
        foreach (PlaneType type in planeTypes)
        {
            if (randomValue < type.spawnWeight)
            {
                return type;
            }

            randomValue -= type.spawnWeight;
        }

        // Default to the first type if something goes wrong
        return planeTypes[0];
    }
}
