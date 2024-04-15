using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace com.horizon.store
{
    //enum for different types of collectible items
    public enum CollectibleItemType
    {
        Apple,
        Banana
    }


    [CreateAssetMenu(fileName = "New Collectible Item", menuName = "Collectible Item")]
    public class CollectibleItem : ScriptableObject
    {
        public string m_ItemName;
        public CollectibleItemType m_itemType;
        public Sprite m_itemIcon; // Icon for the collectible item (for UI)
        public int m_price;

        public void Initialize(string name, CollectibleItemType type, Sprite icon, int itemPrice)
        {
            m_ItemName = name;
            m_itemType = type;
            m_itemIcon = icon;
            m_price = itemPrice;
        }
    }
}