using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargettingSystem : MonoBehaviour 
{


    public float targetingRange;
    public TargetingController targeting;
    public SphereCollider targetingRadius;
    public List<GameObject> enemiesWithinTargetRange;
    public int maxAngle;
    public float maxCamDist;
    public GameObject target;
    public ThirdPersonCamera cam;
	// Use this for initialization
	void Start ()
    {
        targetingRadius.radius = targetingRange;
        cam = Camera.main.gameObject.GetComponent<ThirdPersonCamera>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleTarget();
        }
	}
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 dir = target.transform.position - Camera.main.transform.position;
            float angle = Vector3.Angle(Camera.main.transform.forward, dir);
            //Debug.Log(angle);
            if (angle < -maxAngle/2 || angle > maxAngle/2 || dir.magnitude > maxCamDist)
            {
                ToggleTarget();
            }
        }
        if(enemiesWithinTargetRange.Count != 0)
        {
            List<GameObject> cleanupList = new List<GameObject>();
            foreach (GameObject enemy in enemiesWithinTargetRange)
            {
                if(enemy == null)
                {
                    if(enemy == target)
                    {
                        target = null;
                        targeting.gameObject.SetActive(false);
                        targeting.ReleaseTarget();
                        cam.SetTarget(null);
                        gameObject.transform.root.GetComponent<ThirdPersonController>().Target = null;
                    }
                    cleanupList.Add(enemy);
                }
            }
            foreach(GameObject enem in cleanupList)
            {
                enemiesWithinTargetRange.Remove(enem);
            }
            cleanupList.Clear();

        }


    }
    void ToggleTarget()
    {
        if (target)
        {
            target = null;
            targeting.gameObject.SetActive(false);
            targeting.ReleaseTarget();
            cam.SetTarget(null);
            gameObject.transform.root.GetComponent<ThirdPersonController>().Target = null;
        }
        else
        {
            if(enemiesWithinTargetRange.Count == 0)
            {
                return;
            }
            foreach (GameObject enemy in enemiesWithinTargetRange)
            {
                Vector3 dir = enemy.transform.position - Camera.main.transform.position;
                float angle = Vector3.Angle(Camera.main.transform.forward, dir);
                if (angle > -maxAngle && angle < maxAngle &&  dir.magnitude < maxCamDist)
                {
                    if (target)
                    {
                        Vector3 currDir = target.transform.position - Camera.main.transform.position;
                        float angle2 = Vector3.Angle(Camera.main.transform.forward, currDir);
                        //Debug.Log("Current Target Angle: " + angle2.ToString);
                        if(angle2 > angle)
                        {
                            target = enemy;
                        }
                        else if(angle2 == angle)
                        {
                            if (dir.magnitude < currDir.magnitude)
                            {
                                target = enemy;
                            }
                        }
                    }
                    else
                    {
                        target = enemy;
                    }
                }
            }
            if(target)
            {
                targeting.gameObject.SetActive(true);
                targeting.SetTarget(target);
                gameObject.transform.root.GetComponent<ThirdPersonController>().Target = target;
                cam.SetTarget(target);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHitbox")
        {
            enemiesWithinTargetRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "EnemyHitbox")
        {
            if(enemiesWithinTargetRange.Contains(other.gameObject))
            {
                enemiesWithinTargetRange.Remove(other.gameObject);
            }
        }
    }
}
