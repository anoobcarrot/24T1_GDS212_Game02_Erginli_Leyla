using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CustomerController : MonoBehaviour
{
    public float waitTime = 30f;
    public GameObject thinkBubblePrefab;
    public GameObject[] foodItems;
    public GameObject[] drinks; // Add a drinks array
    private MoneyUI moneyUI;
    private StarRatingUI starRatingUI;
    private Collider2D customerCollider;

    private float startTime;
    private float totalStarRating;
    private int totalCustomers;

    public GameObject gameOverUI;

    [SerializeField] private GameObject[] thinkBubbleItems;

    void Start()
    {
        startTime = Time.time;
        Invoke("SpawnThinkBubble", 0f);

        // Find and assign MoneyUI and StarRatingUI references during runtime
        moneyUI = FindObjectOfType<MoneyUI>();
        starRatingUI = FindObjectOfType<StarRatingUI>();

        // Set initial star rating
        starRatingUI.UpdateStarRating(3);

        // Get the collider component of the customer
        customerCollider = GetComponent<Collider2D>();
    }

    void SpawnThinkBubble()
    {
        // Calculate the position above the customer's head
        Vector3 thinkBubblePosition = transform.position + Vector3.up * 1.3f;

        // Instantiate the think bubble as a child of the customer
        GameObject thinkBubble = Instantiate(thinkBubblePrefab, thinkBubblePosition, Quaternion.identity);
        thinkBubble.transform.SetParent(transform);

        // Fill the think bubble with food items and drinks
        FillThinkBubble(thinkBubble);
    }

    void FillThinkBubble(GameObject thinkBubble)
    {
        int numFoodItems = Random.Range(1, 2); // Choose between 1 to 2 food items
        int numDrinks = Random.Range(0, 2); // Choose between 0 to 1 drinks

        int numItems = numFoodItems + numDrinks; // Total number of items

        // Keep track of instantiated items in the think bubble
        thinkBubbleItems = new GameObject[numItems];

        for (int i = 0; i < numFoodItems; i++)
        {
            GameObject randomFood = foodItems[Random.Range(0, foodItems.Length)];
            Vector3 position = GetNonOverlappingPosition(thinkBubble.transform.position, i, numItems);
            GameObject foodItem = Instantiate(randomFood, position, Quaternion.identity, thinkBubble.transform);
            thinkBubbleItems[i] = foodItem;
        }

        for (int i = numFoodItems; i < numItems; i++)
        {
            GameObject randomDrink = drinks[Random.Range(0, drinks.Length)];
            Vector3 position = GetNonOverlappingPosition(thinkBubble.transform.position, i, numItems);
            GameObject drink = Instantiate(randomDrink, position, Quaternion.identity, thinkBubble.transform);
            thinkBubbleItems[i] = drink;
        }

        // Schedule the customer to leave and pay after waitTime
        Invoke("LeaveWithoutPaying", waitTime);
    }

    Vector3 GetNonOverlappingPosition(Vector3 center, int index, int totalItems)
    {
        // Define the radius of the think bubble
        float bubbleRadius = 0.1f;

        // Calculate the angle step between items based on the number of total items
        float angleStep = 360f / totalItems;

        // Calculate the angle for the current item
        float currentAngle = index * angleStep;

        // Calculate the position based on the index and angle
        Vector3 offset = Quaternion.Euler(0, 0, currentAngle) * Vector3.up * bubbleRadius;

        // Adjust the position to ensure items are evenly distributed within the think bubble
        Vector3 position = center + offset;

        return position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (GameObject thinkBubbleItem in thinkBubbleItems)
        {
            if (other.CompareTag(thinkBubbleItem.tag))
            {
                Debug.Log("Correct food item given!");

                Destroy(other.gameObject); // Destroy the food item
                // Destroy the food item in the think bubble
                Destroy(thinkBubbleItem);

                // Remove the matched item from thinkBubbleItems array
                thinkBubbleItems = thinkBubbleItems.Where(item => item != thinkBubbleItem).ToArray();

                if (thinkBubbleItems.Length == 0)
                {
                    LeaveAndPay();
                }
                return;
            }
        }

        Debug.Log("Incorrect food item given!");
    }

    void LeaveAndPay()
    {
        float elapsedTime = Time.time - startTime;

        // Calculate star rating based on the given elapsed time
        int starRating = CalculateStarRating(elapsedTime);

        if (elapsedTime < waitTime)
        {
            Debug.Log("Update money and star rating");
            int moneyEarned = CalculateMoneyEarned(elapsedTime);

            // Update money UI
            moneyUI.UpdateMoney(moneyEarned);
        }

        // Update star rating UI with the calculated star rating
        starRatingUI.UpdateStarRating(starRating);

        // Destroy the customer object after processing
        Destroy(gameObject);

    }

    void LeaveWithoutPaying()
    {
        Debug.Log("Customer didn't receive food, didn't pay");

        // Assign -1 star rating directly to the existing starRating field
        starRatingUI.UpdateStarRating(-1);

        // Destroy the customer object after processing
        Destroy(gameObject);
        }


    int CalculateStarRating(float elapsedTime)
    {
        int rating = 3; // Initialize rating variable

        // Calculate additional stars based on elapsed time
        if (elapsedTime <= 10f) rating += 6;
        else if (elapsedTime <= 15f) rating += 4;
        else if (elapsedTime <= 20f) rating += 2;
        else if (elapsedTime <= 25f) rating += 1;
        else rating += 1;

        // Update star rating UI with the calculated star rating
        starRatingUI.UpdateStarRating(rating);

        return rating;
    }


    int CalculateMoneyEarned(float elapsedTime)
    {
        if (elapsedTime <= 10f) return 35;
        else if (elapsedTime <= 15f) return 25;
        else if (elapsedTime <= 20f) return 15;
        else if (elapsedTime <= 25f) return 5;
        else return 0;
    }
}











