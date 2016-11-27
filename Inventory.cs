using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
    Loader loader;
    public ItemDatabase database;
    GameObject inventoryPanel;
    GameObject inventorySlotPanel;
    GameObject equipmentPanel;
    GameObject equipmentSlotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();
    public List<GameObject> equipSlots = new List<GameObject>();

    void Start()
    {
        Invoke("GenerateInventory", 0.2f);
    }
    public void GenerateInventory()
    {
        inventoryPanel = GameObject.Find("Inventory Panel");
        inventorySlotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;
        equipmentPanel = GameObject.Find("Equipment Panel");
        ToggleInventory();
        ToggleEquipment();
        loader = GetComponent<Loader>();
        database = loader.ic;
        slotAmount = 25;
        equipmentSlotPanel = equipmentPanel.transform.FindChild("Equipment Manager").gameObject;
        for (int i = 0; i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(inventorySlotPanel.transform);
            slots[i].name = inventorySlot.name + " " + i.ToString();
            slots[i].GetComponent<InventorySlot>().id = i;
        }
        for (int i = 0; i < equipmentSlotPanel.transform.childCount; i++)
        {
            equipSlots.Add(equipmentSlotPanel.transform.GetChild(i).gameObject);
        }
        //for (int i = 0; i < items.Count; i++)
        //{
        //    AddItem(Random.Range(1,34), 1);
        //}
        AddItem(database.FetchItemByName("SteelKiteShield").ID,1);
        AddItem(32, 3);
        AddItem(1, 2);
    }
    public int CheckHowManyIHave(int id)
    {
        if(CheckIfIsInInventory(database.FetchItemByID(id)) == -1)
        {
            return 0;
        }
        else
        {
            int count = 0;
            for (int i = 0; i < items.Count; i++ )
            {
                Debug.Log("Stackable: " + items[i].Stackable);
                if (items[i].ID == id)
                {
                    if(items[i].Stackable == false)
                    {
                        count++;
                    }
                    else
                    {
                        int amt = slots[i].GetComponentInChildren<ItemData>().amount;
                        count += amt;
                    }
                }
            }
            return count;

        }
    }
    public void AddItem(int id, int amount)
    {
        if (amount == 0)
            return;
        Item itemToAdd = database.FetchItemByID(id);    
        if (itemToAdd.Stackable)
        {
            int item = CheckIfIsInInventory(itemToAdd);
            if (item != -1)
            {
                ItemData data = slots[item].transform.GetComponentInChildren<ItemData>();
                data.SetAmount(data.amount + amount);
                return;
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == 0)
            {
                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.position = itemObj.transform.parent.position;
                itemObj.GetComponent<Image>().sprite = itemToAdd.sprite;
                itemObj.name = itemToAdd.Title;
                if(amount > 1 && itemToAdd.Stackable)
                {
                    ItemData data = slots[i].transform.GetComponentInChildren<ItemData>();
                    data.item = itemToAdd;
                    data.slotID = i;
                    data.SetAmount(amount);
                    return;
                }
                else
                {
                    ItemData data = slots[i].transform.GetComponentInChildren<ItemData>();
                    data.item = itemToAdd;
                    data.slotID = i;
                    amount--;
                }
                if(amount == 0)
                {
                    break;
                }
            }
        }
    }

    public bool RemoveItem(int id, int amount)
    {
        Item itemToRemove = database.FetchItemByID(id);
        if (CheckIfIsInInventory(itemToRemove) == -1)
        {
            return false;
        }
        if (itemToRemove.Stackable)
        {
            Debug.Log(itemToRemove.Name);
            int item = CheckIfIsInInventory(itemToRemove);
            ItemData data = slots[item].transform.GetComponentInChildren<ItemData>();
            if(data.amount >= amount)
            {
                if (data.amount - amount == 0)
                {
                    items[item] = new Item();
                }
                data.SetAmount(data.amount - amount);
                return true;
            }
            else
            {
                return false;
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].ID == id)
            {
                items[i] = new Item();
                Destroy(slots[i].transform.GetChild(0).gameObject);
                amount--;
            }
            if (amount == 0)
            {
                return true;
            }
        }
        if(amount > 0)
        {
            return false;
        }
        return true;
    }

    public void SaveOutPlayerData(PlayerData data)
    {
        foreach(GameObject slot in slots)
        {
            if(slot.transform.childCount > 0)
            {
                ItemData item = slot.transform.GetChild(0).GetComponent<ItemData>();
                if(item != null)
                {
                    data.inventory.Add(item.item.ID);
                    data.inventoryAmounts.Add(item.amount);
                }
            }
        }
        equipmentSlotPanel.GetComponent<EquipmentManager>().FillInPlayerEquips(data);
    }
    int CheckIfIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++ )
        {
            if (items[i].ID == item.ID)
                return i;
        }
        return -1;
    }
    public bool CheckIfIHaveEnoughSlots(Item item, int amt)
    {
        if(item.Stackable)
        {
            if (CheckIfIsInInventory(item) >= 0)
            {
                return true;
            }
        }
        int itemCount = 0;
        foreach(GameObject slot in slots)
        {
            if(slot.transform.childCount > 0)
            {
                itemCount++;
            }
        }
        return amt <= slots.Count - itemCount;
    }
    public void ToggleInventory()
    {
        if (inventoryPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            inventoryPanel.GetComponent<CanvasGroup>().alpha = 0;
            inventoryPanel.GetComponent<UIMenu>().active = false;
            inventoryPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        }
        else
        {
            inventoryPanel.GetComponent<CanvasGroup>().alpha = 1;
            inventoryPanel.GetComponent<UIMenu>().active = true;
            inventoryPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (equipmentPanel.GetComponent<UIMenu>().active)
            {
                ToggleEquipment();
            }
            if (inventoryPanel.GetComponent<UIMenu>().active)
            {
                ToggleInventory();
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleEquipment();
        }
    }
    public void ToggleEquipment()
    {
        if (equipmentPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            equipmentPanel.GetComponent<CanvasGroup>().alpha = 0;
            equipmentPanel.GetComponent<UIMenu>().active = false;
            equipmentPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            equipmentPanel.GetComponent<CanvasGroup>().alpha = 1;
            equipmentPanel.GetComponent<UIMenu>().active = true;
            equipmentPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
