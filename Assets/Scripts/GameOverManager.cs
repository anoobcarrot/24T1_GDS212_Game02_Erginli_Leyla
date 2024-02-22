using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    private StarRatingUI starRatingUI;

    private void Start()
    {
        starRatingUI = FindObjectOfType<StarRatingUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for game over condition
        if (starRatingUI.GetStarRating() <= 0)
        {
            // Trigger game over
            gameOverUI.SetActive(true); // Show game over UI
        }
    }

    public void ReturnToMainMenu()
    {
        
    }
}
