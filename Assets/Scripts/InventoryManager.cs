using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UPersian.Components;

namespace com.horizon.store
{
    public class InventoryManager : MonoBehaviour
    {
        public List<CollectibleItem> m_TargetItems;

        private List<CollectibleItem> m_CollectedItems;


        private List<GameObject> m_InstantiatedGrocery;

        [SerializeField] private Transform m_CanvasForGrocery;
        [SerializeField] private GameObject m_GroceryUIPrefab;
        [SerializeField] private Vector2 m_InitialSpawnPosition;

        void Start()
        {

            m_CollectedItems = new List<CollectibleItem>();
            m_InstantiatedGrocery = new List<GameObject>();
            InstantiateGroceryUI();
        }

        public void CollectItem(CollectibleItem item)
        {
            AddToInventory(item);

            // Check if all items are collected
            CheckCollectedItems();
        }

        private void AddToInventory(CollectibleItem itemType)
        {
            m_CollectedItems.Add(itemType);
            Debug.Log(itemType.name + " added to inventory.");
        }

        private void InstantiateGroceryUI()
        {
            Debug.Log("Number of target items: " + m_TargetItems.Count);

            // hashset stores processed item types
            HashSet<CollectibleItemType> processedTypes = new HashSet<CollectibleItemType>();

            foreach (CollectibleItem targetItem in m_TargetItems)
            {
                // Check if the item type has already been processed
                if (!processedTypes.Contains(targetItem.m_itemType))
                {
                    // Process all instances of the current item type
                    int totalCount = 0;

                    foreach (CollectibleItem item in m_TargetItems)
                    {
                        if (item.m_itemType == targetItem.m_itemType)
                        {
                            totalCount++;
                        }
                    }

                    if (totalCount > 0)
                    {
                        GameObject groceryUI = Instantiate(m_GroceryUIPrefab, m_InitialSpawnPosition, Quaternion.identity, m_CanvasForGrocery);

                        // Set icon image, item name, and count
                        groceryUI.GetComponentInChildren<Image>().sprite = targetItem.m_itemIcon;
                        groceryUI.GetComponentInChildren<RtlText>().text = targetItem.m_ItemName;
                        groceryUI.GetComponentInChildren<TextMeshProUGUI>().text = totalCount.ToString();

                        m_InstantiatedGrocery.Add(groceryUI);
                        m_InitialSpawnPosition.y -= 40.0f;

                        // Add the item type to the processed types list
                        processedTypes.Add(targetItem.m_itemType);
                    }
                }
            }
        }



        private int CountItemOccurrences(CollectibleItem item)
        {
            int count = 0;
            foreach (CollectibleItem collectedItem in m_TargetItems)
            {
                if (collectedItem == item)
                {
                    count++;
                }
            }
            return count;
        }



        private void CheckCollectedItems()  
        {
            if (m_CollectedItems.Count == m_TargetItems.Count)
            {
                bool allItemsCollected = true;
                foreach (CollectibleItem targetItem in m_TargetItems)
                {
                    if (!m_CollectedItems.Contains(targetItem))
                    {
                        allItemsCollected = false;
                        break;
                    }
                }

                if (allItemsCollected)
                {
                    Debug.Log("All items collected!");
                    // Handle when all items are collected
                }
                else
                {
                    Debug.Log("Not all items collected yet.");
                }
            }
        }
    }
}
