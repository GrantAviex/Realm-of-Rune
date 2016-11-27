using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    public float damage { get; set; }
    public float lifeTime { get; set; }
    public GameObject owner { get; set; }
    void Start()
    {
        lifeTime = 1.0f;
    }
    public void FixedUpdate()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

}
