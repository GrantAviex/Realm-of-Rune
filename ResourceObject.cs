using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceObject : MonoBehaviour 
{
    public Text resourceInfoText;
    public Text actionInfoText;
    public GameObject Canvas;
    public GameObject player;
    public int Type;
    public int Tier;

    void Start()
    {
        Canvas = transform.GetChild(0).gameObject;
        Create();
    }

    void FixedUpdate()
    {
        if(player != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pressed E");
                player.SendMessage("Collect", this);
            }
        }
    }
    void Create()
    {
        string action = "";
        string stat = "UnsetStat";
        switch (Type)
        {
            case (int)Tools.FishingRod:
                {
                    action += "Fish(E)";
                    stat = "Fisherman";
                    break;
                }
            case (int)Tools.Axe:
                {
                    action += "Chop(E)";
                    stat = "Woodcutter";
                    break;
                }
            case (int)Tools.Pickaxe:
                {
                    action += "Mine(E)";
                    stat = "Miner";
                    break;
                }
            case (int)Tools.ButchersKnife:
                {
                    action += "Skin(E)";
                    stat = "Skinner";
                    break;
                }
            case (int)Tools.Bolas:
                {
                    action += "Capture(E)";
                    stat = "Hunter";
                    break;
                }
        }
        actionInfoText.text = action;

        string resourceInfo = "Unset Tier";
        switch (Tier)
        {
            case 1:
                {
                    resourceInfo = "Noob ";
                    break;
                }
            case 2:
                {
                    resourceInfo = "Apprentice ";
                    break;
                }
            case 3:
                {
                    resourceInfo = "Novice ";
                    break;
                }
            case 4:
                {
                    resourceInfo = "Journeyman ";
                    break;
                }
            case 5:
                {
                    resourceInfo = "Expert ";
                    break;
                }
            case 6:
                {
                    resourceInfo = "Master ";
                    break;
                }
            case 7:
                {
                    resourceInfo = "Grand Master ";
                    break;
                }
        }
        resourceInfo += stat + " required";
        resourceInfoText.text = resourceInfo;
        Canvas.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            Canvas.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
            Canvas.SetActive(false);
        }
    }
}
