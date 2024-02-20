using UnityEngine;
using System.Collections;

public class Stove : MonoBehaviour
{
    public GameObject uncookedPattyPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject burntPattyPrefab;

    public float defaultCookingTime = 10f; // Default cooking time for the patty
    public float defaultBurnTime = 10f; // Default burn time for the patty

    private float cookingTime;
    private float burnTime;
    [SerializeField] private bool isOccupiedWithUncookedPatty = false;
    [SerializeField] private bool isOccupiedWithCookedOrBurntPatty = false;
    private bool isCooking = false;
    private GameObject currentPattyInstance;
    private GameObject cookedPattyInstance;
    private GameObject burntPattyInstance;
    private float currentCookingTimer = 0f;
    private Coroutine burnCoroutine;

    private void Start()
    {
        cookingTime = defaultCookingTime;
        burnTime = defaultBurnTime;
    }

    private void Update()
    {
        if (isCooking)
        {
            currentCookingTimer += Time.deltaTime;

            if (currentCookingTimer >= cookingTime)
            {
                CookPatty();
            }
        }

        // Check for nearby uncooked patty
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Uncooked Patty"))
            {
                if (!isOccupiedWithUncookedPatty && !isOccupiedWithCookedOrBurntPatty)
                {
                    PlacePatty();
                }
            }
            else if (hit.collider.CompareTag("Cooked Patty") || hit.collider.CompareTag("Burnt Patty"))
            {
                isOccupiedWithCookedOrBurntPatty = true;
            }
        }

        // Check if the instantiated cooked or uncooked object is no longer in the scene
        if (transform.childCount == 0 && isOccupiedWithCookedOrBurntPatty)
        {
            Debug.Log("Reset cooked or burnt patty flag");
            isOccupiedWithCookedOrBurntPatty = false;
        }

        if (isOccupiedWithUncookedPatty && isOccupiedWithCookedOrBurntPatty)
        {
            // If there's no uncooked patty and no cooked/burnt patty, reset flags
            ResetPattyFlags();
        }
    }

    private void PlacePatty()
    {
        // Check if the stove is not occupied with an uncooked patty
        if (!isOccupiedWithUncookedPatty)
        {
            // Check if the stove is not occupied with a cooked or burnt patty
            if (!isOccupiedWithCookedOrBurntPatty)
            {
                // Start cooking the patty when an uncooked patty is placed on the stove
                currentPattyInstance = Instantiate(uncookedPattyPrefab, transform.position, Quaternion.identity);
                currentPattyInstance.transform.SetParent(transform);
                isOccupiedWithUncookedPatty = true;
                isCooking = true;
            }
            else
            {
                // If there's a cooked or burnt patty but no uncooked patty, reset flags
                ResetPattyFlags();
            }
        }
    }

    private void CookPatty()
    {
        // Instantiate cooked patty
        GameObject cookedPatty = Instantiate(cookedPattyPrefab, transform.position, Quaternion.identity);

        // Set cooked patty as child of the stove
        cookedPatty.transform.SetParent(transform);

        // Destroy uncooked patty
        Destroy(currentPattyInstance);

        // Reset cooking timer and flags
        currentCookingTimer = 0f;
        isCooking = false;

        // Reset occupancy status for uncooked patty
        isOccupiedWithUncookedPatty = false;

        // Update occupancy status for cooked or burnt patty
        isOccupiedWithCookedOrBurntPatty = true;

        // Check if the cooked patty is successfully instantiated
        if (cookedPatty != null)
        {
            StartBurnTimer(cookedPatty);
        }
    }


    private void StartBurnTimer(GameObject cookedPatty)
    {
        burnCoroutine = StartCoroutine(BurnPatty(cookedPatty));
    }

    private IEnumerator BurnPatty(GameObject cookedPatty)
    {
        yield return new WaitForSeconds(burnTime);

        if (cookedPatty != null) // Check if the cooked patty is still in the scene before instantiating the burnt patty
        {
            GameObject burntPatty = Instantiate(burntPattyPrefab, transform.position, Quaternion.identity);
            burntPatty.transform.SetParent(transform);
            Destroy(cookedPatty);
        }
    }

    private void ResetPattyFlags()
    {
        Debug.Log("Reset flags");
        isOccupiedWithUncookedPatty = false;
        isOccupiedWithCookedOrBurntPatty = false;
    }
}