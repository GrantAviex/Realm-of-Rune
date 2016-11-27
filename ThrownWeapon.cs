using UnityEngine;
using System.Collections;

public class ThrownWeapon : RangedWeapon
{
    MeshRenderer myRender;
    public EquippedItemData myData;
    new void Start()
    {
        myRender = gameObject.GetComponent<MeshRenderer>();
        base.Start();
    }
    public void SetMyData(EquippedItemData data)
    {
        myData = data;
    }
    void FixedUpdate()
    {
        if (nextProjectile > 0)
        {
            //Debug.Log(nextProjectile);
            nextProjectile -= Time.deltaTime;
        }
        if (nextProjectile < 0)
        {
            Throw();
            nextProjectile = 0;
        }
    }
    public void Throw()
    {
        myRender.enabled = false;
        Invoke("ShowObject", shootProjectile / 3);
        float timeDiff = (shootProjectile / 3) / shotsFired;
        for (int i = 0; i < shotsFired; i++)
        {
            if(myData.amount > 0)
            {
                myData.SetThrownWeaponAmount(--myData.amount);
                Invoke("SpawnProjectile", timeDiff * i);
            }
        }
    }
    void SpawnProjectile()
    {
        GameObject liveProjectile = (GameObject)Instantiate(projectile, transform.position,transform.rotation);
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
        //liveProjectile.transform.forward = -fireDir;
        liveProjectile.GetComponent<Projectile>().damage = GetDamage();
        liveProjectile.GetComponent<Projectile>().lifeTime = 2.0f;
    }
    void ShowObject()
    {
        myRender.enabled = true;
    }
}
