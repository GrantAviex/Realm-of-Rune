using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour 
{
    public string enemyName;
    int damage;
    public int magicPower;
    public bool armed = false;
    List<GameObject> hitList;
    public float attackAnimationDuration;
    float attackAnimationTimer;
    float attackDamageTimer;
    public float attackTrailTimer;
    public int weaponSpeed;
    public int type;
    int hitCount;
    int maxHit;
    public MeleeWeaponTrail myTrail;
	// Use this for initialization
	void Start () 
    {
        hitList = new List<GameObject>();
        if(transform.childCount > 0)
            myTrail = transform.GetChild(0).GetComponent<MeleeWeaponTrail>();
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        attackDamageTimer -= Time.deltaTime;
        attackAnimationTimer -= Time.deltaTime;
        if(attackAnimationTimer < 0)
        {
            armed = false;
        }
        attackTrailTimer -= Time.deltaTime;
        if(attackTrailTimer < 0 && myTrail != null)
        {
            myTrail.Emit = false;
        }
    }
    public void Attacking(bool attacking)
    {
        if(armed == false && attacking == true)
        {
            //Debug.Log("Armed!");
            attackAnimationTimer = attackAnimationDuration;
            armed = true;
            hitList.Clear();
            attackDamageTimer = attackAnimationDuration/ 2;
            hitCount = 0;
            maxHit = 0;
            if (myTrail != null)
            {
                attackTrailTimer = attackAnimationDuration * .75f;
                myTrail.Emit = true;
            }
        }
    }
    public  void SetDamage(int myDamage)
    {
        damage = myDamage;
    }
    public int GetDamage()
    {
        return damage;
    }

    void DamageTarget()
    {
        if(hitList.Count != 0 && hitCount != maxHit)
        {
            hitList[hitCount].SendMessage("DealDamage", damage);
            hitCount++;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == enemyName && armed)
        {
            if (!hitList.Contains(collision.gameObject))
            {
                hitList.Add(collision.gameObject);
                if(attackDamageTimer > 0)
                {
                    maxHit++;
                    Invoke("DamageTarget", attackDamageTimer);
                }
            }
        }
    }
}
