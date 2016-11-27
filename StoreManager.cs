using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

public class StoreManager : MonoBehaviour 
{
    public float maxItems;
    public GameObject storeItem;
    public string storeName;

    public Text storeNameText;
    public GameObject storePanel;
    public GameObject slotPanel;
    public GameObject tooltip;
    public GameObject sellingtip;
    public List<StoreItemData> items = new List<StoreItemData>();
    public List<GameObject> slots = new List<GameObject>();
    public Inventory inv;

    ItemData saleItem;
    StoreItemData currentItem;
    Merchant myMerchant;
	void Start () 
    {
        for( int  i = 0; i < slotPanel.transform.childCount; i++ )
        {
            slots.Add(slotPanel.transform.GetChild(i).gameObject);
        }
	}
	
    public void Activate(Merchant newMerchant)
    {
        storePanel.SetActive(true);
        storePanel.GetComponent<UIMenu>().active = true;
        myMerchant = newMerchant;
        storeName = myMerchant.myName + "'s " + myMerchant.merchandise;
        storeNameText.text = storeName;
        int quantity = newMerchant.goods.Count;
        for (int i = 0; i < quantity; i++)
        {
            GameObject newItem = Instantiate(storeItem);
            newItem.transform.SetParent(slots[i].transform);
            newItem.transform.position = newItem.transform.parent.position;
            newItem.GetComponent<Image>().sprite = newMerchant.goods[i].sprite;
            StoreItemData newItemData = newItem.GetComponent<StoreItemData>();
            newItemData.item = newMerchant.goods[i];
            newItemData.SetAmount(newMerchant.goodsQuantity[i]);
            newItemData.slotID = i;
            items.Add(newItemData);
        }
    }
    void FixedUpdate()
    {
        if(myMerchant != null)
        {
            if(storePanel.activeSelf == false)
            {
                myMerchant = null;
                items.Clear();
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].transform.childCount > 1)
                    {
                        Destroy(slots[i].transform.GetChild(1).gameObject);
                    }
                }
                DeactivateToolTip();
            }
        }
    }
    public void Deactivate()
    {
        storePanel.SetActive(false);
        storePanel.GetComponent<UIMenu>().active = false;
        myMerchant = null;
        items.Clear();
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount > 1)
            {
                Destroy(slots[i].transform.GetChild(1).gameObject);
            }
        }
    }

    public void ActivateToolTip(Vector3 position, StoreItemData clickedItem)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        currentItem = clickedItem;
    }
    public void DeactivateToolTip()
    {
        tooltip.SetActive(false);
        currentItem = null;
    }

    public void ActivateSellingToolTip(Vector3 position, ItemData clickedItem)
    {
        sellingtip.SetActive(true);
        sellingtip.transform.position = position;
        saleItem = clickedItem;
        Debug.Log(saleItem.item.Title);
    }
    public void DeactivateSellingToolTip()
    {
        sellingtip.SetActive(false);
        saleItem = null;
    }
    bool CheckIfIHaveEnoughSlots(Item item)
    {   
        for(int i =0; i< items.Count; i++)
        {
            if(items[i].item.ID == item.ID)
            {
                return true;
            }
        }
        if(slots.Count == items.Count)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    public void SellItem(int amt)
    {
        SellTransaction(amt, saleItem);
    }
    public void SellAll()
    {
        SellTransaction(inv.CheckHowManyIHave(saleItem.item.ID),saleItem);
    }
    public void SellTransaction(int amt, ItemData itemForSale)
    {
        DialogueManager.ShowAlert("Selling " + amt.ToString() + " " + itemForSale.item.Title);
        if(CheckIfIHaveEnoughSlots(itemForSale.item))
        {
            GameManager.Instance.gameData.data.gold += (int)(itemForSale.item.Value * amt * .5f);
            inv.RemoveItem(itemForSale.item.ID, amt);
            myMerchant.AddBoughtItem(itemForSale.item.ID, amt);
        }
    }
    public void BuyItem(int amt)
    {
        BuyTransaction(amt, currentItem);
    }
    public void BuyAll()
    {
        StoreItemData boughtItem = items[currentItem.slotID];
        BuyTransaction(boughtItem.amount, currentItem);
    }
    public void BuyAmount(Text amount)
    {
        int amt= 0;
        if(int.TryParse(amount.text, out amt))
        {
            BuyTransaction(amt, currentItem);
        }
    }

    public void BuyTransaction(int amt, StoreItemData purchasedItem)
    {
        StoreItemData boughtItem = items[purchasedItem.slotID];
        int slotsRequired = 1;
        if (!purchasedItem.item.Stackable)
            slotsRequired = amt;
        if (boughtItem.amount >= amt)
        {
            if (GameManager.Instance.gameData.data.gold > amt * purchasedItem.item.Value)
            {
                if (inv.CheckIfIHaveEnoughSlots(boughtItem.item,slotsRequired))
                {
                    GameManager.Instance.gameData.data.gold -= amt * purchasedItem.item.Value;
                    inv.AddItem(purchasedItem.item.ID, amt);
                    myMerchant.RemoveItem(purchasedItem.item.ID, amt);
                    DialogueManager.ShowAlert("You bought " + amt + " " + purchasedItem.item.Title);
                }
                else
                {
                    DialogueManager.ShowAlert("You can't hold all of this!");
                }
            }
            else
            {
                DialogueManager.ShowAlert("You do not have enough moneys!");
            }
        }
        else
        {
            if (GameManager.Instance.gameData.data.gold > boughtItem.amount * purchasedItem.item.Value)
            {
                if (slotsRequired == amt)
                    slotsRequired = boughtItem.amount;
                if (inv.CheckIfIHaveEnoughSlots(boughtItem.item,slotsRequired))
                {
                    GameManager.Instance.gameData.data.gold -= boughtItem.amount * purchasedItem.item.Value;
                    inv.AddItem(purchasedItem.item.ID, boughtItem.amount);
                    myMerchant.RemoveItem(purchasedItem.item.ID, boughtItem.amount);
                    DeactivateToolTip();
                    DialogueManager.ShowAlert("You bought " + boughtItem.amount + " " + purchasedItem.item.Title);
                }
                else
                {
                    DialogueManager.ShowAlert("You can't hold all of this!");
                }
            }
            else
            {
                DialogueManager.ShowAlert("You do not have enough moneys!");
            }
        }
    }
}
