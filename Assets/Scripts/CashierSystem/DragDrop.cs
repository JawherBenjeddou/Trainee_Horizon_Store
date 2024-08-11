using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject draggablePrefab; // Prefab of the draggable item
    public bool resize;
    public bool rotate;
    public bool NonInstantiable;
    private RectTransform rectTransform;
    private Vector2 initialRectTransform;
    private CanvasGroup canvasGroup;
    private GameObject initialCopy; // Copy of the draggable item at its initial position

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialRectTransform = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        if (resize)
        {
            rectTransform.sizeDelta = new Vector2(200f, 200f);

        }
        if (rotate)
        {
            rectTransform.Rotate(new Vector3(0, 0, -90f));
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"canvas.scaleFactor{canvas.scaleFactor}");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (!NonInstantiable)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(initialRectTransform.x, initialRectTransform.y);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (NonInstantiable==false)
        {
            initialCopy = Instantiate(draggablePrefab, rectTransform.position, Quaternion.identity, rectTransform.parent);
            initialCopy.GetComponent<CanvasGroup>().alpha = 1f;
        }
        Debug.Log("OnPointerDown");
     
    }
}
