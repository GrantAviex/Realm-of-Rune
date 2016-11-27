using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour 
{
    private Item item;
    private string data;
    public GameObject tooltip;
    public Text tooltipText;
    public GameObject reqPanel;
    public RectTransform textPanel;
    void Start()
    {
        tooltip.SetActive(false);
    }

    void FixedUpdate()
    {
        if(tooltip.activeSelf)
            reqPanel.transform.localPosition = Vector3.up * -textPanel.rect.height;
    }
    public void Activate(Item item, bool store,Vector3 pos)
    {
        tooltip.transform.position = pos;
        this.item = item;
        ConstructDataString(store);
        tooltip.SetActive(true);
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void ConstructDataString(bool store)
    {
        ItemInfo info = new ItemInfo();
        data = info.Construct(item);
        if (store)
        {
            data += "\nPrice: " + item.Value.ToString() + " gold";
        }
        int border = 0;
        for (int i = 0; i < 3; i ++ )
        {
            for (int j = 0; j < reqPanel.transform.GetChild(i).childCount; j++)
			{
                reqPanel.transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
			}
        }
        if(item.Eqp != null)
        {
            foreach (LevelReq req in item.Eqp.LvlsReq)
            {
                reqPanel.transform.GetChild(border).GetComponent<Requirement>().SetSprites(req);
                border++;
            }
            reqPanel.SetActive(true);
        }
        else
        {
            reqPanel.SetActive(false);
        }
        tooltipText.text = data;
    }
}