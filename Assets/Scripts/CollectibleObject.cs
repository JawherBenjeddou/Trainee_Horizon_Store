using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




namespace com.horizon.store
{
    public class CollectibleObject : MonoBehaviour
    {
        public CollectibleItem m_CollectibleItem; // Reference to the CollectibleItem for this object
       
        void Update()
        {
            CheckTouch();
        }

        private void CheckTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // Perform the raycast and visualize it with a debug ray
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Draw a debug ray from the touch position to the hit point
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1f);

                        if (hit.collider.gameObject == gameObject)
                        {
                            OnInteract();
                        }
                    }
                    else
                    {
                        // Draw a debug ray from the touch position in the direction of the ray
                        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
                    }
                }
            }
        }


        // method called when the player interacts with the object
        public void OnInteract()
        {
            // Example: Collect the item and add it to the inventory
            FindObjectOfType<InventoryManager>().CollectItem(m_CollectibleItem);

            // Destroy the collectible object after it's collected
            Destroy(this.gameObject);
        }

   
    }
}