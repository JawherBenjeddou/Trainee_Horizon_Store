using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UPersian.Components;

namespace com.horizon.store
{

    [System.Serializable]
    public struct ItemPair
    {
        public GameObject uiElement;
        public GameObject itemPrefab;
    }

    public class InventoryManager : MonoBehaviour
    {
        private Dictionary<CollectibleItemType, int> m_CollectedItems;
        private Dictionary<CollectibleItemType, int> m_TargetItems;
        private List<ItemPair> m_UIItemPairs;
        [SerializeField]
        private GameObject m_ItemUI; //ItemUI PREFAB.
        [SerializeField]
        private Transform m_CanvasForUI;
        [SerializeField]
        private Vector2 m_InitialSpawnPosition;
        [SerializeField]
        private List<GameObject> m_InGameItems;

        void Start()
        {
            m_CollectedItems = new Dictionary<CollectibleItemType, int>();
            m_TargetItems = new Dictionary<CollectibleItemType, int>();

            InitializeUI();
        }

        public void CollectItem(CollectibleItem item)
        {
            AddToInventory(item.m_itemType);
        }

        private void InitializeUI()
        {
            m_UIItemPairs = new List<ItemPair>();
            Vector2 spawnPosition = m_InitialSpawnPosition;
            foreach (GameObject item in m_InGameItems) //this goes threw each item in the scene to make it's ui
            {
                GameObject newItem = Instantiate(m_ItemUI, spawnPosition, Quaternion.identity, m_CanvasForUI);
                m_UIItemPairs.Add(new ItemPair { uiElement = newItem, itemPrefab = item });
                // TODO: Check if they both pointing to the same memory or it's making a copy m_UIItemPairs[0] and newItem
                SetThumbnailOnImage(newItem, item);
                spawnPosition.y += 50.0f;
            }
        }

        private void AddToInventory(CollectibleItemType itemType)
        {
            if (m_CollectedItems.ContainsKey(itemType))
            {
                m_CollectedItems[itemType]++;
            }
            else
            {
                m_CollectedItems.Add(itemType, 1);
            }
        }


        // Method to get the thumbnail sprite of the collectibleObject
        public Sprite GetThumbnail(GameObject prefab)
        {

            Texture2D thumbnail = AssetPreview.GetAssetPreview(prefab);
            if (thumbnail != null)
            {
                // Create a sprite from the texture
                return Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.one * 0.5f);
            }

            Debug.LogWarning("Thumbnail sprite not found for the collectible object: " + gameObject.name);
            return null;
        }

        void SetThumbnailOnImage(GameObject itemUI, GameObject prefab)
        {
            if (itemUI == null || prefab == null)
            {
                Debug.LogWarning("Item UI or Prefab reference is null!");
                return;
            }
                var imageComponent = itemUI.GetComponentInChildren<UnityEngine.UI.Image>();
                if (imageComponent != null)
                {
                    
                    imageComponent.sprite = GetThumbnail(prefab);
                }
                else
                {
                    Debug.LogWarning("Image component not found in item UI!");
                }
            }
        }
}