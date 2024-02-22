using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float initialSpawnIntervalMin = 20f;
    public float initialSpawnIntervalMax = 30f;
    public float minDistanceBetweenCustomers = 0.5f;

    private float nextSpawnTime;
    private Collider2D spawnerCollider;

    void Start()
    {
        spawnerCollider = GetComponent<Collider2D>();
        nextSpawnTime = Time.time + Random.Range(initialSpawnIntervalMin, initialSpawnIntervalMax);
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCustomer();
            CalculateNextSpawnTime();
        }
    }

    void SpawnCustomer()
    {
        Vector3 spawnPosition = FindValidSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            Instantiate(customerPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 FindValidSpawnPosition()
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(spawnerCollider.bounds.min.x, spawnerCollider.bounds.max.x),
            Random.Range(spawnerCollider.bounds.min.y, spawnerCollider.bounds.max.y),
            transform.position.z
        );

        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(randomPoint, minDistanceBetweenCustomers);

        // Check if any nearby colliders belong to customers
        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.CompareTag("Customer"))
            {
                // There's already a customer nearby, so return zero vector
                return Vector3.zero;
            }
        }

        // No customers nearby, so return the random point
        return randomPoint;
    }

    void CalculateNextSpawnTime()
    {
        float spawnInterval;
        float currentTime = Time.time;

        if (currentTime < 120f) // First 2 minutes (0-120 seconds)
        {
            Debug.Log("first interval");
            // Customers spawn between 20-30 seconds
            spawnInterval = Random.Range(20f, 30f);
        }
        else if (currentTime < 360f) // 6 minutes (121-360 seconds)
        {
            Debug.Log("second interval");
            // Customers spawn between 15-20 seconds
            spawnInterval = Random.Range(15f, 20f);
        }
        else // Rest of the scene
        {
            Debug.Log("final interval");
            // Customers spawn between 10-15 seconds
            spawnInterval = Random.Range(10f, 15f);
        }

        // Update next spawn time based on current time and calculated spawn interval
        nextSpawnTime = currentTime + spawnInterval;
    }
}





