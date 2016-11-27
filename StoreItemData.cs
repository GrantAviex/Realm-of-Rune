using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreItemData : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Item item;
    public int amount;
    public int slotID;
    public Text StackText;
    private Inventory inv;
    private Tooltip tooltip;
    StoreManager myManager;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<Tooltip>();
        StackText = GetComponentInChildren<Text>();
        myManager = gameObject.transform.root.GetComponent<StoreManager>();
    }

    void Update()
    {

    }
    public void SetAmount(int _amount)
    {
        if(_amount == 0)
        {
            Destroy(gameObject);
        }
        if(StackText == null)
        {
            StackText = transform.GetChild(0).GetComponent<Text>();
        }
        amount = _amount;
        StackText.text = amount.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item, true,eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            myManager.ActivateToolTip(eventData.position,this);
        }
    }
}
