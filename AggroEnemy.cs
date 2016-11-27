using UnityEngine;
using System.Collections;

public class AggroEnemy : Enemy
{
    public GameObject AggroZone;
    private SphereCollider AggroZoneTrigger;
    public float aggroRange;

	// Use this for initialization
	new void Start () 
    {
       AggroZoneTrigger = AggroZone.GetComponent<SphereCollider>();
       AggroZoneTrigger.radius = aggroRange / transform.localScale.x;
       base.Start();
	}
	
	// Update is called once per frame
	new void Update () 
    {
        base.Update();
	}
}
