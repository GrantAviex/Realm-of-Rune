using UnityEngine;
using System.Collections;

public class MinimapFollow : MonoBehaviour
{
    public GameObject player;
    public float followDistance;
    public bool rotate;
    public Vector3 rotateOffset;
	// Use this for initialization
	void Start () 
    {
        rotateOffset = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = player.transform.position + (Vector3.up * followDistance);
        if(rotate)
        {
            transform.localEulerAngles = player.transform.localEulerAngles + rotateOffset;
        }
	}
}
