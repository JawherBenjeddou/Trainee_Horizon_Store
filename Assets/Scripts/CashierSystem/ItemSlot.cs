using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] bool ImCashMachine;
    [SerializeField] MoneyHandler My_MnynHndlr;
    [SerializeField] GameObject ReturnedMoneyGo;
    [SerializeField] GameObject NewAddedMoneyValuePrefab;
    [SerializeField] Text ReturnedMoneyTextValue;
    GameObject CurrentDropedDragableItem;

    public void OnDrop(PointerEventData eventData)
    {
            if (eventData.pointerDrag != null)
            {
                // Get a random position within the specified range
                Vector2 randomOffset = Random.insideUnitCircle * (GetComponent<RectTransform>().rect.width * 0.1f);
                RectTransform itemRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                RectTransform slotRectTransform = GetComponent<RectTransform>();
                CurrentDropedDragableItem = itemRectTransform.gameObject;
                //Vector3 difference = slotRectTransform.position - itemRectTransform.position;
                itemRectTransform.SetParent(transform);
                // Set the item's position to the slot's center position
                itemRectTransform.position = slotRectTransform.position;
                itemRectTransform.anchoredPosition = new Vector2(itemRectTransform.anchoredPosition.x + randomOffset.x, itemRectTransform.anchoredPosition.y + randomOffset.y);
                itemRectTransform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                // Remove the DragDrop script component from the dropped item
                Destroy(eventData.pointerDrag.GetComponent<DragDrop>());
                eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1;
                GameObject newAddedMoneyValue = Instantiate(NewAddedMoneyValuePrefab, ReturnedMoneyGo.transform);
                newAddedMoneyValue.GetComponent<Text>().text = My_MnynHndlr.GetMoneyValue(My_MnynHndlr.FindSpriteIndex(itemRectTransform.gameObject.GetComponent<Image>().sprite)).ToString();
                OrganizeChildren(ReturnedMoneyGo);
                ReturnedMoneyTextValue.text= My_MnynHndlr.PlayerReturnedMoneyValue().ToString();
            }
    }
    // Function to find the first child GameObject meeting the condition
 
    public void OrganizeChildren(GameObject parent)
    {
        // Get parent RectTransform
        RectTransform parentRect = parent.GetComponent<RectTransform>();
        // Margin from the top of the parent
        float margin = parentRect.sizeDelta.y * 0.01f;
        int numberOflines =1;
        foreach (Transform child in parent.transform)
        {
            RectTransform childRect = child.GetComponent<RectTransform>();
            float InitialTranslate = -(childRect.sizeDelta.y / 2);

            if (numberOflines == 1)
            {
                childRect.anchoredPosition = new Vector2(0, -(((childRect.sizeDelta.y / 2) * numberOflines) +margin));
                numberOflines += 1;
            }
            else
            {
                childRect.anchoredPosition = new Vector2(0, -(((childRect.sizeDelta.y) * numberOflines) +margin+  InitialTranslate));
                numberOflines += 1;
            }
        }
    }
}