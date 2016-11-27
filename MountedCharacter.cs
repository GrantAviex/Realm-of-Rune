using UnityEngine;
using System.Collections;

public class MountedCharacter : MonoBehaviour 
{
    public GameObject character;
    public GameObject Saddle;
    public GameObject Ride;
    public GameObject ConnectionPoint;
    public bool flying = false;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Saddle.transform.rotation = ConnectionPoint.transform.rotation;
        if (flying)
            Saddle.transform.position = ConnectionPoint.transform.position;
    }
}
