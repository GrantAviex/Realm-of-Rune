using UnityEngine;
using System.Collections;

public class Pet : MonoBehaviour 
{
    //Movement
    public float followDistance;
    public float speed;
    public float followSpeedModifier;
    [Range(0f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;

    //Roaming
    public float maxRoamTime;
    float roamTimer;
    public GameObject owner;    
    Vector3 currentDirection;
    CharacterController m_petController;

    //Attacking
    public GameObject target;
    public float attackDistance;
    [Range(0f, 4f)]
    [SerializeField]
    public float attackCooldown;
    float attackTimer;
    Animator m_petAnimator;
    [SerializeField]
    bool rangedPet;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    GameObject firingAnchor;
    [Range(0, 30f)]
    [SerializeField]
    float attackDamage;
	void Start ()
    {
        m_petController = GetComponent<CharacterController>();
        m_petAnimator = transform.GetChild(0).GetComponent<Animator>();
        attackTimer = 0;
        roamTimer = 0;
	}

	void FixedUpdate () 
    {
        if(Attack() == false)
        {
            FindDirection();
            FaceDirection();
            Move();
        }
	}

    void Move()
    {
        roamTimer -= Time.deltaTime;
        m_petController.Move(currentDirection * Time.deltaTime);
        m_petController.Move(new Vector3(0, -1 * m_GravityMultiplier * Time.deltaTime, 0));
    }
    void FindDirection()
    {
        Vector3 currentDist = owner.transform.position - transform.position;
        //Debug.Log(currentDist.magnitude);
        if(currentDist.magnitude > followDistance)
        {
            currentDirection = currentDist.normalized * speed * followSpeedModifier;
            currentDirection.y = 0;
            //Debug.Log(currentDirection);
        }
        else
        {
            if(roamTimer < 0)
            {
                float maxX = owner.transform.position.x + followDistance;
                float maxZ = owner.transform.position.z + followDistance;
                float minX = owner.transform.position.x - followDistance;
                float minZ = owner.transform.position.z - followDistance;
                float x = transform.position.x;
                float z = transform.position.z;

                currentDirection = new Vector3(Random.Range(minX - x, maxX - x), 0, Random.Range(minZ - z, maxZ - z)).normalized * speed;
                roamTimer = maxRoamTime;
            }
        }
    }
    void FaceDirection()
    {
        if (currentDirection != transform.forward)
        {
            if (currentDirection == Vector3.zero)
            {
                return;
            }
            Quaternion q = Quaternion.LookRotation(currentDirection * speed * followSpeedModifier);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
        }
    }
    void FaceTarget()
    {
        currentDirection = (target.transform.position - transform.position).normalized;
        if (currentDirection != transform.forward)
        {
            if (currentDirection == Vector3.zero)
            {
                return;
            }
            Quaternion q = Quaternion.LookRotation(currentDirection * speed * followSpeedModifier * 2);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
        }
    }
    bool Attack()
    {
        if (attackTimer <= 0)
        {
            if (target != null)
            {
                if ((target.transform.position - transform.position).magnitude <= attackDistance)
                {
                    FaceTarget();
                    m_petAnimator.SetTrigger("Attack");
                    attackTimer = attackCooldown;
                    if(rangedPet)
                    {
                        Invoke("Fire", attackTimer * .3f);
                    }
                    return true;
                }
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
            FaceDirection();
            return true;
        }
        return false;
    }
    void Fire()
    {
        GameObject liveProjectile = (GameObject)Instantiate(projectile, firingAnchor.transform.position, Quaternion.identity);
        Rigidbody arrowBody = liveProjectile.GetComponent<Rigidbody>();
        float height = target.GetComponentInChildren<BoxCollider>().size.y;
        Vector3 fireDir = (target.transform.position + new Vector3(0,height *.5f,0) - firingAnchor.transform.position).normalized;
        arrowBody.AddForce(fireDir * arrowBody.mass * 200);
        liveProjectile.transform.forward = fireDir;
        liveProjectile.GetComponent<Projectile>().damage = attackDamage;
        liveProjectile.GetComponent<Projectile>().lifeTime = 2.0f;
    }

}
