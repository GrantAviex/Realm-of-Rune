using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public float spawnRate;
    public float nextSpawn;
    public int spawnCount;
    public int spawnMax;
    public float spawnOffset;
    List<GameObject> spawnedMobs;

	// Use this for initialization
	void Start () 
    {
        spawnOffset = gameObject.GetComponent<SphereCollider>().radius * transform.localScale.x;
        nextSpawn = spawnRate;
        spawnedMobs = new List<GameObject>(spawnMax);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (nextSpawn < 0 && spawnCount < spawnMax)
        {
            Spawn();
        }
	}
    void FixedUpdate()
    {
        nextSpawn -= Time.deltaTime;
    }
    void Spawn()
    {
        RaycastHit hitInfo;
        Vector3 SpawnPos = transform.position + new Vector3(Random.Range(-spawnOffset,spawnOffset),0,Random.Range(-spawnOffset,spawnOffset));
        if (Physics.Raycast((SpawnPos + Vector3.up * 10f), Vector3.down, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                GameObject spawnedMob = (GameObject)Instantiate(EnemyPrefab, hitInfo.point, Quaternion.identity);
                spawnedMob.GetComponent<Enemy>().mySpawner = this;
                spawnedMob.GetComponentInChildren<UnitStats>().mySpawner = this;
                spawnCount++;
                nextSpawn = spawnRate;
            }
        }
    }
    public void SpawnedMobDied(GameObject mob)
    {
        //Debug.Log(EnemyPrefab.name + " died");
        spawnedMobs.Remove(mob);
        spawnCount--;
    }
}
