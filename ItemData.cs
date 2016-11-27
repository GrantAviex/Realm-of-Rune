using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    public int slotID;
    public Text StackText;
    EquipmentManager eqpManager;
    public Vector3 originalPosition;
    private Inventory inv;
    private Tooltip tooltip;
    StoreManager storeManager;
    Vector2 dragOffset;
    void Start()
    {
        storeManager = GameObject.Find("Store Manager").GetComponent<StoreManager>();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        eqpManager = GameObject.Find("Equipment Manager").GetComponent<EquipmentManager>();
        StackText = GetComponentInChildren<Text>();
        tooltip = inv.GetComponent<Tooltip>();
    }
    public void SetAmount(int _amount)
    {
        if(StackText == null)
        {
            StackText = GetComponentInChildren<Text>();
        }
        if (_amount == 0)
        {
            Destroy(gameObject);
        }
        amount = _amount;
        StackText.text = amount.ToString();
    }
    public void SetSlot(int slot)
    {
        slotID = slot;
        transform.SetParent(inv.slots[slot].transform);
        transform.position = transform.parent.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                this.transform.SetParent(this.transform.parent.parent);
                dragOffset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
                originalPosition = transform.parent.position - transform.position;
                this.transform.position = eventData.position - dragOffset;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                this.transform.position = eventData.position - dragOffset; ;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(item != null)
            {
                this.transform.SetParent(inv.slots[slotID].transform);
                this.transform.position = transform.parent.position;
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item, false,eventData.position); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }   
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(storeManager.storePanel.activeSelf == true)
            {
                storeManager.ActivateSellingToolTip(eventData.position, this);
                return;
            }
            if(item.Eqp != null)
            {
                if(eqpManager.EquipItem(item, item.Eqp.Type, amount))
                {
                    inv.items[slotID] = new Item();
                    tooltip.Deactivate();
                    Destroy(gameObject);
                }
            }
            if(item.Use != null)
            {
                if (eqpManager.modelEquip == null)
                    eqpManager.FindModelEquip();
                eqpManager.modelEquip.gameObject.SendMessage("MapActivate");
            }
        }
    }
}
