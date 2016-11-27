using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum EntityIDs { Player = 1, Enemy, Ally, Neutral }

public class UnitStats : MonoBehaviour 
{
    [SerializeField]
    EntityIDs myID;

    UnitStatsManager myManager;
    public EnemySpawner mySpawner;
    public float health;
    public float maxHealth;
    public GameObject healthBar;
    public Text healthText;
    private float gracePeriod;
	// Use this for initialization
	void Start ()
    {
        myManager = GameObject.Find("UnitStatsManager").GetComponent<UnitStatsManager>();
        myManager.AddEntity(this);
        FloatingTextController.Initialize();
        health = maxHealth;
        if(healthText)
        {
            healthText.text = (health.ToString() + "/" + maxHealth.ToString());
        }
	}
	public EntityIDs GetID()
    {
        return myID;
    }
	// Update is called once per frame
	void Update () 
    {
        gracePeriod -= Time.deltaTime;
        if(gameObject.tag == "EnemyHitbox")
        {
            healthBar.transform.parent.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
	}
    void DealDamage(float damage)
    {
        if(gracePeriod <0)
        {
            Vector3 spawnPos = transform.position;
            if (gameObject.transform.tag == "EnemyHitBOx")
            {
                spawnPos.y = healthBar.transform.position.y;
            }
            else
            {
                spawnPos.y += .5f;
            }
            FloatingTextController.CreateFloatingText(damage.ToString(), spawnPos);
            health -= damage;
            gracePeriod = 0.0f;
            SetHealth();
        }
        if(health <= 0)
        {
            myManager.RemoveEntity(this);
            if(mySpawner)
            {
                mySpawner.SpawnedMobDied(gameObject);
            }
            Destroy(gameObject.transform.root.gameObject);
        }
    }
    void SetHealth()
    {
        healthBar.transform.localScale = new Vector3(health / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        if (healthText)
        {
            healthText.text = (health.ToString() + " / " + maxHealth.ToString());
        }
    }


}
