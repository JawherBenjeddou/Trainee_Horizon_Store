using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    bool DragableItemAlreadyDroped = false;
    [SerializeField] bool ImMoneyHolder;
    [SerializeField] bool ImCashMachine;
    [SerializeField] MoneyHandler My_MnynHndlr;
    [SerializeField] GameObject ReturnedMoneyGo;
    [SerializeField] GameObject NewAddedMoneyValuePrefab;
    [SerializeField] Text ReturnedMoneyTextValue;
    GameObject CurrentDropedDragableItem;
    ItemSlot MyParentitemSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (ImCashMachine)
        {
            if (eventData.pointerDrag != null)
            {
                Destroy(eventData.pointerDrag);
                My_MnynHndlr.NumberOfGivenPaper--;
                if (My_MnynHndlr.NumberOfGivenPaper==0)
                {
                    My_MnynHndlr.CashMachineAnimation();
                }
            }
        }
        else if (ImMoneyHolder)
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
        else
        {
            if (eventData.pointerDrag != null)
            {
                if (DragableItemAlreadyDroped)
                {
                    Destroy(CurrentDropedDragableItem);
                }
                RectTransform itemRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                RectTransform slotRectTransform = GetComponent<RectTransform>();
                CurrentDropedDragableItem = itemRectTransform.gameObject;

                // Set the item's position to the slot's center position
                itemRectTransform.position = slotRectTransform.position;

                // Remove the DragDrop script component from the dropped item
                Destroy(eventData.pointerDrag.GetComponent<DragDrop>());
                eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1;
                DragableItemAlreadyDroped = true;
                transform.GetChild(0).GetComponent<Image>().sprite = CurrentDropedDragableItem.GetComponent<Image>().sprite;
            }
        }

    }
    // Function to find the first child GameObject meeting the condition
    public GameObject FindFirstChildWithNoneSprite()
    {
        // Get the transform of the GameObject this script is attached to
        Transform parentTransform = transform;

        // Iterate through all the child GameObjects of the parent GameObject
        foreach (Transform childTransform in parentTransform)
        {
            // Check if the current child has a child with an image sprite equal to none
            Image childImage = childTransform.GetComponentInChildren<Image>();
            if (childImage != null && childImage.sprite == null)
            {
                // Return the first child GameObject meeting the condition
                return childTransform.gameObject;
            }
        }

        // Return null if no child GameObject meets the condition
        return transform.GetChild(0).gameObject;
    }
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

/*if (DragableItemAlreadyDroped)
{
    Destroy(CurrentDropedDragableItem);
}
Image ImageHoldreForComparison;
RectTransform itemRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
RectTransform slotRectTransform;
if (transform.gameObject.name == "ReturnedMoneyContainer" || transform.GetComponent<Image>().sprite == null)
{
    if (transform.gameObject.name == "ReturnedMoneyContainer")
    {
        slotRectTransform = FindFirstChildWithNoneSprite().GetComponent<RectTransform>();
        ImageHoldreForComparison = FindFirstChildWithNoneSprite().transform.GetChild(0).GetComponent<Image>();
        FindFirstChildWithNoneSprite().transform.GetChild(0).GetComponent<Image>().sprite = itemRectTransform.gameObject.GetComponent<Image>().sprite;
        ImageHoldreForComparison.SetNativeSize();
        if (My_MnynHndlr.IsItPaperKind(ImageHoldreForComparison.sprite))
        {
            ImageHoldreForComparison.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -90f); ;
        }
    }
    else
    {
        MyParentitemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        slotRectTransform = MyParentitemSlot.FindFirstChildWithNoneSprite().GetComponent<RectTransform>();
        ImageHoldreForComparison = MyParentitemSlot.FindFirstChildWithNoneSprite().transform.GetChild(0).GetComponent<Image>();
        MyParentitemSlot.FindFirstChildWithNoneSprite().transform.GetChild(0).GetComponent<Image>().sprite = itemRectTransform.gameObject.GetComponent<Image>().sprite;
        ImageHoldreForComparison.SetNativeSize();
        if (My_MnynHndlr.IsItPaperKind(ImageHoldreForComparison.sprite))
        {
            ImageHoldreForComparison.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -90f); ;
        }
    }
    ImageHoldreForComparison.color = new Color(1f, 1f, 1f, 1f);
}
else
{
    slotRectTransform = GetComponent<RectTransform>();
    ImageHoldreForComparison = transform.GetChild(0).GetComponent<Image>();
    transform.GetChild(0).GetComponent<Image>().sprite = itemRectTransform.gameObject.GetComponent<Image>().sprite;
    ImageHoldreForComparison.SetNativeSize();
    if (My_MnynHndlr.IsItPaperKind(ImageHoldreForComparison.sprite))
    {
        ImageHoldreForComparison.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -90f); ;
    }
    ImageHoldreForComparison.color = new Color(1f, 1f, 1f, 1f);

}
Sprite HolderForMoney = My_MnynHndlr.ReturnSuitableMoneyHolderSprite(eventData.pointerDrag.GetComponent<Image>().sprite);
slotRectTransform.gameObject.GetComponent<Image>().sprite = HolderForMoney;
slotRectTransform.gameObject.GetComponent<Image>().SetNativeSize();
slotRectTransform.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

CurrentDropedDragableItem = itemRectTransform.gameObject;
// Calculate the difference between the slot and item positions in world space
//Vector3 difference = slotRectTransform.position - itemRectTransform.position;

// Set the item's position to the slot's center position
itemRectTransform.position = slotRectTransform.position;
if (transform.gameObject.name == "ReturnedMoneyContainer")
{
    OrganizeChildren(ReturnedMoneyGo);

}
else
{
    OrganizeChildren(transform.parent.gameObject);

}
// Adjust the item's position to maintain the visual center alignment
//  itemRectTransform.position += difference;

// Remove the DragDrop script component from the dropped item
eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1;
DragableItemAlreadyDroped = true;
Destroy(eventData.pointerDrag);
            }*/

