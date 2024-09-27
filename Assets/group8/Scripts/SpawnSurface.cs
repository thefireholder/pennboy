using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    // Object to spawn
    public GameObject objectToSpawn;

    // Number of objects to spawn
    public int numberOfObjects = 10;

    // Reference to the plane's mesh
    private MeshRenderer planeMeshRenderer;

    // Radius of the object being spawned
    private float objectRadius;

    void Start()
    {
        // Get the MeshRenderer component from the plane
        planeMeshRenderer = GetComponent<MeshRenderer>();

        // Calculate the radius based on the prefab's collider and scale
        objectRadius = GetObjectRadius();

        // Generate and spawn objects
        Vector3[] spawnPositions = GenerateSpawnPositions();
        SpawnObjects(spawnPositions);
    }

    float GetObjectRadius()
    {
        // Instantiate the prefab temporarily to access its collider
        GameObject tempObject = Instantiate(objectToSpawn);

        // Get the collider component from the temporary object
        Collider collider = tempObject.GetComponent<Collider>();
        float radius = 0.5f; // Default radius if no collider found

        if (collider != null)
        {
            // Calculate the effective radius based on collider size and scale
            float colliderRadius = Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.z);
            // Adjust by the largest scale factor (to accommodate all dimensions)
            float scaleFactor = Mathf.Max(tempObject.transform.localScale.x,
                                           tempObject.transform.localScale.y,
                                           tempObject.transform.localScale.z);
            radius = colliderRadius * scaleFactor; // Correct radius calculation
        }
        Debug.Log(radius);
        // Destroy the temporary object after getting the radius
        Destroy(tempObject);

        return radius;
    }

    Vector3[] GenerateSpawnPositions()
    {
        // Get the size of the plane (in world coordinates)
        Vector3 planeSize = planeMeshRenderer.bounds.size;

        // Get the local size of the plane (width = x, length = z) and factor in the plane's scale
        float effectiveWidth = planeSize.x * transform.localScale.x;
        float effectiveLength = planeSize.z * transform.localScale.z;
        Debug.Log(effectiveLength + " " + effectiveWidth);

        // Calculate the grid size based on object radius
        int gridXCount = Mathf.FloorToInt(effectiveWidth / (objectRadius * 2)); // Number of cells along the width
        int gridZCount = Mathf.FloorToInt(effectiveLength / (objectRadius * 2)); // Number of cells along the length

        // Create a list of potential positions using grid-based coordinates
        List<Vector3> potentialPositions = new List<Vector3>();

        for (int i = 0; i < gridXCount; i++)
        {
            for (int j = 0; j < gridZCount; j++)
            {
                // Convert grid coordinates to local plane coordinates
                float xPosition = -effectiveWidth / 2 + (i * objectRadius * 2) + objectRadius; // Center each object in the grid
                float zPosition = -effectiveLength / 2 + (j * objectRadius * 2) + objectRadius;

                // Add the calculated local position to the list
                potentialPositions.Add(new Vector3(xPosition, 0, zPosition));
            }
        }
        Debug.Log(gridXCount + " " + gridZCount);

        // Shuffle the positions to randomize the spawn locations
        ShuffleList(potentialPositions);

        // Select a subset of positions equal to the number of objects to spawn
        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < Mathf.Min(numberOfObjects, potentialPositions.Count); i++)
        {
            // Transform local position to world position
            Vector3 spawnPosition = transform.TransformPoint(potentialPositions[i]);
            spawnPositions.Add(spawnPosition);

            Debug.Log("Spawn Position: " + spawnPosition); // Print the position to the console
        }

        return spawnPositions.ToArray(); // Return the valid positions as an array
    }

    void SpawnObjects(Vector3[] spawnPositions)
    {
        // Instantiate each object at the generated positions
        foreach (Vector3 position in spawnPositions)
        {
            Instantiate(objectToSpawn, position, Quaternion.identity);
        }
    }

    // Method to shuffle the list for random placement
    void ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
