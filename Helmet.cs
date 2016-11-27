using UnityEngine;
using System.Collections;

public class Helmet : MonoBehaviour
{
    Animator m_animator;

	void Start () 
    {
        m_animator = GetComponent<Animator>();
	}

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            m_animator.SetTrigger("HelmetTransition");
        }
    }

}
