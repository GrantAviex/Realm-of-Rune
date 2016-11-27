using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool active;
    Vector2 dragOffset;
    public Vector3 originalPosition;


    void FixedUpdate()
    {
        if(active)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                active = false;
                if(gameObject.GetComponent<CanvasGroup>() != null)
                {
                    gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            dragOffset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
            originalPosition = transform.position;
            this.transform.position = eventData.position - dragOffset;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.transform.position = eventData.position - dragOffset; 
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {   
        }
    }
}
