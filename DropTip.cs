using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DropTip : MonoBehaviour {

    private Item item;
    private string data;
    private GameObject tooltip;

    void Start()
    {
        tooltip = GameObject.Find("Droptip");
        tooltip.SetActive(false);
    }

    void Update()
    {
    }

    public void Activate(Item item)
    {
        this.item = item;
        ConstructDataString();
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString()
    {
        ItemInfo info = new ItemInfo();
        data = info.Construct(item);
        tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
    }
}