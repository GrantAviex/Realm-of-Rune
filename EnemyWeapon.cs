using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyWeapon : MonoBehaviour 
{
    public string enemyName;
    int damage;
    public bool armed = false;
    List<GameObject> hitList;
    public float attackAnimationDuration;
    float attackAnimationTimer;
    float attackDamageTimer;
    int hitCount;
    int maxHit;
    public Enemy myOwner;
    // Use this for initialization
    void Start()
    {
        hitList = new List<GameObject>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        attackDamageTimer -= Time.deltaTime;
        attackAnimationTimer -= Time.deltaTime;
        if (attackAnimationTimer <= 0)
        {
            armed = false;
        }
    }
    public void Attacking(bool attacking)
    {
        if (armed == false && attacking == true)
        {
            attackAnimationTimer = attackAnimationDuration;
            armed = attacking;
            hitList.Clear();
            attackDamageTimer = attackAnimationDuration / 2;
            hitCount = 0;
            maxHit = 0;
        }
    }
    public void SetDamage(int myDamage)
    {
        damage = myDamage;
    }
    public int GetDamage()
    {
        return damage;
    }

    void DamageTarget()
    {
        if (hitList.Count != 0 && hitCount != maxHit)
        {
            hitList[hitCount].SendMessage("DealDamage", myOwner.attackDamage);
            hitCount++;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == enemyName && armed)
        {
            if (!hitList.Contains(other.gameObject))
            {
                hitList.Add(other.gameObject);
                if (attackDamageTimer > 0)
                {
                    maxHit++;
                    Invoke("DamageTarget", attackDamageTimer);
                }
            }
        }
    }
}
