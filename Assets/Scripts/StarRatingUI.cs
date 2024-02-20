using UnityEngine;
using UnityEngine.UI;

public class StarRatingUI : MonoBehaviour
{
    public Image[] starImages;

    void Start()
    {
        // Ensure that only the first star is initially visible
        for (int i = 1; i < starImages.Length; i++)
        {
            starImages[i].enabled = false;
        }
    }

    public void UpdateStarRating(int starRating)
    {
        // Ensure that starRating is within the valid range
        starRating = Mathf.Clamp(starRating, 1, starImages.Length);

        // Enable the first star and disable the rest based on the star rating
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < starRating)
                starImages[i].enabled = true;
            else
                starImages[i].enabled = false;
        }
    }
}



