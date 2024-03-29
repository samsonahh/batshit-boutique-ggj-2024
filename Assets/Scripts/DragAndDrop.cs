using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private Image image;

    bool b_isDraggingTool = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.b_canDragTool) return;
        b_isDraggingTool = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.b_canDragTool) return;
        GameManager.Instance.b_isDraggingTool = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.b_isDraggingTool = false;
        b_isDraggingTool = false;
        if (!GameManager.Instance.b_canDragTool) return;
        if (!GameManager.Instance.b_isOverFace) return;
        GameManager.Instance.ApplyMakeup(gameObject.name);
        transform.position = originalPosition;
    }

    private void Start()
    {
        originalPosition = transform.position;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!GameManager.Instance.b_canDragTool) return;

        if (b_isDraggingTool && GameManager.Instance.b_isDraggingTool)
        {
            Cursor.visible = false;
            transform.position = Vector2.Lerp(transform.position, Input.mousePosition, 50 * Time.deltaTime);
        }
        else
        {
            Cursor.visible = true;
            transform.position = originalPosition;
        }
    }
}
