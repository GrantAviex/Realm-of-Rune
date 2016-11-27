using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlType(AnonymousType = true)]
[XmlRoot("ItemDatabase")]
public class ItemDatabase
{
    [XmlElement("Item")]
    public Item[] Items
    {
        get
        {
            if (_itemList == null)
            {
                _itemList = new List<Item>();
            }
            return _itemList.ToArray();
        }

        set
        {
            if (_itemList == null)
            {
                _itemList = new List<Item>();
            }

            if (value != null)
            {
                _itemList.AddRange(value);
            }
        }
    }
    private List<Item> _itemList = new List<Item>();
    public List<Item> database = new List<Item>();
    public static ItemDatabase Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));

        using(StringReader reader = new StringReader(_xml.text))
        {
            return serializer.Deserialize(reader) as ItemDatabase;
        }

    }
    public void ConstructItemDatabase()
    {
        foreach(Item item in Items)
        {
            database.Add(new Item(item));
        }
    }
    public Item FetchItemByID(int id)
    {
        foreach (Item item in database)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }
    public Item FetchItemByName(string name)
    {
        foreach (Item item in database)
            if (item.Name == name)
                return item;
        return null;
    }
}
