using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    bool playerInRange = false;
    Animator m_animator;
    public GameObject interactTooltipPrefab;
    private GameObject myTooltip;

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if(playerInRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                m_animator.SetTrigger("Transition");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = false;
            Destroy(myTooltip);
            myTooltip = null;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
            myTooltip = Instantiate(interactTooltipPrefab);
        }
    }
}
