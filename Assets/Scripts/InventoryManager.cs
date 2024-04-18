using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UPersian.Components;

namespace com.horizon.store
{
    public class InventoryManager : MonoBehaviour
    {
        private Dictionary<CollectibleItemType, int> m_CollectedItems;
        private Dictionary<CollectibleItemType, int> m_TargetItems;
        public GameObject m_ItemUI;
        public Transform Canvas;
        GameObject m_ItemInstance;
        public CollectibleObject apple;

        void Start()
        {
            m_ItemInstance = Instantiate(m_ItemUI, new Vector2(235.8f, 924f), Quaternion.identity, Canvas);

            m_CollectedItems = new Dictionary<CollectibleItemType, int>();
            m_TargetItems = new Dictionary<CollectibleItemType, int>();
            StartCoroutine(GetSprite(1.0f));
        }
        private IEnumerator GetSprite(float time)
        {
            yield return new WaitForSeconds(time);
            var imageComponent = m_ItemInstance.GetComponentInChildren<UnityEngine.UI.Image>();
            imageComponent.sprite = apple.icon; 
            
        }

        public void CollectItem(CollectibleItem item)
        {
            AddToInventory(item.m_itemType);
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


        //Texture2D GetPrefabThumbnail(GameObject prefab)
        //{
        //    if (prefab == null)
        //    {
        //        Debug.LogWarning("Prefab is null!");
        //        return null;
        //    }

        //    // Get the preview texture of the prefab
        //    Texture2D thumbnail = AssetPreview.GetAssetPreview(prefab);

        //    if (thumbnail == null)
        //    {
        //        Debug.LogWarning("No thumbnail found for the prefab!");
        //        return null;
        //    }

        //    return thumbnail;
        //}

        //void SetThumbnailOnImage(GameObject itemUI, GameObject prefab)
        //{
        //    if (itemUI == null || prefab == null)
        //    {
        //        Debug.LogWarning("Item UI or Prefab reference is null!");
        //        return;
        //    }

        //    Texture2D thumbnail = GetPrefabThumbnail(prefab);
        //    if (thumbnail != null)
        //    {
        //        var imageComponent = itemUI.GetComponentInChildren<UnityEngine.UI.Image>();
        //        if (imageComponent != null)
        //        {
        //            Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.zero);
        //            imageComponent.sprite = sprite;
        //        }
        //        else
        //        {
        //            Debug.LogWarning("Image component not found in item UI!");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No thumbnail found for the prefab!");
        //    }
        //}

    }
}