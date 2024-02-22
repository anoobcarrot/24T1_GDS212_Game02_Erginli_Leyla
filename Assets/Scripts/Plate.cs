using UnityEngine;

public class Plate : MonoBehaviour
{
    public GameObject burgerBunPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject burgerPrefab;
    public GameObject friesPacketPrefab;
    public GameObject cookedFriesPrefab;
    public GameObject completedFriesPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Cast a ray from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check if the ray hits an object
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name); // Debug the name of the hit object
                Debug.Log("Hit object tag: " + hit.collider.gameObject.tag); // Debug the tag of the hit object

                if (GetComponent<Collider2D>().bounds.Intersects(hit.collider.bounds))
                    // Check if the hit object has a "Burger Bun" tag
                    if (transform.childCount == 0)
                {
                    if (hit.collider.CompareTag("Burger Bun"))
                    {
                        InstantiateAndCenter(burgerBunPrefab);
                    }
                    else if (hit.collider.CompareTag("Fries Packet"))
                    {
                        InstantiateAndCenter(friesPacketPrefab);
                    }
                }
                else
                {
                    // Check if the plate contains a burger bun
                    if (transform.GetChild(0).CompareTag("Burger Bun"))
                    {
                        if (hit.collider.CompareTag("Cooked Patty"))
                        {
                            // Turn burger bun into burger
                            InstantiateAndCenter(burgerPrefab);
                            Destroy(transform.GetChild(0).gameObject);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    // Check if the plate contains a fries packet
                    else if (transform.GetChild(0).CompareTag("Fries Packet"))
                    {
                        if (hit.collider.CompareTag("Cooked Fries"))
                        {
                            // Turn fries packet into completed fries
                            InstantiateAndCenter(completedFriesPrefab);
                            Destroy(transform.GetChild(0).gameObject);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void InstantiateAndCenter(GameObject prefab)
    {
        // Get the center position of the plate collider
        Vector2 center = GetComponent<Collider2D>().bounds.center;

        // Instantiate the prefab at the center position
        Instantiate(prefab, center, Quaternion.identity, transform);
    }
}

