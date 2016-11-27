using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InventorySlot : MonoBehaviour , IDropHandler
{
    public int id;
    private Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        Debug.Log(inv.items[id].ID);
        if(inv.items[id].ID == 0)
        {
            inv.items[droppedItem.slotID] = new Item();
            inv.items[id] = droppedItem.item;
            droppedItem.slotID = id;
        }
        else
        {
            Transform currItem = this.transform.GetChild(0);
            currItem.GetComponent<ItemData>().SetSlot(droppedItem.slotID);
            droppedItem.SetSlot(id);

            inv.items[droppedItem.slotID] = currItem.GetComponent<ItemData>().item;
            inv.items[id] = droppedItem.item;
        }
    }

}
