using UnityEngine;
using System.Collections;

public class TargetingController : MonoBehaviour 
{
     GameObject Target;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Target)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position + (Vector3.up * 0.1f));
            transform.position = screenPosition;
        }
    }
    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            Target = target;

            Vector2 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position + (Vector3.up * 0.1f));
            transform.position = screenPosition;
        }

    }
    public void ReleaseTarget()
    {
        Target = null;
    }
    
}
