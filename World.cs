using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{
    public townInfo[,] towns;
    public Biome[,] world;
    public bool[,] chunks;
    public int size;
    public int distCheck;
    public int[] biomeCount;
    public float biomeSize;
    public float biomeScale;
    public GameObject biomePrefab;
    public GameObject terrainPrefab;
    public GameObject chunkPrefab;
    public int chunkSize;
    private int chunkAmt;
    int biomeID = 7;
    uint biomeCounter = 0;
    
    // Use this for initialization
    void Start()
    {
        towns = new townInfo[size, size];
        world = new Biome[size, size];
        chunkAmt = size / chunkSize;
        chunks = new bool[chunkAmt,chunkAmt];
    }

    void Create()
    {
        GenerateBiomes();
        Invoke("DelayStart", 0.1f);
    }
    public void Load(WorldData data)
    {
        if(data.create)
        {
            Create();
            return;
        }
        size = data.size;
        biomeScale = data.biomeScale;
        biomeSize = data.biomeSize;
        chunkSize = data.chunkSize;
        towns = data.towns;
        world = new Biome[size, size];
        LoadChunks(data);
        LoadTowns(data);
    }
    void LoadChunks(WorldData data)
    {
        for (int i = 0; i < size/chunkSize; i++)
        {
            for (int j = 0; j < size/chunkSize; j++)
            {
                GameObject chunk = Instantiate(chunkPrefab);
                chunk.transform.parent = transform;
                chunk.transform.position = new Vector3(i * biomeSize * chunkSize + (1.5f* biomeSize), 0, j * biomeSize * chunkSize + (1.5f*biomeSize));
                chunks[i, j] = true;
                int id = data.world[i * chunkSize, j * chunkSize].id;
                #region chunkNaming
                switch (id)
                {
                    case 0:
                        {
                            chunk.name = "Ocean";
                            break;
                        }
                    case 1:
                        {
                            chunk.name = "Coast";
                            break;
                        }
                    case 2:
                        {
                            chunk.name = "Desert";
                            break;
                        }
                    case 3:
                        {
                            chunk.name = "Plains";
                            break;
                        }
                    case 4:
                        {
                            chunk.name = "Forest";
                            break;
                        }
                    case 5:
                        {
                            chunk.name = "Hills";
                            break;
                        }
                    case 6:
                        {
                            chunk.name = "Wasteland";
                            break;
                        }
                    case 7:
                        {
                            chunk.name = "Tundra";
                            break;
                        }
                    case 8:
                        {
                            chunk.name = "Mountains";
                            break;
                        }
                    case 9:
                        {
                            chunk.name = "Island";
                            break;
                        }
                }
                #endregion
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        LoadBiome(i * chunkSize + x, j * chunkSize + y, chunk,data);
                    }
                }
            }
            
        }
    }
    void LoadBiome(int i, int j, GameObject myChunk, WorldData data)
    {
        int biomeInstance = data.world[i, j].instance;
        int id = data.world[i, j].id;
        string path = "Biomes/";
        path += id + "/";
        path += biomeInstance;
        GameObject biome = Resources.Load<GameObject>(path);
        GameObject newBiome = Instantiate(biomePrefab);
        GameObject terrain = Instantiate(terrainPrefab);
        newBiome.transform.parent = myChunk.transform;
        terrain.GetComponent<MeshFilter>().sharedMesh = biome.GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshRenderer>().materials = new Material[1];
        terrain.GetComponent<MeshRenderer>().sharedMaterial = biome.GetComponent<MeshRenderer>().sharedMaterial;
        terrain.GetComponent<MeshCollider>().sharedMesh = biome.GetComponent<MeshFilter>().sharedMesh;
        terrain.transform.parent = newBiome.transform;
        terrain.transform.localPosition = Vector3.zero;
        terrain.transform.localScale = Vector3.one * biomeScale;
        newBiome.transform.Rotate(Vector3.up, data.world[i,j].rot);
        Biome spawnBiome = newBiome.GetComponent<Biome>();
        spawnBiome.id = id;
        spawnBiome.locationX = i;
        spawnBiome.locationY = j;
        spawnBiome.world = this;
        spawnBiome.instance = biomeInstance;
        spawnBiome.myID = biomeCounter++;
        path = "Towns/" + 4 + "/Town";
        spawnBiome.TownPrefab = Resources.Load<GameObject>(path);
        Vector3 biomePos = new Vector3(i * biomeSize, 0, j * biomeSize);
        newBiome.transform.position = biomePos;
        Vector3 spawnPoint = terrain.GetComponent<Renderer>().bounds.center;
        spawnPoint.y = 0;
        spawnBiome.spawnPoint = spawnPoint;
        ResourceSpawner resourceSpawner = newBiome.GetComponent<ResourceSpawner>();
        resourceSpawner.Initalize(spawnBiome.id, biomeSize * 0.9f);
        world[i, j] = spawnBiome;
    }
    void LoadTowns(WorldData data)
    {
        Debug.Log(towns.Length);
        Debug.Log(size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(towns[i,j].exists == 1)
                {
                    Biome biome = world[i, j];
                    biome.myTown = (GameObject)Instantiate(biome.TownPrefab, biome.spawnPoint, Quaternion.identity);
                    biome.myTown.transform.parent = biome.transform;
                    float rot = towns[i, j].rot;
                    biome.myTown.transform.Rotate(Vector3.up, rot);
                    biome.GetComponent<ResourceSpawner>().inTown = true;
                }
            }
        }
    }
    public void Save(WorldData data)
    {
        data.create = false;
        data.size = size;
        data.biomeScale = biomeScale;
        data.biomeSize = biomeSize;
        data.chunkSize = chunkSize;
        Debug.Log("Saving world");
        data.towns = towns;
        data.world = new biomeInfo[size, size];
        for(int i=0; i< size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                data.world[i, j].id = world[i, j].id;
                data.world[i, j].rot = world[i, j].transform.rotation.y;
                data.world[i, j].instance = world[i, j].instance;
            }
        }
    }
    void DelayStart()
    {
        float waitTime = (float)(size) / 10;
        Invoke("GenerateTowns", waitTime);
    }
    void GenerateBiomes()
    {
        CreateChunk(0, 0);
    }
    void CreateChunk(int i, int j)
    {   
        if (i >= chunkAmt)
            return;
        if (j >= chunkAmt)
            return;

        if(!chunks[i,j])
        {
            GameObject chunk = Instantiate(chunkPrefab);
            chunk.transform.parent = transform;
            chunk.transform.position = new Vector3(i * biomeSize * chunkSize + (1.5f* biomeSize), 0, j * biomeSize * chunkSize + (1.5f*biomeSize));
            chunks[i, j] = true;
            #region chunkNaming
            switch (biomeID)
            {
                case 0:
                    {
                        chunk.name = "Ocean";
                        break;
                    }
                case 1:
                    {
                        chunk.name = "Coast";
                        break;
                    }
                case 2:
                    {
                        chunk.name = "Desert";
                        break;
                    }
                case 3:
                    {
                        chunk.name = "Plains";
                        break;
                    }
                case 4:
                    {
                        chunk.name = "Forest";
                        break;
                    }
                case 5:
                    {
                        chunk.name = "Hills";
                        break;
                    }
                case 6:
                    {
                        chunk.name = "Wasteland";
                        break;
                    }
                case 7:
                    {
                        chunk.name = "Tundra";
                        break;
                    }
                case 8:
                    {
                        chunk.name = "Mountains";
                        break;
                    }
                case 9:
                    {
                        chunk.name = "Island";
                        break;
                    }
            }
            #endregion
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    CreateBiome(i * chunkSize + x, j * chunkSize + y, chunk);
                }
            }
            if (i + 1 < chunkAmt)
            {
                StartCoroutine(DelayChunkCreation(i + 1, j));
            }
            if (j + 1 < chunkAmt)
            {
                StartCoroutine(DelayChunkCreation(i, j + 1));
            }
        }
    }
    
    IEnumerator DelayChunkCreation(int i, int j)
    {
        yield return new WaitForSeconds(0.0001f);
        CreateChunk(i, j);
        
    }

    void CreateBiome(int i, int j, GameObject myChunk)
    {
        int biomeInstance = Random.Range(0, biomeCount[biomeID]);
        string path = "Biomes/";
        path += biomeID + "/";
        path += biomeInstance;
        GameObject biome = Resources.Load<GameObject>(path);
        GameObject newBiome = Instantiate(biomePrefab);
        GameObject terrain = Instantiate(terrainPrefab);
        newBiome.transform.parent = myChunk.transform;
        terrain.GetComponent<MeshFilter>().sharedMesh = biome.GetComponent<MeshFilter>().sharedMesh;
        terrain.GetComponent<MeshRenderer>().materials = new Material[1];
        terrain.GetComponent<MeshRenderer>().sharedMaterial = biome.GetComponent<MeshRenderer>().sharedMaterial;
        terrain.GetComponent<MeshCollider>().sharedMesh = biome.GetComponent<MeshFilter>().sharedMesh;    
        terrain.transform.parent = newBiome.transform;
        terrain.transform.localPosition = Vector3.zero;
        terrain.transform.localScale = Vector3.one * biomeScale;
        int rot = Random.Range(0, 4);
        newBiome.transform.Rotate(Vector3.up, 90 * rot);
        Biome spawnBiome = newBiome.GetComponent<Biome>();
        spawnBiome.id = biomeID;
        spawnBiome.locationX = i;
        spawnBiome.locationY = j;
        spawnBiome.world = this;
        spawnBiome.instance = biomeInstance;
        spawnBiome.myID = biomeCounter++;
        path = "Towns/" + 4 + "/Town";
        spawnBiome.TownPrefab = Resources.Load<GameObject>(path);
        Vector3 biomePos = new Vector3(i * biomeSize, 0, j * biomeSize);
        newBiome.transform.position = biomePos;
        Vector3 spawnPoint = terrain.GetComponent<Renderer>().bounds.center;
        spawnPoint.y = 0;
        spawnBiome.spawnPoint = spawnPoint;
        ResourceSpawner resourceSpawner = spawnBiome.GetComponent<ResourceSpawner>();
        resourceSpawner.Initalize(spawnBiome.id, biomeSize * 0.9f);
        world[i, j] = spawnBiome;
    }
    void GenerateTowns()
    {
        //CheckForNearbyTowns(size/2, size/2, 4);
        foreach (Biome biome in world)
        {
            int rand= Random.Range(0, 10);
            bool townCheck = CheckForNearbyTowns(biome.locationX,biome.locationY,biome.id);
            if (!townCheck && rand < 3)
            {
                towns[biome.locationX, biome.locationY].exists = 1;
                biome.myTown = (GameObject)Instantiate(biome.TownPrefab, biome.spawnPoint, Quaternion.identity);
                biome.myTown.transform.parent = biome.transform;
                int rot = Random.Range(0, 4);
                biome.myTown.transform.Rotate(Vector3.up, 90 * rot);
                towns[biome.locationX, biome.locationY].rot = rot * 90;
                biome.GetComponent<ResourceSpawner>().inTown = true;
            }
            else
            {
                towns[biome.locationX, biome.locationY].exists = 0;
                towns[biome.locationX, biome.locationY].rot = 0;
            }
        }

    }
    bool CheckForNearbyTowns(int x, int y, int id)
    {
        if (RecursiveCheckForTowns(x + 1, y, id, x, y, distCheck))
            return true;
        if (RecursiveCheckForTowns(x - 1, y, id, x, y, distCheck))
            return true;
        if (RecursiveCheckForTowns(x, y + 1, id, x, y, distCheck))
            return true;
        if (RecursiveCheckForTowns(x, y - 1, id, x, y, distCheck))
            return true;

        return false;
    }
    bool RecursiveCheckForTowns(int x, int y, int id, int startX, int startY, int dist)
    {
        if (x < 0 || x >= size || y < 0 || y >= size || (x == startX && y == startY) || dist <= 0)
            return false;
        if (RecursiveCheckForTowns(x + 1, y, id, x, y, dist-1))
            return true;
        if (RecursiveCheckForTowns(x - 1, y, id, x, y, dist-1))
            return true;
        if (RecursiveCheckForTowns(x, y + 1, id, x, y, dist-1))
            return true;
        if (RecursiveCheckForTowns(x, y - 1, id, x, y, dist-1))
            return true;
        if (world[x, y] == null)
            Debug.Log("biome doesn't exist at x:" + x + " y:" + y);
        if(world[x,y].id == id)
        {
            if(world[x,y].myTown != null)
            {
                Destroy(world[x, y].transform.GetChild(0).gameObject);
                return true;
            }
        }
        return false;
    }
        
}
