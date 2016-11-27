using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Item
{
    [XmlAttribute("name")]
    public string Name { get; set; }
    [XmlAttribute("Title")]
    public string Title { get; set; }

    [XmlElement("ID")]
    public int ID { get; set; }
    [XmlElement("Value")]
    public int Value { get; set; }
    [XmlElement("Equippable",IsNullable=true)]
    public Equipment Eqp { get; set; }
    [XmlElement("Useable", IsNullable = true)]
    public Useable Use { get; set; }
    [XmlElement("Stackable")]
    public bool Stackable { get; set; }
    [XmlElement("Description")]
    public ItemDescription Description { get; set; }

    public Sprite sprite { get; set; }
    public Item()
    {
    }
    public Item(Item other)
    {
        Name = other.Name;
        Title = other.Title;
        ID = other.ID;
        Value = other.Value;
        if(other.Eqp != null)
            Eqp = new Equipment(other.Eqp);
        if (other.Use != null)
            Use = new Useable(other.Use);
        Stackable = other.Stackable;
        Description = other.Description;
        #region spriteStuff
        string spritePath = "Sprites/";
        if(this.Eqp != null)
        {
            if(this.Eqp.Wep != null)
            {
                spritePath += "Weapons/";
            }
            if(this.Eqp.Arm != null)
            {
                switch (this.Eqp.Type)
                {
                    case 3:
                        {
                            spritePath += "Armors/";
                            break;
                        }
                    case 4:
                        {
                            spritePath += "Hats/";
                            //hat
                            break;
                        }
                    case 5:
                        {
                            spritePath += "Pants/";
                            //pants
                            break;
                        }
                    case 6:
                        {
                            spritePath += "Boots/";
                            //boots
                            break;
                        }
                    case 7:
                        {
                            spritePath += "Gloves/";
                            //gloves
                            break;
                        }
                    case 8:
                        {
                            spritePath += "Capes/";
                            //cape
                            break;
                        }
                    case 9:
                        {
                            spritePath += "Helmets/";
                            //helmets
                            break;
                        }
                    case 10:
                        {
                            spritePath += "Shields/";
                            //helmets
                            break;
                        }

                }
            }
            if(this.Eqp.Tool != null)
            {
                spritePath += "Tools/";
            }
        }
        else
        {
            spritePath += "Items/";
        }
        sprite = Resources.Load<Sprite>(spritePath + Name);
        #endregion
    }

}
public class ItemDescription
{
    [XmlAttribute("Description")]
    public string Description { get; set; }
}
public class LevelReq
{
    [XmlElement("Level")]
    public int level { get; set; }
    [XmlElement("Skill")]
    public int skill { get; set; }
}
public class Equipment
{
    [XmlElement("Rarity")]
    public int Rarity { get; set; }
    [XmlElement("Type")]
    public int Type { get; set; }
    [XmlElement("LevelReq")]
    public LevelReq[] LvlsReq
    {
        get
        {
            if (_levelReqs == null)
            {
                _levelReqs = new List<LevelReq>();
            }
            return _levelReqs.ToArray();
        }

        set
        {
            if (_levelReqs == null)
            {
                _levelReqs = new List<LevelReq>();
            }

            if (value != null)
            {
                _levelReqs.AddRange(value);
            }
        }
    }

    private List<LevelReq> _levelReqs = new List<LevelReq>();
    [XmlElement("Weapon",IsNullable=true)]
    public WeaponInfo Wep { get; set; }
    [XmlElement("Armor",IsNullable=true)]
    public ArmorInfo Arm { get; set; }

    [XmlElement("Tool", IsNullable=true)]
    public ToolInfo Tool { get; set; }
    public Equipment()
    {

    }

    public Equipment(Equipment other)
    {
        if(other != null)
        {
            Rarity = other.Rarity;
            Type = other.Type;
            LvlsReq = (LevelReq[])other.LvlsReq.Clone();
            Wep = other.Wep;
            Arm = other.Arm;
            Tool = other.Tool;
        }
    }
}
public class Useable
{
    [XmlElement("Effect")]
    public int Effect { get; set; }
    [XmlElement("Amount")]
    public int Amount { get; set; }

    public Useable()
    {

    }
    public Useable(Useable other)
    {
        if (other != null)
        {
            Effect = other.Effect;
            Amount = other.Amount;
        }
    }


}


public class WeaponInfo
{
    [XmlElement("AttackSpeed")]
    public int AttackSpeed { get; set; }
    [XmlElement("Damage")]
    public int Damage { get; set; }
    [XmlElement("MagicPower")]
    public int MagicPower { get; set; }
}
public class ArmorInfo
{
    [XmlElement("Defense")]
    public int Defense { get; set; }
    [XmlElement("MagicResist")]
    public int MagicResist { get; set; }
}
public class ToolInfo
{
    [XmlElement("Type")]
    public int Type { get; set; }
    [XmlElement("Tier")]
    public int Tier { get; set; }
}
