using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoneyDropSlot : MonoBehaviour, IDropHandler
{
    public int maxChildren = 5; // The maximum number of child image objects
    public GameObject imagePrefab; // Prefab of the image object to be instantiated
    public List<Image> slotImages = new List<Image>(); // List to store the child image objects
    void Start()
    {
        // Create child image objects
        CreateSlotImages();
    }

    // Create child image objects based on the specified number
    void CreateSlotImages()
    {
        RectTransform slotRectTransform = GetComponent<RectTransform>();
        float slotWidth = slotRectTransform.rect.width;
        float slotHeight = slotRectTransform.rect.height;

        float imageWidth = slotWidth / maxChildren;
        float imageHeight = slotHeight;

        for (int i = 0; i < maxChildren; i++)
        {
            GameObject imageGO = Instantiate(imagePrefab, transform);
            Image image = imageGO.GetComponent<Image>();
            image.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);
            slotImages.Add(image);
        }

        UpdateSlotImagesPosition();
    }
  
    // Update the position of child image objects
    void UpdateSlotImagesPosition()
    {
        RectTransform slotRectTransform = GetComponent<RectTransform>();
        float slotWidth = slotRectTransform.rect.width;
        float slotHeight = slotRectTransform.rect.height;
        float imageWidth = slotWidth / maxChildren;

        for (int i = 0; i < slotImages.Count; i++)
        {
            float posX = -(slotWidth / 2) + (imageWidth * (i + 0.5f));
            float posY = 0f;
            slotImages[i].rectTransform.localPosition = new Vector3(posX, posY, 0);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Image slotImage = GetEmptySlotImage();

            if (slotImage != null)
            {
                // Set the slot image's sprite to the dropped item's sprite and set native size
                slotImage.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
                slotImage.SetNativeSize();

                // Set the alpha of the slot image to 1
                slotImage.color = new Color(slotImage.color.r, slotImage.color.g, slotImage.color.b, 1f);

                // Remove the DragDrop script component from the dropped item
                Destroy(eventData.pointerDrag.GetComponent<DragDrop>());
                eventData.pointerDrag.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }

    // Get an empty slot image
    Image GetEmptySlotImage()
    {
        foreach (Image image in slotImages)
        {
            if (image.sprite == null)
            {
                return image;
            }
        }
        return null;
    }
}
