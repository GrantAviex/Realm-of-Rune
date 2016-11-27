using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class EquippedItemData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int amount;
    EquipmentManager eqpManager;
    private Inventory inv;
    private Tooltip tooltip;
    public Text amountText;

    void Start()
    {
        amount = 0;
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        eqpManager = GameObject.Find("Equipment Manager").GetComponent<EquipmentManager>();
        tooltip = inv.GetComponent<Tooltip>();
    }
    public void SetGear(Item gear)
    {
        if (item != null)
        {
            inv.AddItem(item.ID, amount);
        }

        item = gear;
        Image gearImage = GetComponent<Image>();
        if (gear != null)
        {
            gearImage.enabled = true;
            gearImage.sprite = item.sprite;
            amount = 1;
            if(amountText)
                amountText.text = "";
        }
        else
        {
            gearImage.enabled = false;
            amount = 0;
            if(amountText)
                amountText.text = "";
        }
    }
    public void SetAmout(int newAmount)
    {
        amount = newAmount;
        if(newAmount > 1)
            amountText.text = amount.ToString();  
    }
    public void SetThrownWeaponAmount(int newAmount)
    {
        amount = newAmount;
        amountText.text = amount.ToString();
        if(newAmount == 0)
        {
            eqpManager.UnquipItem(transform.parent.gameObject, item.Eqp.Type);
            SetGear(null);
        }  
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item,false,eventData.position);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();   
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            eqpManager.UnquipItem(gameObject.transform.parent.gameObject, item.Eqp.Type);
            SetGear(null);
        }
    }
}
