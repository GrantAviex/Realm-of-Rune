using UnityEngine;
using System.Collections;

public class Arrow : Projectile
{
    Rigidbody m_rigidBody;
    public string enemyName;
    public int projectileType;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    new void FixedUpdate()
    {
        if (m_rigidBody == null)
            m_rigidBody = GetComponent<Rigidbody>();

        else if(m_rigidBody != null && m_rigidBody.useGravity)
        {
            if(projectileType == 0)
                transform.up = -m_rigidBody.velocity.normalized;
            else if(projectileType == 1)
                transform.forward = m_rigidBody.velocity.normalized;
            else if (projectileType == 2)
                transform.forward = -m_rigidBody.velocity.normalized;
        }

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
            if(m_rigidBody == null)
            {
                m_rigidBody = GetComponent<Rigidbody>();
            }
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.useGravity = false;
        }
    }
}
