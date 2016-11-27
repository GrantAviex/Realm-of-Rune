using UnityEngine;
using System.Collections;

public class Bola : Projectile
{
    Rigidbody m_rigidBody;
    public string enemyName;
    float rotationAmt = 0;
    public int tier;
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
            rotationAmt += 720 * Time.deltaTime;
            //transform.Rotate(transform.right,-rotationAmt);
            transform.localRotation = Quaternion.Euler(0.0f, rotationAmt, 90);
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
