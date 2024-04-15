using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace com.horizon.store
{
 
    public class InventoryManager : MonoBehaviour
    {
        private List<CollectibleItem> collectedItems;

        public void CollectItem(CollectibleItem item)
        {
            AddToInventory(item);
        }

        private void AddToInventory(CollectibleItem item)
        {
            collectedItems.Add(item);
        }
    }

}