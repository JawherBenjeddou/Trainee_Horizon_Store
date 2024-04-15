using System.Collections;
using System.Collections.Generic;
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

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            OnInteract();
                        }
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