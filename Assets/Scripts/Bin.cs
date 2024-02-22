using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    // List of tags for objects that should not be deleted
    public List<string> protectedTags = new List<string>();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Cast a ray from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Check if the ray hits a collider with a game object
            if (hit.collider != null)
            {
                // Check if the ray hit object is not the bin itself
                if (hit.collider.gameObject != gameObject)
                {
                    // Check if the ray hit object is inside the bin's collider
                    if (GetComponent<Collider2D>().bounds.Contains(hit.collider.bounds.center))
                    {
                        // Check if the hit object has one of the protected tags
                        if (protectedTags.Contains(hit.collider.tag))
                        {
                            Debug.Log("This object cannot be destroyed.");
                        }
                        else
                        {
                            // Destroy the hit object
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }
        }
    }
}


