using UnityEngine;
using UnityEngine.UI;

public class StarRatingUI : MonoBehaviour
{
    public Image[] starImages;

    private int starRating;

    void Start()
    {
        // Set the initial star rating to 3
        starRating = 3;
        UpdateStarRatingUI(starRating);
    }

    public void UpdateStarRating(int newStarRating)
    {
        // Ensure that newStarRating is within the valid range
        newStarRating = Mathf.Clamp(newStarRating, 0, starImages.Length);

        starRating = newStarRating;
        UpdateStarRatingUI(starRating);
    }

    private void UpdateStarRatingUI(int rating)
    {
        // Enable the stars based on the star rating
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].enabled = i < rating;
        }
    }

    public int GetStarRating()
    {
        return starRating;
    }
}





