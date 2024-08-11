using UnityEngine;
using UnityEngine.UI;

namespace com.horizon.store
{
    //enum for different types of collectible items
    public enum CollectibleItemType
    {
        Apple,
        Banana,
        Ananas,
        Milk
    }


    [CreateAssetMenu(fileName = "New Collectible Item", menuName = "Collectible Item")]
    public class CollectibleItem : ScriptableObject
    {
        public string m_ItemName;
        public CollectibleItemType m_itemType;
        public Sprite m_itemIcon; // Icon for the collectible item (for UI)
        public int m_price;
        
    }
}