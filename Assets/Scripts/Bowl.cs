using UnityEngine;

public class Bowl : MonoBehaviour
{
    public GameObject soySaucePrefab;
    public GameObject garlicPrefab;
    public GameObject cookedPorkBellyPrefab;
    public GameObject onionPrefab;
    public GameObject bayLeavesPrefab;
    public GameObject porkAdoboPrefab;

    private int ingredientCount = 0;
    private GameObject adoboInstance; // Reference to the instantiated adobo prefab

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Cast a ray from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check if the ray hits an object
            if (hit.collider != null)
            {

                if (GetComponent<Collider2D>().bounds.Contains(hit.collider.bounds.center))
                    switch (hit.collider.tag)
                    {
                        case "Soy Sauce":
                            if (ingredientCount == 0)
                                InstantiateAndPosition(soySaucePrefab);
                            break;
                        case "Garlic":
                            if (ingredientCount == 1)
                                InstantiateAndPosition(garlicPrefab);
                            break;
                        case "Cooked Pork Belly":
                            GameObject ovenObject = GameObject.FindGameObjectWithTag("Oven");
                            if (ingredientCount ==2)
                          
                            if (ovenObject != null)
                            {
                                Transform[] children = ovenObject.GetComponentsInChildren<Transform>();
                                foreach (Transform child in children)
                                {
                                    if (child.CompareTag("Cooked Pork Belly"))
                                    {
                                        GameObject cookedPorkBelly = child.gameObject;
                                        Destroy(cookedPorkBelly);
                                        InstantiateAndPosition(cookedPorkBellyPrefab);
                                        break;
                                    }
                                }
                            }
                            break;
                        case "Onion":
                            if (ingredientCount == 3)
                                InstantiateAndPosition(onionPrefab);
                            break;
                        case "Bay Leaves":
                            if (ingredientCount == 4)
                            {
                                InstantiateAndPosition(bayLeavesPrefab);
                                CreatePorkAdobo();
                                DestroyIngredients();
                            }
                            break;
                    }
            }
        }
    }

    private void InstantiateAndPosition(GameObject prefab)
    {
        Vector3 position = transform.position + new Vector3(0f, 0.2f * ingredientCount, 0f);
        Instantiate(prefab, position, Quaternion.identity, transform);
        ingredientCount++;
    }

    private void CreatePorkAdobo()
    {
        // Instantiate the Pork Adobo prefab at the center of the bowl collider
        Vector3 center = transform.GetComponent<Collider2D>().bounds.center;
        adoboInstance = Instantiate(porkAdoboPrefab, center, Quaternion.identity, transform);

        // Reset ingredient count
        ingredientCount = 0;
    }

    private void DestroyIngredients()
    {
        // Destroy all child objects of the bowl except for the pork adobo
        foreach (Transform child in transform)
        {
            if (child.gameObject != adoboInstance)
            {
                Destroy(child.gameObject);
            }
        }
    }
}







