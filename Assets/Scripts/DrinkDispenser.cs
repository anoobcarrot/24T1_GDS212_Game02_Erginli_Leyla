using UnityEngine;
using System.Collections;

public class DrinkDispenser : MonoBehaviour
{
    public GameObject flavorPrefab; // flavour drink prefab
    public Collider2D targetCollider; // drinkDispenser collider
    public GameObject checkImage; // Check image

    private Collider2D dispenserCollider;
    private bool isClicked = false;

    private void Start()
    {
        // get the collider component of the dispenser object
        dispenserCollider = GetComponent<Collider2D>();
        checkImage.SetActive(false); // Ensure check image is initially disabled
    }

    private void Update()
    {
        // Check for mouse click on the collider
        if (Input.GetMouseButtonDown(0) && !isClicked)
        {
            // Cast a ray from the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // Check if the ray hits the collider
            if (hit.collider == dispenserCollider)
            {
                isClicked = true;
                StartCoroutine(DispenseFlavorAfterDelay()); // Dispense flavor after 5 seconds
            }
        }
    }

    private IEnumerator DispenseFlavorAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        // Calculate the center position of the target collider
        Vector3 targetCenter = targetCollider.bounds.center;

        // Instantiate the flavor prefab as a child of the target collider's center
        Instantiate(flavorPrefab, targetCenter, Quaternion.identity, targetCollider.transform);

        // Display check image for 2 seconds
        checkImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        checkImage.SetActive(false);

        // Reset the click state to allow dispensing again
        isClicked = false;
    }
}



