using UnityEngine;
using System.Collections;

public class AggroRange : MonoBehaviour 
{
    AggroEnemy myAggro;
	// Use this for initialization
	void Start () 
    {
        myAggro = transform.parent.GetComponent<AggroEnemy>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            myAggro.SetTarget(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myAggro.SetTarget(null);
        }
    }
}
