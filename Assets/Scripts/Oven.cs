using UnityEngine;
using System.Collections;

public class Oven : MonoBehaviour
{
    public GameObject uncookedPorkBellyPrefab;
    public GameObject cookedPorkBellyPrefab;
    public GameObject burntPorkBellyPrefab;
    public GameObject checkImage;
    public GameObject warningImage;

    public float defaultCookingTime = 15f; // Default cooking time for the pork belly
    public float defaultBurnTime = 10f; // Default burn time for the pork belly
    public float warningTimeThreshold = 13f; // Time threshold for displaying warning before burning
    public float checkImageDuration = 2f; // Duration for displaying the check image
    public float warningImageDuration = 0.3f;

    private float cookingTime;
    private float burnTime;
    [SerializeField] private bool isOccupiedWithUncookedPorkBelly = false;
    [SerializeField] private bool isOccupiedWithCookedOrBurntPorkBelly = false;
    private bool isCooking = false;
    private GameObject currentPorkBellyInstance;
    private GameObject cookedPorkBellyInstance;
    private GameObject burntPorkBellyInstance;
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
            if (hit.collider != null && hit.collider.CompareTag("Uncooked Pork Belly"))
            {
                // Check if the oven collider contains the uncooked pork belly collider
                if (GetComponent<Collider2D>().bounds.Intersects(hit.collider.bounds))
                {
                    // Check if the oven is not occupied with an uncooked pork belly and not cooking
                    if (!isOccupiedWithUncookedPorkBelly && !isOccupiedWithCookedOrBurntPorkBelly && !isCooking)
                    {
                        Debug.Log("Placing uncooked pork belly in the oven");
                        PlacePorkBelly();
                    }
                }
            }
        }

        if (isCooking)
        {
            currentCookingTimer += Time.deltaTime;

            if (currentCookingTimer >= cookingTime)
            {
                CookPorkBelly();
            }
        }

        // Check if the instantiated cooked or uncooked object is no longer in the scene
        if (transform.childCount == 0 && isOccupiedWithCookedOrBurntPorkBelly)
        {
            Debug.Log("Reset cooked or burnt pork belly flag");
            isOccupiedWithCookedOrBurntPorkBelly = false;
        }

        if (isOccupiedWithUncookedPorkBelly && isOccupiedWithCookedOrBurntPorkBelly)
        {
            // If there's no uncooked pork belly and no cooked/burnt pork belly, reset flags
            ResetPorkBellyFlags();
        }
    }

    private void PlacePorkBelly()
    {
        // Start cooking the pork belly when an uncooked pork belly is placed in the oven
        currentPorkBellyInstance = Instantiate(uncookedPorkBellyPrefab, transform.position, Quaternion.identity);
        currentPorkBellyInstance.transform.SetParent(transform);
        isOccupiedWithUncookedPorkBelly = true;
        isCooking = true;
    }

    private void CookPorkBelly()
    {
        // Instantiate cooked pork belly
        cookedPorkBellyInstance = Instantiate(cookedPorkBellyPrefab, transform.position, Quaternion.identity);

        // Set cooked pork belly as child of the oven
        cookedPorkBellyInstance.transform.SetParent(transform);

        // Display check image
        checkImage.SetActive(true);

        // Destroy uncooked pork belly
        Destroy(currentPorkBellyInstance);

        // Reset cooking timer and flags
        currentCookingTimer = 0f;
        isCooking = false;

        // Reset occupancy status for uncooked pork belly
        isOccupiedWithUncookedPorkBelly = false;

        // Update occupancy status for cooked or burnt pork belly
        isOccupiedWithCookedOrBurntPorkBelly = true;

        // Check if the cooked pork belly is successfully instantiated
        if (cookedPorkBellyInstance != null)
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
        burnCoroutine = StartCoroutine(BurnPorkBelly());
    }

    private IEnumerator BurnPorkBelly()
    {
        // Wait until there are 2 seconds left on the remaining burn time
        yield return new WaitForSeconds(burnTime - 2f);

        float remainingBurnTime = 2f; // Set remaining burn time to 2 seconds

        // Flash warning image for 0.1 seconds interval
        while (remainingBurnTime > 0 && isOccupiedWithCookedOrBurntPorkBelly) // Check if still cooking
        {
            warningImage.SetActive(!warningImage.activeSelf);
            yield return new WaitForSeconds(0.1f);
            remainingBurnTime -= 0.1f; // Decrement remaining time
        }

        // Disable warning image after the specified duration
        yield return new WaitForSeconds(warningImageDuration);
        warningImage.SetActive(false);

        // Check if still cooking before proceeding
        if (isOccupiedWithCookedOrBurntPorkBelly)
        {
            // Instantiate burnt pork belly
            burntPorkBellyInstance = Instantiate(burntPorkBellyPrefab, transform.position, Quaternion.identity);
            burntPorkBellyInstance.transform.SetParent(transform);

            // Destroy cooked pork belly
            Destroy(cookedPorkBellyInstance);

            // Disable warning image (just to ensure it's off)
            warningImage.SetActive(false);
        }
    }

    private void ResetPorkBellyFlags()
    {
        Debug.Log("Reset flags");
        isOccupiedWithUncookedPorkBelly = false;
        isOccupiedWithCookedOrBurntPorkBelly = false;

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


