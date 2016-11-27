using UnityEngine;
using System.Collections;

public class PickableItem : MonoBehaviour 
{
    public int id;
    private Item item;
    private Inventory inv;
    private DropTip dropTip;
    bool ableToPickup;
	void Start ()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        dropTip = inv.GetComponent<DropTip>();
        ableToPickup = false;
	}
	
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(ableToPickup)
            {
                inv.AddItem(id, 1);
                dropTip.Deactivate();
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ableToPickup = true;
            if( item == null)
            {
                item = inv.database.FetchItemByID(id);
            }
            dropTip.Activate(item);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ableToPickup = false;
            dropTip.Deactivate();
        }
    }
}
