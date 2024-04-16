using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPersian.Components;

namespace com.horizon.store
{
    public class InventoryManager : MonoBehaviour
    {
        private Dictionary<CollectibleItemType, int> m_CollectedItems;
        private Dictionary<CollectibleItemType, int> m_TargetItems;
        public RtlText m_Text;
        void Start()
        {
            m_CollectedItems = new Dictionary<CollectibleItemType, int>();
            m_TargetItems = new Dictionary<CollectibleItemType, int>();
            m_TargetItems.Add(CollectibleItemType.Apple, 3);
            m_TargetItems.Add(CollectibleItemType.Banana, 6);

            InitializeUIText();
        }

        public void CollectItem(CollectibleItem item)
        {
            AddToInventory(item.m_itemType);
            UpdateUIText();
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

        private void InitializeUIText()
        {
            foreach (var kvp in m_TargetItems)
            {
                if(kvp.Key == CollectibleItemType.Apple)
                { 
                    m_Text.text += kvp.Value + " " + "تفاحاة" + "\n";
                }
                else
                {
                    m_Text.text += kvp.Value + " " + "موز" + "\n";
                }
            }
        }

        private void UpdateUIText()
        {
            m_Text.text = "";
            foreach (var kvp in m_CollectedItems)
            {
                if (kvp.Key == CollectibleItemType.Apple)
                {
                    int remaining = (m_TargetItems.ContainsKey(kvp.Key) ? m_TargetItems[kvp.Key] : 0) - kvp.Value;
                    m_Text.text += remaining + " " + "تفاحاة" + "\n";
                }
                else if (kvp.Key == CollectibleItemType.Banana)
                {
                    int remaining = (m_TargetItems.ContainsKey(kvp.Key) ? m_TargetItems[kvp.Key] : 0) - kvp.Value;
                    m_Text.text += remaining + " " + "موز" + "\n";
                }
            }
        }


    }
}
