using UnityEngine;
using System.Collections;

public class MinimapFog : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
