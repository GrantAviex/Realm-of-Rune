using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlType(AnonymousType = true)]
[XmlRoot("RecipeDatabase")]
public class RecipeDatabase 
{
    [XmlElement("Station")]
    public Station[] stations
    {
        get
        {
            if (_stationList == null)
            {
                _stationList = new List<Station>();
            }
            return _stationList.ToArray();
        }

        set
        {
            if (_stationList == null)
            {
                _stationList = new List<Station>();
            }

            if (value != null)
            {
                _stationList.AddRange(value);
            }
        }
    }
    private List<Station> _stationList = new List<Station>();
    public List<Station> database = new List<Station>();
    public static RecipeDatabase Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(RecipeDatabase));

        using(StringReader reader = new StringReader(_xml.text))
        {
            return serializer.Deserialize(reader) as RecipeDatabase;
        }

    }
    public void ConstructRecipeDatabase()
    {
        foreach (Station station in stations)
        {
            database.Add(new Station(station));
        }
    }
    public Station FetchStationByID(int id)
    {
        foreach (Station station in database)
        {
            if (station.ID == id)
            {
                return station;
            }
        }
        return null;
    }
    public Station FetchStationByName(string name)
    {
        foreach (Station station in database)
            if (station.Name == name)
                return station;
        return null;
    }
}
