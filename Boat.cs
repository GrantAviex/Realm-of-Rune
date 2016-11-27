using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boat : MonoBehaviour
{
    List<GameObject> playersInsideRange;
    public GameObject interactTooltip;
    GameObject playerMovePos;
    GameObject currentRider;
	// Use this for initialization
	void Start ()
    {
        playersInsideRange = new List<GameObject>();
        playerMovePos = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (playersInsideRange.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(currentRider == null)
                {
                    currentRider = playersInsideRange[0];
                    currentRider.gameObject.transform.parent = playerMovePos.transform;
                    currentRider.transform.localPosition = Vector3.zero;
                    currentRider.GetComponent<ThirdPersonController>().SetBoat(gameObject);
                    currentRider.transform.forward = -transform.forward;
                }
                else
                {
                    currentRider.gameObject.transform.parent = null;
                    currentRider.GetComponent<ThirdPersonController>().SetBoat(null);
                    currentRider = null;
                }
            }
            if (interactTooltip.activeSelf == false)
            {
                interactTooltip.SetActive(true);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playersInsideRange.Add(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playersInsideRange.Contains(other.gameObject))
            {
                playersInsideRange.Remove(other.gameObject);
                if (playersInsideRange.Count == 0)
                {
                    if (interactTooltip.activeSelf == true)
                    {
                        interactTooltip.SetActive(false);
                    }
                }
            }
        }
    }
}
