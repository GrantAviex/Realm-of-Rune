using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public GameObject Target;
    bool aggrivated;
    public float attackRange;
    public float moveSpeed;
    public float maxMoveSpeed;
    public float attackCooldown;
    float currAttackCooldown;
    public float attackLength;
    public float attackDamage;

    Animator m_animator;
    CharacterController m_charController;
    float roamTime;
    Vector3 roamDir;
    public EnemySpawner mySpawner;
    public float currAttackTimer;
	// Use this for initialization
	public void Start () 
    {
        m_charController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        aggrivated = false;
        roamTime = 1.0f;
        roamDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        m_animator.SetBool("Roaming", Target == null);
	}
	
    void FixedUpdate()
    {
        if(Target != null)
        {
            if (Target.tag != "Player")
            {
                SetTarget(null);
            }
        }
        currAttackCooldown -= Time.deltaTime;
        roamTime -= Time.deltaTime;
        currAttackTimer -= Time.deltaTime;
    }
	// Update is called once per frame
	public void Update () 
    {
	    if(aggrivated)
        {
            if(Target)
            {
                FaceTarget();
                if (currAttackTimer < 0)
                {
                    if((Target.transform.position - transform.position).magnitude <= attackRange )
                    {
                        Attack();
                    }
                    else
                    {
                        Move();
                    }
                }
            }
        }
        else
        {
            Move();
        }
	}
    void FaceTarget()
    {
        Vector3 myPos = transform.position;
        Vector3 targetPos = Target.transform.position;
        targetPos.y = myPos.y = 0;
        Quaternion q = Quaternion.LookRotation(targetPos - myPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
        //transform.forward = Vector3.RotateTowards(transform.forward, Target.transform.position, 3, 1);
        //transform.LookAt(targetPos);

    }
    void FaceDestination()
    {
        if(roamDir != transform.forward)
        {
            if(roamDir == Vector3.zero)
            {
                return;
            }
            Quaternion q = Quaternion.LookRotation(roamDir * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
        }
        //transform.forward = Vector3.RotateTowards(transform.forward, Target.transform.position, 3, 1);
        //transform.LookAt(targetPos);

    }
    void Attack()
    {
        m_animator.SetFloat("Speed", 0);
        m_animator.SetTrigger("Attacking");
        float[] timers = new float[2];
        timers[0] = attackLength / 3;
        timers[1] = attackLength / 2;
        gameObject.BroadcastMessage("Attacking", true, SendMessageOptions.DontRequireReceiver);
        gameObject.BroadcastMessage("ActivateAttackParticles", timers,SendMessageOptions.DontRequireReceiver);
        currAttackCooldown = attackCooldown;
        currAttackTimer = attackLength;
        Debug.Log(currAttackTimer);
        Debug.Log("Attacking!");
    }
    void Move()
    {
        m_animator.SetFloat("Speed", m_charController.velocity.magnitude);
        if(Target)
        {
            //Debug.Log(agent.remainingDistance);
            m_charController.Move(transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            if(roamTime <= 0 && mySpawner != null)
            {
                roamTime = 1.5f;

                float xDist = mySpawner.transform.position.x - mySpawner.spawnOffset;
                float zDist = mySpawner.transform.position.x - mySpawner.spawnOffset;

                //roam Freely
                //Debug.Log("x:" + xDist.ToString() + " z:" + zDist.ToString());
                //Debug.Log("Spawner offset: " + mySpawner.spawnOffset.ToString());
                if(Mathf.Abs(xDist) < mySpawner.spawnOffset / 2 &&Mathf.Abs(zDist) < mySpawner.spawnOffset /2 )
                {
                    roamDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
                }
                  //roam A little less freely prioritizng stayng in the spawn area
               else
               {
                    float minX = mySpawner.transform.position.x - mySpawner.spawnOffset - 5;
                    float maxX = mySpawner.transform.position.x + mySpawner.spawnOffset + 5;
                    float minZ = mySpawner.transform.position.z - mySpawner.spawnOffset - 5;
                    float maxZ = mySpawner.transform.position.z + mySpawner.spawnOffset + 5;
                    float x = transform.position.x;
                    float z = transform.position.z;

                    //Debug.Log(roamDir);
                    roamDir = new Vector3(Random.Range(minX - x, maxX - x), 0, Random.Range(minZ - z, maxZ - z)).normalized;
                }
            }
            else
            {
                m_charController.Move(roamDir * moveSpeed * Time.deltaTime);
            }

        }
    }
    public void SetTarget(GameObject tar)
    {
        Target = tar;        
        aggrivated = tar != null;
        m_animator.SetBool("Roaming", tar == null);
    }
}
