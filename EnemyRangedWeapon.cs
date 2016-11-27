using UnityEngine;
using System.Collections;

public class EnemyRangedWeapon : EnemyWeapon 
{

public float range;
    public GameObject projectile;
    public float nextProjectile;
    public float shootProjectile;
    public int shotsFired;

    void Start()
    {
    }
    new void FixedUpdate()
    {
        if(nextProjectile > 0)
        {
            nextProjectile -= Time.deltaTime;
        }
        if(nextProjectile < 0)
        {
            Shoot();
            nextProjectile = 0;
        }
        base.FixedUpdate();
    }
    public new void Attacking(bool attacking)
    {
        if (armed == false && attacking == true)
        {
            armed = attacking;
            nextProjectile = shootProjectile;
        }
        if (armed == true && attacking == false)
        {
            armed = attacking;
        }
    }
    public void Shoot()
    {
        float timeDiff = (shootProjectile / 3) /shotsFired;
        for (int i = 0; i < shotsFired; i++)
        {
            Invoke("Fire", timeDiff * i);
        }
    }
    public void Fire()
    {
        if (myOwner.Target != null)
        {
            GameObject liveProjectile = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody arrowBody = liveProjectile.GetComponent<Rigidbody>();
            Vector3 fireDir;
            float dist = (myOwner.Target.transform.position - transform.parent.position).magnitude;
            fireDir = ((myOwner.Target.transform.position - transform.parent.position) + (Vector3.up * (dist / 5f))).normalized;
            arrowBody.AddForce(fireDir * arrowBody.mass * 600);
            liveProjectile.transform.up = -fireDir;
            liveProjectile.GetComponent<Projectile>().damage = myOwner.attackDamage;
            liveProjectile.GetComponent<Projectile>().lifeTime = 2.0f;
        }
    }
}
