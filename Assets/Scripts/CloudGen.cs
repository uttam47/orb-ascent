using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{

    public class CloudGen : MonoBehaviour
    {
        [SerializeField] private GameObject spherePrefab; // Prefab of the sphere to spawn
        [SerializeField] private int minSpheres = 30;   // Minimum number of spheres to spawn
        [SerializeField] private int maxSpheres = 50;   // Maximum number of spheres to spawn
        [SerializeField] private float minScale = 0.5f;   // Minimum scale of the spheres
        [SerializeField] private float maxScale = 2f;     // Maximum scale of the spheres
        [SerializeField] private float xSeparationFactor = 1.5f; // Factor to increase separation in X direction

        void Start()
        {
            SpawnCloud();
        }

        void SpawnCloud()
        {
            // Select a random number of spheres to spawn within the specified range
            int totalSpheres = Random.Range(minSpheres, maxSpheres);

            for (int i = 0; i < totalSpheres; i++)
            {
                // Random position within a unit circle (XY plane only)
                Vector2 randomPosition2D = Random.insideUnitCircle;

                // Apply the separation factor to the X component to increase horizontal spacing
                randomPosition2D.x *= xSeparationFactor;

                // Convert 2D position to 3D, keeping Z as 0
                Vector3 randomPosition = new Vector3(randomPosition2D.x, randomPosition2D.y, 0f);

                // Adjust the scale based on the total number of spheres
                float scaleFactor = 1f - (float)(totalSpheres - minSpheres) / (maxSpheres - minSpheres);
                float randomScale = Mathf.Lerp(minScale, maxScale, scaleFactor);

                // Apply some randomization to the scale to avoid uniformity
                randomScale *= Random.Range(0.8f, 1.2f);

                // Adjust position to ensure spheres are packed closely together
                Vector3 spawnPosition = transform.position + randomPosition * randomScale;

                // Instantiate the sphere
                GameObject sphere = Instantiate(spherePrefab, spawnPosition, Quaternion.identity);
                sphere.name = "CloudFragment_" + i; 
                sphere.SetActive(true);

                // Apply the calculated scale
                sphere.transform.localScale = Vector3.one * randomScale;

                // Set the parent to keep the hierarchy clean
                sphere.transform.parent = transform;
            }
        }
    }

}