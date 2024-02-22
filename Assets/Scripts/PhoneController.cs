using UnityEngine;
using UnityEngine.UI;

public class PhoneController : MonoBehaviour
{
    public GameObject phonePopup;
    public GameObject recipeBookUI;
    public GameObject newsUI;
    public GameObject drinksUI;

    private bool phonePopupActive = false;

    private void Start()
    {
        // Disable UI elements at the start
        phonePopup.SetActive(false);
        recipeBookUI.SetActive(false);
        newsUI.SetActive(false);
        drinksUI.SetActive(false);
    }

    private void Update()
    {
        // Check for left click on the phone item
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Phone"))
            {
                // Toggle phone popup visibility
                phonePopupActive = !phonePopupActive;
                phonePopup.SetActive(phonePopupActive);
            }
        }
    }

    public void OpenRecipeBook()
    {
        // Disable phone popup and enable recipe book UI
        recipeBookUI.SetActive(true);
        newsUI.SetActive(false);
        drinksUI.SetActive(false);
    }

    public void OpenDrinks()
    {
        // Disable phone popup and enable drinks UI
        drinksUI.SetActive(true);
        recipeBookUI.SetActive(false);
        newsUI.SetActive(false);
    }

    public void OpenNews()
    {
        // Disable phone popup and enable news UI
        recipeBookUI.SetActive(false);
        newsUI.SetActive(true);
        drinksUI.SetActive(false);
    }
    public void Home()
    {
        // Return to the home menu on phone
        recipeBookUI.SetActive(false);
        newsUI.SetActive(false);
        drinksUI.SetActive(false);
    }

    public void Close()
    {
        // Return to the home menu on phone
        recipeBookUI.SetActive(false);
        drinksUI.SetActive(false);
        newsUI.SetActive(false);
        phonePopup.SetActive(false);
    }
}


