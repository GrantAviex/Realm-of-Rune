using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour 
{
    public string Target;
    public string GatherFunction;
    public int damage;
    bool armed = false;
    float attackDamageTimer;
    Weapon thisWep;
    GameObject hitTree;
    int hit = 0;
    void Start()
    {
        thisWep = gameObject.GetComponent<Weapon>();
    }
    void FixedUpdate()
    {
        attackDamageTimer -= Time.deltaTime;
    }
    void Update()
    {
        Attacking(thisWep.armed);
    }
    public void Attacking(bool attacking)
    {
        if (armed == true && attacking == false)
        {
            armed = attacking;
        }
        if (armed == false && attacking == true)
        {
            hit = 0;
            armed = attacking;
            hitTree = null;
            attackDamageTimer = thisWep.attackAnimationDuration / 2;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == Target && armed)
        {
            if(hitTree == null)
            {
                hitTree = collision.gameObject;
                if (attackDamageTimer > 0)
                {
                    Debug.Log("Hitting " + collision.gameObject.name + " in " + attackDamageTimer + " seconds.");
                    Invoke("DamageTarget", attackDamageTimer);
                }
            }
        }
    }
    void DamageTarget()
    {
        if(hit == 0)
        {
            hit++;
            hitTree.SendMessage(GatherFunction, damage);
        }
    }
}