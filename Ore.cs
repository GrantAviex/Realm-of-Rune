using UnityEngine;
using System.Collections;

public class Ore : MonoBehaviour 
{

    public GameObject orePrefab;
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int dropAmount;
    int nextOreDrop;
    int health;
    public GameObject nextPhaseObj;
    public ResourceToSpawn mySpawner;
    void Start()
    {
        health = maxHealth;
        nextOreDrop = maxHealth / dropAmount;
    }

    void SpawnOre()
    {
        RaycastHit hitInfo;
        while (true)
        {
            Vector3 SpawnOffset = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
            if (SpawnOffset.x < 3 && SpawnOffset.x > 0)
            {
                Mathf.Clamp(SpawnOffset.x, 3, 6);
            }
            else if (SpawnOffset.x <= 0 && SpawnOffset.x > -3)
            {
                Mathf.Clamp(SpawnOffset.x, -6, -3);
            }
            if (SpawnOffset.z < 3 && SpawnOffset.z > 0)
            {
                Mathf.Clamp(SpawnOffset.z, 3, 6);
            }
            else if (SpawnOffset.z <= 0 && SpawnOffset.z > -3)
            {
                Mathf.Clamp(SpawnOffset.z, -6, -3);
            }
            Vector3 SpawnPos = transform.position + SpawnOffset * 0.12f;
            if (Physics.Raycast((SpawnPos + Vector3.up * 10f), Vector3.down, out hitInfo))
            {
                if (hitInfo.point.y <= transform.position.y + 0.24f)
                {
                    Instantiate(orePrefab, hitInfo.point, Quaternion.identity);
                    break;
                }
            }
        }
    }
    void MineOre(int damage)
    {
        health -= damage;
        nextOreDrop -= damage;
        if (nextOreDrop <= 0)
        {
            Debug.Log("Dropping log " + nextOreDrop + " hits till next Log");
            SpawnOre();
            nextOreDrop = maxHealth / dropAmount;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
            if (nextPhaseObj != null)
            {
                GameObject nextObj = (GameObject)Instantiate(nextPhaseObj, transform.position, Quaternion.identity);
                nextObj.SendMessage("SetSpawner", mySpawner);
                Destroy(gameObject);
            }
        }
    }
    void SetSpawner(ResourceToSpawn spawner)
    {
        mySpawner = spawner;
    }
}
