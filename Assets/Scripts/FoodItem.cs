using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        // Store the original position and rotation of the food item
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        // Instantiate the food item back to its original position and rotation
        Instantiate(gameObject, originalPosition, originalRotation);
    }
}

