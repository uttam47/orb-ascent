using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; // Object to spawn
    [SerializeField] private int numberOfObjects = 10; // Number of objects to spawn
    [SerializeField] private float r = 5f; // Radius of the sphere
    [SerializeField] private float rotateSpeed = 5f; // Radius of the sphere
    void Start()
    {
        SpawnObjectsOnSphere();
        objectToSpawn.SetActive(false); 
    }

    void SpawnObjectsOnSphere()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Generate a random point on the surface of a sphere using spherical coordinates
            Vector3 randomDirection = Random.onUnitSphere;

            // Position the object at the correct distance from the origin
            Vector3 spawnPosition = randomDirection.normalized * r;
            spawnPosition += transform.position;
            // Instantiate the object at the spawn position and with no rotation
            GameObject cloud = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, transform);
            cloud.SetActive(true); 
        }
    }

    private void Update()
    {
        transform.Rotate(0,rotateSpeed * Time.deltaTime,0);
    }
}