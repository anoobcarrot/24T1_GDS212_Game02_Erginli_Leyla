using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public float initialSpawnIntervalMin = 10f;
    public float initialSpawnIntervalMax = 20f;
    public float firstIntervalStart = 120f;
    public float firstIntervalMin = 240f;
    public float firstIntervalMax = 300f;
    public float secondIntervalStart = 420f;
    public float secondIntervalMin = 600f;
    public float secondIntervalMax = 660f;
    public float thirdIntervalStart = 780f;
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

        if (currentTime < firstIntervalStart)
        {
            spawnInterval = Random.Range(initialSpawnIntervalMin, initialSpawnIntervalMax);
        }
        else if (currentTime < firstIntervalMin)
        {
            spawnInterval = Random.Range(firstIntervalMin, firstIntervalMax);
        }
        else if (currentTime < secondIntervalStart)
        {
            spawnInterval = Random.Range(firstIntervalMin, firstIntervalMax);
        }
        else if (currentTime < secondIntervalMin)
        {
            spawnInterval = Random.Range(secondIntervalMin, secondIntervalMax);
        }
        else if (currentTime < thirdIntervalStart)
        {
            spawnInterval = Random.Range(secondIntervalMin, secondIntervalMax);
        }
        else
        {
            spawnInterval = Random.Range(180f, 600f); // Between 3 to 10 seconds
        }

        nextSpawnTime = currentTime + spawnInterval;
    }
}



