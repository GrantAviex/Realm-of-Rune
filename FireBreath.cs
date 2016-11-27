using UnityEngine;
using System.Collections;

public class FireBreath : MonoBehaviour
{
    float activationTimer;
    float delayTimer;
    ParticleSystem attackParticles;
    CapsuleCollider fireCollider;
    bool playing;
    GameObject lightsource;
	// Use this for initialization
	void Start ()
    {
        lightsource = gameObject.transform.GetChild(0).gameObject;
        attackParticles = GetComponent<ParticleSystem>();
        fireCollider = GetComponent<CapsuleCollider>();
        playing = false;
	}
	
    void FixedUpdate()
    {
        //Debug.Log(delayTimer);
        if(delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        else
        {
            activationTimer -= Time.deltaTime;
        }
    }
	// Update is called once per frame
	void Update () 
    {
        if(playing)
        {
            if(delayTimer <0)
            {
                attackParticles.Play();
                fireCollider.enabled = true;
                lightsource.SetActive(true);
            }
            if(activationTimer < 0)
            {
                lightsource.SetActive(false);
                attackParticles.Stop();
                fireCollider.enabled = false;
            }
        }
	}
    //0 = activation timer //1 = delay timer
    void ActivateAttackParticles(float[] timers)
    {
        //lightsource.SetActive(true);
        playing = true;
        activationTimer = timers[0];
        delayTimer = timers[1];
    }
    void OnTriggerEnter(Collider other)
    {
        //.Log("FireHit :");
        if(other.tag == "Player")
        {
            //Debug.Log(other.tag);
            other.SendMessage("DealDamage", transform.root.GetComponent<Enemy>().attackDamage);
        }
    }
}
