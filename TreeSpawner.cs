using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour 
{
    public GameObject TreePrefab;
    public float spawnRate;
    public float nextSpawn;
    public int spawnCount;
    public int spawnMax;
    public float spawnOffset;
    List<GameObject> spawnedTrees;
    bool spawning = false;

	// Use this for initialization
	void Start () 
    {
        nextSpawn = spawnRate;
        spawnedTrees = new List<GameObject>(spawnMax);
	}
    void FixedUpdate()
    {
        nextSpawn -= Time.deltaTime;
        if (nextSpawn < 0 && spawnCount < spawnMax && spawning)
        {
            Spawn();
        }
    }
    void Spawn()
    {
        RaycastHit hitInfo;
        Vector3 SpawnPos = transform.position + new Vector3(Random.Range(-spawnOffset,spawnOffset),0,Random.Range(-spawnOffset,spawnOffset));
        if (Physics.Raycast((SpawnPos + Vector3.up * 10f), Vector3.down, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject spawnedTree = (GameObject)Instantiate(TreePrefab, hitInfo.point, Quaternion.identity);
                spawnedTree.SendMessage("SetSpawner", this);
                spawnCount++;
                nextSpawn = spawnRate;
            }
        }
    }
    public void SpawnedTreeDied(GameObject Tree)
    {
        spawnedTrees.Remove(Tree);
        spawnCount--;
    }
    void StartSpawning()
    {
        spawning = true;
    }
    void StopSpawning()
    {
        spawning = false;
    }
}

