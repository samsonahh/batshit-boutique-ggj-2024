using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Image image;

    public void OnDrag(PointerEventData eventData)
    {
        GameManager.Instance.b_isDraggingTool = true;
        transform.position = Vector2.Lerp(transform.position, Input.mousePosition, 50 * Time.deltaTime);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.b_isDraggingTool = false;
        GameManager.Instance.ApplyRandomMakeup(gameObject.name);
        transform.position = originalPosition;
    }

    private void Start()
    {
        originalPosition = transform.position;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
}
