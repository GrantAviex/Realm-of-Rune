using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon 
{
    public GameObject projectile;
    public float nextProjectile;
    public float shootProjectile;
    public GameObject Target;
    public int shotsFired;

    public void Start()
    {
        Invoke("SetSpeed", 1f);
    }
    void SetSpeed()
    {
        shootProjectile = attackAnimationDuration * .75f;
    }
    void FixedUpdate()
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
    }
    public void Attacking(bool attacking, GameObject target)
    {
        if (armed == false && attacking == true)
        {
            armed = attacking;
            Target = target;
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
        GameObject liveProjectile = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody arrowBody = liveProjectile.GetComponent<Rigidbody>();
        Vector3 fireDir;
        if (Target != null)
        {
            float dist = (Target.transform.position - transform.parent.position).magnitude;
            fireDir = ((Target.transform.position - transform.parent.position) + (Vector3.up * (dist / 2.75f))).normalized;
        }
        else
        {
            fireDir = (Camera.main.transform.forward + (Vector3.up * .75f)).normalized;
        }
        arrowBody.AddForce(fireDir * arrowBody.mass * 300);
        liveProjectile.transform.up = -fireDir;
        liveProjectile.GetComponent<Projectile>().damage = GetDamage();
        liveProjectile.GetComponent<Projectile>().lifeTime = 2.0f;
    }
}
