using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Spawner
{
    [XmlElement("Biome")]
    public int biome { get; set; }

    [XmlElement("Resource")]
    public Resource[] resources
    {
        get
        {
            if (_resourceList == null)
            {
                _resourceList = new List<Resource>();
            }
            return _resourceList.ToArray();
        }

        set
        {
            if (_resourceList == null)
            {
                _resourceList = new List<Resource>();
            }

            if (value != null)
            {
                _resourceList.AddRange(value);
            }
        }
    }
    private List<Resource> _resourceList = new List<Resource>();

    
    public Spawner(Spawner other)
    {
        biome = other.biome;
        resources = (Resource[])other.resources.Clone();
    }
    public Spawner()
    {

    }
}
public class Resource
{
    [XmlAttribute("Name")]
    public string spawnName { get; set; }
    [XmlElement("Type")]
    public int type { get; set; }
    [XmlElement("Rate")]
    public int spawnRate { get; set; }
    [XmlElement("Max")]
    public int spawnMax { get; set; }
}
public class ResourceToSpawn
{
    public int type;
    public GameObject spawnPrefab;
    public float spawnRate;
    public float nextSpawn;
    public int spawnCount;
    public int spawnMax;
    public float spawnOffset;
    List<GameObject> spawnedObjects;
    public bool spawning;
    public Vector3 pos;
    public ResourceSpawner mySpawner;

    public void Intialize() 
    {
        spawnCount = 0;
        nextSpawn = Random.Range(0.0f, 1.0f);
        spawnedObjects = new List<GameObject>(spawnMax);
	}
    public void Update(float delta)
    {
        nextSpawn = nextSpawn - delta;
        if (nextSpawn < 0 && spawnCount < spawnMax)
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        RaycastHit hitInfo;
        Vector3 SpawnPos = pos + new Vector3(Random.Range(-spawnOffset,spawnOffset),0,Random.Range(-spawnOffset,spawnOffset));
        if (Physics.Raycast((SpawnPos + Vector3.up * 10f), Vector3.down, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject spawnedObject = mySpawner.SpawnResource(spawnPrefab,hitInfo.point);
                spawnedObject.SendMessage("SetSpawner", this);
                spawnCount++;
                nextSpawn = spawnRate;
            }
        }
    }
    public void SpawnedTreeDied(GameObject Tree)
    {
        spawnedObjects.Remove(Tree);
        spawnCount--;
    }
    public void StartSpawning()
    {
        spawning = true;
    }
    public void StopSpawning()
    {
        spawning = false;
    }
}


public class ResourceSpawner : MonoBehaviour
{
    ResourceDatabase database;
    List<ResourceToSpawn> resources;
    int biome;
    public bool inTown;

    public void Initalize(int id, float spawnOffset)
    {
        inTown = false;
        resources = new List<ResourceToSpawn>();
        biome = id;
        database = GameObject.Find("Inventory").GetComponent<Loader>().sc;
        Spawner spawn = database.FetchSpawnerByBiome(biome);
       foreach  (Resource resource in spawn.resources)
       {
           ResourceToSpawn rts = new ResourceToSpawn();
           rts.Intialize();
           rts.spawnRate = resource.spawnRate;
           rts.nextSpawn += rts.spawnRate;
           rts.spawnMax = resource.spawnMax;
           rts.type = resource.type;
           rts.spawning = false;
           string path = "Environment/";
           switch (rts.type)
           {
               case 1:
                   {
                       path += "Trees/";
                       path += resource.spawnName;
                       path += "/" + resource.spawnName + "Sapling"; 
                       break;
                   }
               case 2:
                   {
                       path += "Ores/";
                       path += resource.spawnName;
                       path += "/" + resource.spawnName + "Ore"; 
                       break;
                   }
           }
           rts.spawnPrefab = Resources.Load<GameObject>(path);
           rts.pos = transform.position;
           rts.mySpawner = this;
           rts.spawnOffset = spawnOffset;
           resources.Add(rts);
       }
    }

    void FixedUpdate()
    {
        if(!inTown)
        {
            if (resources != null)
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].spawning)
                    {
                        resources[i].Update(Time.deltaTime);
                    }
                }
            }
        }
    }
    public GameObject SpawnResource(GameObject spawnPrefab, Vector3 spawnPos)
    {
        return (GameObject)Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }

    void StartSpawning(int type)
    {
        if(resources != null)
        {
            foreach (ResourceToSpawn resource in resources)
            {
                if (resource.type == type || type == 0)
                {
                    resource.StartSpawning();
                }
            }
        }
    }
    void StopSpawning(int type)
    {
        foreach(ResourceToSpawn resource in resources)
        {
            if (resource.type == type || type == 0)
            {
                Debug.Log("Stop spawning");
                resource.StopSpawning();
            }
        }
    }
}
