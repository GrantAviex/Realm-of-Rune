using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CraftingStation : MonoBehaviour 
{
    public int myStationID;
    public Station myStation;
    RecipeDatabase database;
    bool playerInRange = false;
    public GameObject interactTooltipPrefab;
    private GameObject myTooltip;
    public GameObject myManagerPrefab;
    private CraftingManager myManager = null;

	void Start () 
    {
        Invoke("WhatsMyStation", .1f);
        GameObject.Find("Interact Tooltip");
	}
	void WhatsMyStation()
    {
        database = GameObject.Find("Inventory").GetComponent<Loader>().rc;
        myStation = database.FetchStationByID(myStationID);
    }
    void FixedUpdate()
    {
        if(playerInRange && myManager == null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                myManager = Instantiate(myManagerPrefab).GetComponentInChildren<CraftingManager>();
                myManager.LoadStation(this);
            }
        }
        if(myManager != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Destroy!");
                Destroy(myManager.gameObject.transform.root.gameObject);
                myManager = null;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = true;
            myTooltip = Instantiate(interactTooltipPrefab);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            Destroy(myTooltip);
            myTooltip = null;
            if (myManager != null)
            {
                Destroy(myManager.gameObject.transform.root.gameObject);
                myManager = null;
            }
        }
    }
}
