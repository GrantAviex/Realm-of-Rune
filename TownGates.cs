using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class TownGates : MonoBehaviour
{
    bool GatesOpen;
    GameObject LeaveTownPanel;
    Animator m_animator;
    public GameObject otherGate;
	void Start ()
    {
        m_animator = transform.parent.GetComponent<Animator>();
	}
	

	void Update ()
    {
	
	}
    public void OpenGate()
    {
        m_animator.SetBool("Closed", false);
        otherGate.SetActive(false);
    }
    public void CloseGates()
    {
        m_animator.SetBool("Closed", true);
    }
    public void ChargeFee()
    {
        GameManager.Instance.gameData.data.gold -= 50;
    }
    void ActivateOtherGate()
    {
        otherGate.SetActive(true);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_animator.SetBool("Closed", true);
            Invoke("ActivateOtherGate", 3.0f);
        }
    }
}
