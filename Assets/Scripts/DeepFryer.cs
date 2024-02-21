using UnityEngine;
using System.Collections;

public class DeepFryer : MonoBehaviour
{
    public GameObject uncookedFriesPrefab;
    public GameObject cookedFriesPrefab;
    public GameObject burntFriesPrefab;
    public GameObject checkImage;
    public GameObject warningImage;

    public float defaultCookingTime = 15f; // Default cooking time for the fries
    public float defaultBurnTime = 10f; // Default burn time for the fries
    public float warningTimeThreshold = 13f; // Time threshold for displaying warning before burning
    public float checkImageDuration = 2f; // Duration for displaying the check image
    public float warningImageDuration = 0.3f;

    private float cookingTime;
    private float burnTime;
    [SerializeField] private bool isOccupiedWithUncookedFries = false;
    [SerializeField] private bool isOccupiedWithCookedOrBurntFries = false;
    private bool isCooking = false;
    private GameObject currentFriesInstance;
    private GameObject cookedFriesInstance;
    private GameObject burntFriesInstance;
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
        if (isCooking)
        {
            currentCookingTimer += Time.deltaTime;

            if (currentCookingTimer >= cookingTime)
            {
                CookFries();
            }
        }

        // Check for nearby uncooked fries
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Uncooked Fries"))
            {
                if (!isOccupiedWithUncookedFries && !isOccupiedWithCookedOrBurntFries)
                {
                    PlaceFries();
                }
            }
            else if (hit.collider.CompareTag("Cooked Fries") || hit.collider.CompareTag("Burnt Fries"))
            {
                isOccupiedWithCookedOrBurntFries = true;
            }
        }

        // Check if the instantiated cooked or uncooked object is no longer in the scene
        if (transform.childCount == 0 && isOccupiedWithCookedOrBurntFries)
        {
            Debug.Log("Reset cooked or burnt fries flag");
            isOccupiedWithCookedOrBurntFries = false;
        }

        if (isOccupiedWithUncookedFries && isOccupiedWithCookedOrBurntFries)
        {
            // If there's no uncooked fries and no cooked/burnt fries, reset flags
            ResetFriesFlags();
        }
    }

    private void PlaceFries()
    {
        // Check if the deep fryer is not occupied with uncooked fries
        if (!isOccupiedWithUncookedFries)
        {
            // Check if the deep fryer is not occupied with cooked or burnt fries
            if (!isOccupiedWithCookedOrBurntFries)
            {
                // Start cooking the fries when uncooked fries are placed in the deep fryer
                currentFriesInstance = Instantiate(uncookedFriesPrefab, transform.position, Quaternion.identity);
                currentFriesInstance.transform.SetParent(transform);
                isOccupiedWithUncookedFries = true;
                isCooking = true;
            }
            else
            {
                // If there are cooked or burnt fries but no uncooked fries, reset flags
                ResetFriesFlags();
            }
        }
    }

    private void CookFries()
    {
        // Instantiate cooked fries
        cookedFriesInstance = Instantiate(cookedFriesPrefab, transform.position, Quaternion.identity);

        // Set cooked fries as child of the deep fryer
        cookedFriesInstance.transform.SetParent(transform);

        // Display check image
        checkImage.SetActive(true);

        // Destroy uncooked fries
        Destroy(currentFriesInstance);

        // Reset cooking timer and flags
        currentCookingTimer = 0f;
        isCooking = false;

        // Reset occupancy status for uncooked fries
        isOccupiedWithUncookedFries = false;

        // Update occupancy status for cooked or burnt fries
        isOccupiedWithCookedOrBurntFries = true;

        // Check if the cooked fries are successfully instantiated
        if (cookedFriesInstance != null)
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
        burnCoroutine = StartCoroutine(BurnFries());
    }

    private IEnumerator BurnFries()
    {
        // Wait until there are 2 seconds left on the remaining burn time
        yield return new WaitForSeconds(burnTime - 2f);

        float remainingBurnTime = 2f; // Set remaining burn time to 2 seconds

        // Flash warning image for 0.1 seconds interval
        while (remainingBurnTime > 0 && isOccupiedWithCookedOrBurntFries) // Check if still cooking
        {
            warningImage.SetActive(!warningImage.activeSelf);
            yield return new WaitForSeconds(0.1f);
            remainingBurnTime -= 0.1f; // Decrement remaining time
        }

        // Disable warning image after the specified duration
        yield return new WaitForSeconds(warningImageDuration);
        warningImage.SetActive(false);

        // Check if still cooking before proceeding
        if (isOccupiedWithCookedOrBurntFries)
        {
            // Instantiate burnt fries
            burntFriesInstance = Instantiate(burntFriesPrefab, transform.position, Quaternion.identity);
            burntFriesInstance.transform.SetParent(transform);

            // Destroy cooked fries
            Destroy(cookedFriesInstance);

            // Disable warning image (just to ensure it's off)
            warningImage.SetActive(false);
        }
    }

    private void ResetFriesFlags()
    {
        Debug.Log("Reset flags");
        isOccupiedWithUncookedFries = false;
        isOccupiedWithCookedOrBurntFries = false;

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

