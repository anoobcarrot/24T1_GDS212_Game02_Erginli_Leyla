using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public float waitTime = 30f;
    public GameObject thinkBubblePrefab;
    public GameObject[] foodItems;
    private MoneyUI moneyUI;
    private StarRatingUI starRatingUI;

    private float startTime;
    private float totalStarRating;
    private int totalCustomers;

    void Start()
    {
        startTime = Time.time;
        Invoke("SpawnThinkBubble", 0f);

        // Find and assign MoneyUI and StarRatingUI references during runtime
        moneyUI = FindObjectOfType<MoneyUI>();
        starRatingUI = FindObjectOfType<StarRatingUI>();
    }

    void SpawnThinkBubble()
    {
        // Calculate the position above the customer's head
        Vector3 thinkBubblePosition = transform.position + Vector3.up * 2f;

        // Instantiate the think bubble as a child of the customer
        GameObject thinkBubble = Instantiate(thinkBubblePrefab, thinkBubblePosition, Quaternion.identity);
        thinkBubble.transform.SetParent(transform);

        // Fill the think bubble with food items
        FillThinkBubble(thinkBubble);
    }

    void FillThinkBubble(GameObject thinkBubble)
    {
        int numItems = 1;
        float currentTime = Time.time;

        if (currentTime >= 120f && currentTime < 240f)
        {
            numItems = Random.Range(1, 3);
        }
        else if (currentTime >= 420f && currentTime < 600f)
        {
            numItems = Random.Range(1, 4);
        }
        else if (currentTime >= 780f)
        {
            numItems = Random.Range(1, 6);
        }

        for (int i = 0; i < numItems; i++)
        {
            GameObject randomFood = foodItems[Random.Range(0, foodItems.Length)];
            Instantiate(randomFood, thinkBubble.transform.position, Quaternion.identity, thinkBubble.transform);
        }

        // Schedule the customer to leave and pay after waitTime
        Invoke("LeaveAndPay", waitTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            LeaveAndPay();
            Destroy(other.gameObject); // Destroy the food item
        }
    }

    void LeaveAndPay()
    {
        float elapsedTime = Time.time - startTime;
        int starRating;

        if (elapsedTime <= waitTime)
        {
            Debug.Log("Update money and star rating");
            starRating = CalculateStarRating(elapsedTime);
            int moneyEarned = CalculateMoneyEarned(elapsedTime);

            // Update money and star rating UI
            moneyUI.UpdateMoney(moneyEarned);
        }
        else
        {
            starRating = -1; // Customer did not pay, assign -1 star rating
        }

        // Update total star rating and total customers
        totalStarRating += starRating;
        totalCustomers++;

        // Update average star rating UI
        starRatingUI.UpdateStarRating((int)(totalStarRating / totalCustomers));

        // Destroy the customer object
        Destroy(gameObject);
    }

    int CalculateStarRating(float elapsedTime)
    {
        if (elapsedTime <= 10f) return 5;
        else if (elapsedTime <= 15f) return 4;
        else if (elapsedTime <= 20f) return 3;
        else if (elapsedTime <= 25f) return 2;
        else return 1;
    }

    int CalculateMoneyEarned(float elapsedTime)
    {
        if (elapsedTime <= 10f) return 100;
        else if (elapsedTime <= 15f) return 80;
        else if (elapsedTime <= 20f) return 60;
        else if (elapsedTime <= 25f) return 40;
        else return 20;
    }
}







