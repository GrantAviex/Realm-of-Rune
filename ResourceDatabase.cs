using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[XmlType(AnonymousType = true)]
[XmlRoot("ResourceDatabase")]
public class ResourceDatabase
{
    [XmlElement("Spawner")]
    public Spawner[] spawners
    {
        get
        {
            if (_spawnerList == null)
            {
                _spawnerList = new List<Spawner>();
            }
            return _spawnerList.ToArray();
        }

        set
        {
            if (_spawnerList == null)
            {
                _spawnerList = new List<Spawner>();
            }

            if (value != null)
            {
                _spawnerList.AddRange(value);
            }
        }
    }
    private List<Spawner> _spawnerList = new List<Spawner>();
    public List<Spawner> database = new List<Spawner>();
    public static ResourceDatabase Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(ResourceDatabase));

        using (StringReader reader = new StringReader(_xml.text))
        {
            return serializer.Deserialize(reader) as ResourceDatabase;
        }

    }
    public void ConstructResourceDatabase()
    {
        foreach (Spawner spawner in spawners)
        {
            database.Add(new Spawner(spawner));
        }
    }
    public Spawner FetchSpawnerByBiome(int biome)
    {
        foreach (Spawner spawner in database)
        {
            if (spawner.biome == biome)
            {
                return spawner;
            }
        }
        return null;
    }
}
