using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 startPosition;
    private Transform originalParent;

    private void OnMouseDown()
    {
        isDragging = true;
        startPosition = transform.position;
        originalParent = transform.parent;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        transform.position = startPosition;

        // Check if dropped on a valid target and handle accordingly
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && !isDragging)
        {
            if (hit.collider.CompareTag("Stove"))
            {
                Debug.Log("Dropped on stove");
            }
            else if (hit.collider.CompareTag("Plate"))
            {
                Debug.Log("Dropped on plate");
            }
            else
            {
                Debug.Log("Dropped on something else");
            }
        }
    }
}





