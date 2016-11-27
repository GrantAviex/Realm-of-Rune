using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BuyingTooltip : MonoBehaviour, IPointerExitHandler
{
    StoreManager manager;
    void Start()
    {
        manager = GameObject.Find("Store Manager").GetComponent<StoreManager>();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(gameObject.name == "Buying Tooltip")
        {
            manager.DeactivateToolTip();
        }
        else
        {
            manager.DeactivateSellingToolTip();
        }
    }

}
