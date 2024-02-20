using UnityEngine;

public class Plate : MonoBehaviour
{
    public GameObject burgerBunPrefab;
    public GameObject burgerPrefab;

    private GameObject currentObjectOnPlate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Burger Bun") && currentObjectOnPlate == null)
        {
            currentObjectOnPlate = Instantiate(burgerBunPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentObjectOnPlate)
        {
            currentObjectOnPlate = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cooked Patty") && currentObjectOnPlate != null)
        {
            Destroy(collision.gameObject);
            Destroy(currentObjectOnPlate);
            currentObjectOnPlate = Instantiate(burgerPrefab, transform.position, Quaternion.identity);
        }
    }
}

