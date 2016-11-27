using UnityEngine;
using System.Collections;

public class ProjectileBlast : Projectile
{
    Rigidbody m_rigidBody;
    public string enemyName;
    public float enlargeTime;
    float currentEnlarge;

    void Start()
    {
        currentEnlarge = enlargeTime;
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
    }

    void Update()
    {
        if (currentEnlarge > 0)
            currentEnlarge -= Time.deltaTime;
        else
            currentEnlarge = 0;
        float enlargeScale = 1-(currentEnlarge / enlargeTime);
        transform.localScale = new Vector3(enlargeScale, enlargeScale, enlargeScale);
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == enemyName)
        {
            other.SendMessage("DealDamage", damage);
            Destroy(gameObject);
        }
        else if(other.tag == "Terrain" || other.tag == "Tree")
        {
            Destroy(gameObject);
        }
    }
}