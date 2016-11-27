using System;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ThirdPersonController))]
public class ThirdPersonUser : MonoBehaviour
{
    private ThirdPersonController m_Character; // A reference to the ThirdPersonCharacter on the object
    private PlayerAttackingController m_Attacker;
    private PlayerCollectingController m_Collector;
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool mapActive;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonController>();
        m_Attacker = GetComponent<PlayerAttackingController>();
        m_Collector = GetComponent<PlayerCollectingController>();
    }

    public void MapActivate()
    {
        mapActive = true;
    }
    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (mapActive == false)
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float a = Input.GetAxis("Fire1");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            // pass all parameters to the character control script
            m_Character.Move(m_Move, m_Jump);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                m_Attacker.Attack(a);
            }
            m_Jump = false;
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
            m_Character.Move(m_Move, m_Jump);
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                mapActive = false;
            }
        }
    }
}
