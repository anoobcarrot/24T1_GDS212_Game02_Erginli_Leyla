using UnityEngine;

public class Plate : MonoBehaviour
{
    public GameObject burgerBunPrefab;
    public GameObject burgerPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject friesPacketPrefab;
    public GameObject cookedFriesPrefab;
    public GameObject completedFriesPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Burger Bun") && transform.childCount == 0)
        {
            InstantiateAndCenter(burgerBunPrefab);
            
        }
        else if (other.CompareTag("Cooked Patty") && transform.childCount == 1 && transform.GetChild(0).CompareTag("Burger Bun"))
        {
            InstantiateAndCenter(burgerPrefab);
            Destroy(other.gameObject);
            Destroy(transform.GetChild(0).gameObject);
        }
        else if (other.CompareTag("Fries Packet") && transform.childCount == 0)
        {
            InstantiateAndCenter(friesPacketPrefab);
            
        }
        else if (other.CompareTag("Cooked Fries") && transform.childCount == 1 && transform.GetChild(0).CompareTag("Fries Packet"))
        {
            InstantiateAndCenter(completedFriesPrefab);
            Destroy(other.gameObject);
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private void InstantiateAndCenter(GameObject prefab)
    {
        // Get the center position of the collider
        Vector2 center = GetComponent<Collider2D>().bounds.center;

        // Instantiate the prefab at the center position
        Instantiate(prefab, center, Quaternion.identity, transform);
    }
}