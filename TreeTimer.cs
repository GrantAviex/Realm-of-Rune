using UnityEngine;
using System.Collections;

public class TreeTimer : MonoBehaviour
{
    public TreeSpawner mySpawner;
    public float lifeTimer;
    float currLife;
    public GameObject nextPhaseObj;
	void Start () 
    {
        currLife = lifeTimer;
	}
	
	void FixedUpdate () 
    {
        currLife -= Time.deltaTime;
        if(currLife < 0)
        {
            if(nextPhaseObj != null)
            {
                GameObject nextObj = (GameObject)Instantiate(nextPhaseObj, transform.position, Quaternion.identity);
                nextObj.SendMessage("SetSpawner", mySpawner);
                Destroy(gameObject);
            }
            else
            {
                mySpawner.SpawnedTreeDied(gameObject);
                Destroy(gameObject);
            }
        }
	}
    void SetSpawner(TreeSpawner spawner)
    {
        mySpawner = spawner;
    }
}
