using UnityEngine;
using System.Collections;

public class TreeCollect : MonoBehaviour 
{
    public GameObject woodPrefab;
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int dropAmount;
    int nextLogDrop;
    int health;
    public ResourceToSpawn mySpawner;
    public GameObject nextPhaseObj;
    void Start()
    {
        health = maxHealth;
        nextLogDrop = maxHealth / dropAmount;
    }
    void SpawnLog()
    {
        RaycastHit hitInfo;
        while (true)
        {
            Vector3 SpawnOffset = new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
            if(SpawnOffset.x <3 && SpawnOffset.x > 0)
            {
                Mathf.Clamp(SpawnOffset.x,3,6);
            }
            else if(SpawnOffset.x <=0 && SpawnOffset.x > -3)
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
                if(hitInfo.point.y <= transform.position.y + 0.24f)
                {
                    Instantiate(woodPrefab, hitInfo.point, Quaternion.identity);
                    break;  
                }
            }
        }
    }
    void ChopTree(int damage)
    {
        health -= damage;
        nextLogDrop -= damage;
        Debug.Log(nextLogDrop + " hits till next Log");
        if (nextLogDrop <= 0)
        {
            Debug.Log("Dropping log " + nextLogDrop + " hits till next Log");
            SpawnLog();
            nextLogDrop = maxHealth / dropAmount;
        }
        if(health <= 0)
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
