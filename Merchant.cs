using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Merchant : MonoBehaviour 
{
    public string myName;
    public string merchandise;
    public List<Item> goods = new List<Item>();
    public List<int> goodsQuantity = new List<int>();
    public StoreManager manager;
    Loader itemData;
	// Use this for initialization
	void Start () 
    {
        manager = GameObject.Find("Store Manager").GetComponent<StoreManager>();
        itemData = GameObject.Find("Inventory").GetComponent<Loader>();
        int amt = Random.Range(1, 65);
        for(int i =0; i< amt; i++)
        {
            int id = Random.Range(1, 29);
            int quantity = Random.Range(1, 100);
            AddItem(id, quantity);
        }
        AddItem(7, 600);
	}
	public void AddItem(int id, int quantity)
    {
        for(int i = 0; i< goods.Count; i++)
        {
            if(goods[i].ID == id)
            {
                goodsQuantity[i] += quantity;
                return;
            }
        }
        goods.Add(new Item(itemData.ic.FetchItemByID(id)));
        goodsQuantity.Add(quantity);
    }
    public void AddBoughtItem(int id, int quantity)
    {
        AddItem(id, quantity);
        UpdateShop();
    }
    public void RemoveItem(int id, int quantity)
    {
        for (int i = 0; i < goods.Count; i++)
        {
            if (goods[i].ID == id)
            {
                goodsQuantity[i] -= quantity;
                if(goodsQuantity[i] <= 0)
                {
                    goods.RemoveAt(i);
                    goodsQuantity.RemoveAt(i);
                }
                UpdateShop();
                return;
            }
        }
    }
    public void UpdateShop()
    {
        manager.Deactivate();
        manager.Activate(this);
    }
    void FixedUpdate()
    {

    }
    void OpenShop()
    {
        manager.Activate(this);
    }
    void CloseShop()
    {
        manager.Deactivate();
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            CloseShop();
        }
    }
}
