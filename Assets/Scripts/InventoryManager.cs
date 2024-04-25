using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UPersian.Components;
using UnityEngine.PlayerLoop;


namespace com.horizon.store
{
    public class InventoryManager : MonoBehaviour
    {
        public List<CollectibleItem> m_TargetItems;

        private List<CollectibleItem> m_PickedItems;


        private List<GameObject> m_InstantiatedGroceryUI;

        [SerializeField] private Transform m_CanvasForGrocery;
        [SerializeField] private GameObject m_GroceryUIPrefab;
        [SerializeField] private Vector2 m_InitialSpawnPosition;

        void Start()
        {
            m_PickedItems = new List<CollectibleItem>();
            m_InstantiatedGroceryUI = new List<GameObject>();
            InstantiateGroceryUI();
        }

        public void CollectItem(CollectibleItem item)
        {
            // Check if the collected item is not in the target list
            if (!m_TargetItems.Contains(item))
            {
                Debug.Log("Item '" + item.m_ItemName + "' is not in the target list.");
                ScreenShake.Instance.Shake();
                return;
            }

            AddToInventory(item);
            UpdateUI(item);

            // Check if all items are collected
            CheckCollectedItems();
        }


        private void AddToInventory(CollectibleItem itemType)
        {
            m_PickedItems.Add(itemType);
            Debug.Log(itemType.name + " added to inventory.");
        }

        private void InstantiateGroceryUI()
        {
            Debug.Log("Number of target items: " + m_TargetItems.Count);

            // stores processed item types
            HashSet<CollectibleItemType> processedTypes = new HashSet<CollectibleItemType>();

            foreach (CollectibleItem targetItem in m_TargetItems)
            {
                // Check if the item type has already been processed
                if (!processedTypes.Contains(targetItem.m_itemType))
                {
                    // Process all instances of the current item type
                    int totalCount = CountItemOccurrences(targetItem);

                    if (totalCount > 0)
                    {
                        GameObject groceryUI = Instantiate(m_GroceryUIPrefab, m_InitialSpawnPosition, Quaternion.identity, m_CanvasForGrocery);

                        // Set icon image, item name, and count
                        groceryUI.GetComponentInChildren<Image>().sprite = targetItem.m_itemIcon;
                        groceryUI.GetComponentInChildren<RtlText>().text = targetItem.m_ItemName;
                        groceryUI.transform.Find("Total").GetComponent<TextMeshProUGUI>().text = totalCount.ToString();
                        groceryUI.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = "0";

                        m_InstantiatedGroceryUI.Add(groceryUI);
                        m_InitialSpawnPosition.y -= 50.0f;

                        // Add the item type to the processed types list
                        processedTypes.Add(targetItem.m_itemType);
                    }
                }
            }
        }


        private void UpdateUI(CollectibleItem item)
        {
            // Find the UI element corresponding to the collected item type
            foreach (GameObject groceryUI in m_InstantiatedGroceryUI)
            {
                if (groceryUI.GetComponentInChildren<Image>().sprite == item.m_itemIcon)
                {
                    
                    TextMeshProUGUI countText = groceryUI.transform.Find("Count").GetComponent<TextMeshProUGUI>();

                    // Get the current count from the UI and subtract 1
                    int currentCount = int.Parse(countText.text);
                    currentCount++;

                    // Update the count in the UI
                    countText.text = currentCount.ToString();

                    // If the count reaches 0, remove the UI element
                    if (currentCount.ToString() == groceryUI.transform.Find("Total").GetComponent<TextMeshProUGUI>().text)
                    {
                        //TODO: do i keep on removing the ui from the list to not manipulate it by mistake later??
                        m_InstantiatedGroceryUI.Remove(groceryUI);
                        groceryUI.transform.Find("Total").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false) ;
                        groceryUI.transform.Find("Count").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false) ;
                        groceryUI.transform.Find("Slash").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false) ;
                        groceryUI.transform.Find("Tick").gameObject.SetActive(true) ;
                        //Destroy(groceryUI);
                    }

                    break;
                }
            }
        }
        private int CountItemOccurrences(CollectibleItem item)
        {
            int count = 0;
            foreach (CollectibleItem collectedItem in m_TargetItems)
            {
                if (collectedItem.m_itemType == item.m_itemType)
                {
                    count++;
                }
            }
            return count;
        }

        private void CheckCollectedItems()  
        {
            if (m_PickedItems.Count == m_TargetItems.Count)
            {
                bool allItemsCollected = true;
                foreach (CollectibleItem targetItem in m_TargetItems)
                {
                    if (!m_PickedItems.Contains(targetItem))
                    {
                        allItemsCollected = false;
                        break;
                    }
                }

                if (allItemsCollected)
                {
                    Debug.Log("All items collected!");
                }
                else
                {
                    Debug.Log("Not all items collected yet.");
                }
            }

        }
    }
}





