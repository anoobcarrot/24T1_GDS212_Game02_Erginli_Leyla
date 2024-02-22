using UnityEngine;
using System.Collections;

public class Stove : MonoBehaviour
{
    public GameObject uncookedPattyPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject burntPattyPrefab;
    public GameObject checkImage;
    public GameObject warningImage;

    public float defaultCookingTime = 10f; // Default cooking time for the patty
    public float defaultBurnTime = 10f; // Default burn time for the patty
    public float warningTimeThreshold = 8f; // Time threshold for displaying warning before burning
    public float checkImageDuration = 2f; // Duration for displaying the check image
    public float warningImageDuration = 0.3f;

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

        // Disable check and warning images at the start
        checkImage.SetActive(false);
        warningImage.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Cast a ray from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check if the ray hits a collider with the tag "Uncooked Pork Belly"
            if (hit.collider != null && hit.collider.CompareTag("Uncooked Patty"))
            {
                // Check if the oven collider contains the uncooked pork belly collider
                if (GetComponent<Collider2D>().bounds.Intersects(hit.collider.bounds))
                {
                    // Check if the oven is not occupied with an uncooked pork belly and not cooking
                    if (!isOccupiedWithUncookedPatty && !isOccupiedWithCookedOrBurntPatty && !isCooking)
                    {
                        Debug.Log("Placing uncooked patty");
                        PlacePatty();
                    }
                }
            }
        }

        if (isCooking)
        {
            currentCookingTimer += Time.deltaTime;

            if (currentCookingTimer >= cookingTime)
            {
                CookPatty();
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
        // Start cooking the patty when an uncooked patty is placed on the stove
        currentPattyInstance = Instantiate(uncookedPattyPrefab, transform.position, Quaternion.identity);
        currentPattyInstance.transform.SetParent(transform);
        isOccupiedWithUncookedPatty = true;
        isCooking = true;
    }

    private void CookPatty()
    {
        // Instantiate cooked patty
        cookedPattyInstance = Instantiate(cookedPattyPrefab, transform.position, Quaternion.identity);

        // Set cooked patty as child of the stove
        cookedPattyInstance.transform.SetParent(transform);

        // Display check image
        checkImage.SetActive(true);

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
        if (cookedPattyInstance != null)
        {
            StartBurnTimer();
            StartCoroutine(DisableCheckImageAfterDuration());
        }
    }

    private IEnumerator DisableCheckImageAfterDuration()
    {
        yield return new WaitForSeconds(checkImageDuration);
        checkImage.SetActive(false);
    }

    private void StartBurnTimer()
    {
        burnCoroutine = StartCoroutine(BurnPatty());
    }

    private IEnumerator BurnPatty()
    {
        // Wait until there are 2 seconds left on the remaining burn time
        yield return new WaitForSeconds(burnTime - 2f);

        float remainingBurnTime = 2f; // Set remaining burn time to 2 seconds

        // Flash warning image for 0.1 seconds interval
        while (remainingBurnTime > 0 && isOccupiedWithCookedOrBurntPatty) // Check if still cooking
        {
            warningImage.SetActive(!warningImage.activeSelf);
            yield return new WaitForSeconds(0.1f);
            remainingBurnTime -= 0.1f; // Decrement remaining time
        }

        // Disable warning image after the specified duration
        yield return new WaitForSeconds(warningImageDuration);
        warningImage.SetActive(false);

        // Check if still cooking before proceeding
        if (isOccupiedWithCookedOrBurntPatty)
        {
            // Instantiate burnt patty
            burntPattyInstance = Instantiate(burntPattyPrefab, transform.position, Quaternion.identity);
            burntPattyInstance.transform.SetParent(transform);

            // Destroy cooked patty
            Destroy(cookedPattyInstance);

            // Disable warning image (just to ensure it's off)
            warningImage.SetActive(false);
        }
    }

    private void ResetPattyFlags()
    {
        Debug.Log("Reset flags");
        isOccupiedWithUncookedPatty = false;
        isOccupiedWithCookedOrBurntPatty = false;

        // Reset cooking timer
        currentCookingTimer = 0f;
        isCooking = false;

        // Stop burn coroutine if it's running
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
    }
}
