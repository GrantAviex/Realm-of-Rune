using UnityEngine;
using System.Collections;

public class ItemInfo
{

    string info;

    public ItemInfo()
    {
        info = "";
    }
    public string Construct(Item item)
    {
        if(item.Eqp != null)
        {
            switch (item.Eqp.Rarity)
            {
                case 1:
                    info = "<b>" + item.Title + "</b> \n";
                    break;
                case 2:
                    info = "<b><color=#2eb000>" + item.Title + "</color></b> \n";
                    break;
                case 3:
                    info = "<b><color=##3578ff>" + item.Title + "</color></b> \n";
                    break;
                case 4:
                    info = "<b><color=##8732c8>" + item.Title + "</color></b> \n";
                    break;
                case 5:
                    info = "<b><color=##ffb425>" + item.Title + "</color></b> \n";
                    break;
                case 6:
                    info = "<b><color=##ff9615>" + item.Title + "</color></b> \n";
                    break;
                case 7:
                    info = "<b><color=##cd1717>" + item.Title + "</color></b> \n";
                    break;
            }
        }
        else
        {
            info = "<b>" + item.Title + "</b> \n";
        }
        if (item.Eqp != null)
        {
            switch (item.Eqp.Type)
            {
                case 0:
                    info += "One-Handed\n";
                    break;
                case 1:
                    info += "Two-Handed\n";
                    break;
                case 2:
                    info += "Ranged\n";
                    break;
                case 3:
                    info += "Armor\n";
                    break;
                case 4:
                    info += "Hat\n";
                    break;
                case 5:
                    info += "Pants\n";
                    break;
                case 6:
                    info += "Boots\n";
                    break;
                case 7:
                    info += "Gloves\n";
                    break;
                case 8:
                    info += "Cape\n";
                    break;
                case 9:
                    info += "Helmet\n";
                    break;
                case 10:
                    info += "Shield\n";
                    break;
                case 11:
                    info += "Thrown\n";
                    break;
            }
            if(item.Eqp.Tool != null)
            {
                switch (item.Eqp.Tool.Type)
                {
                    case (int)Tools.FishingRod:
                        {
                            info += "FishingRod\n";
                            break;
                        }
                    case (int)Tools.Axe:
                        {
                            info += "Axe\n";
                            break;
                        }
                    case (int)Tools.Pickaxe:
                        {
                            info += "Pickaxe\n";
                            break;
                        }
                    case (int)Tools.ButchersKnife:
                        {
                            info += "HuntersKnife\n";
                            break;
                        }
                    case (int)Tools.Bolas:
                        {
                            info += "Bolas\n";
                            break;
                        }
                }
            }
            if (item.Eqp.Wep != null)
            {
                info += item.Eqp.Wep.Damage.ToString() + " damage \n";
                info += item.Eqp.Wep.MagicPower.ToString() + " magic power \n";
            }
            if (item.Eqp.Arm != null)
            {
                info += item.Eqp.Arm.Defense.ToString() + " defense \n";
                info += item.Eqp.Arm.MagicResist.ToString() + "Magic Resist \n";
            }
        }
        info += item.Description.Description;
        return info;
    }
}
