using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class ThirdPersonController : MonoBehaviour  
{
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_JumpPower = 12f;
    [Range(0f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;//specific to the character in sample assets, will need to be modified to work with others
    [SerializeField]
    float m_WalkSpeedMultiplier = 1.5f;
    [SerializeField]
    float m_RunSpeedMultiplier = 2f;
    [SerializeField]
    float m_SprintSpeedMultiplier = 3f;
    float m_MoveSpeedMultiplier = 1f;
    int currentSpeed = 1;

    public float maxStamina;
    float currStamina;
    public float staminaRate;
    public GameObject staminaBar;
    public Text staminaText;
    public GameObject currButtonHighlight;

    CharacterController m_charController;
    Animator m_Animator;
    public bool m_IsGrounded;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    bool m_Crouching;
    bool m_jumping;
    public float jumpTime;
    float jumpDuration = 1.1f;

    bool mapActive = false;
    public GameObject map;
    public UIMenu mapUI;

    private GameObject boat;
    public GameObject Target;
    public GameObject torsoPlaceholder;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_charController = GetComponent<CharacterController>();
        m_MoveSpeedMultiplier = m_WalkSpeedMultiplier;
        currStamina = maxStamina;
        if(staminaText)
        {
            staminaBar.transform.localScale = new Vector3(currStamina / maxStamina, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
            if (staminaText)
            {
                staminaText.text = (((int)currStamina).ToString() + " / " + maxStamina.ToString());
            }
        }
    }
	public void MapActivate()
    {
        mapActive = true;
        m_Animator.SetBool("MapOpen", mapActive);
        map.SetActive(true);
        Invoke("MapHUDActivate", 1.0f);
    }
    public void MapHUDActivate()
    {
        mapUI.active = true;
        mapUI.gameObject.SetActive(true);
    }
    public void MapDeactivate()
    {
        map.SetActive(false);
    }
    public void Move(Vector3 move, bool jump)
    {

        ApplyMovement(move);

        // control and velocity handling is different when grounded and airborne:
        m_Animator.SetBool("Grounded", m_IsGrounded);
        if (m_IsGrounded)
        {
           HandleGroundedMovement(jump);
        }
    }
    public void FixedUpdate()
    {
        if(mapActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mapActive = false;
                m_Animator.SetBool("MapOpen", false);
                Invoke("MapDeactivate", 1.0f);
                mapUI.active = false;
                mapUI.gameObject.SetActive(false);
            }
        }
        jumpTime -= Time.deltaTime;
        m_charController.Move(new Vector3(0, -1 * m_GravityMultiplier * Time.deltaTime, 0));
        if(jumpTime >  jumpDuration - 0.35f)
        {
            m_charController.Move(new Vector3(0, m_JumpPower * Time.deltaTime, 0));
        }

        
    }
    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !m_jumping && jumpTime < 0 && boat == null)
        {
            m_jumping = true;
            m_Animator.SetTrigger("Jump");
            // jump!
            //m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            jumpTime = jumpDuration;
           // m_Animator.applyRootMotion = false;
        }
    }
    public void MovementToggle(GameObject obj)
    {
        if(currButtonHighlight)
        {
            if(currButtonHighlight == obj)
            {
                return;
            }
            currButtonHighlight.GetComponent<Image>().color = new Color(1, 1, 1);
        }

        currButtonHighlight = obj;
        currButtonHighlight.GetComponent<Image>().color = new Color(1, .8f, 0);
        if (obj.name == "Walking Button")
        {
            m_MoveSpeedMultiplier = m_WalkSpeedMultiplier;
            currentSpeed = 1;
        }
        if (currStamina > 10)
        {
            if (obj.name == "Running Button")
            {
                m_MoveSpeedMultiplier = m_RunSpeedMultiplier;
                currentSpeed = 2;
            }
            if (obj.name == "Sprinting Button")
            {
                m_MoveSpeedMultiplier = m_SprintSpeedMultiplier;
                currentSpeed = 3;
            }
        }

    }
    void ApplyMovement(Vector3 moveDir)
    {
        m_ForwardAmount = 0;
        if (moveDir != Vector3.zero)
        {
            moveDir = transform.InverseTransformDirection(moveDir);
            m_TurnAmount = Mathf.Atan2(moveDir.x, moveDir.z);
            m_ForwardAmount = moveDir.z;
            if (Target)
            {
                Vector3 myPos = transform.position;
                Vector3 movePos = transform.position;
                movePos += transform.forward * m_ForwardAmount * m_MoveSpeedMultiplier;
                movePos += transform.right * moveDir.x * m_MoveSpeedMultiplier;
                Vector3 targetMoveDIff = Target.transform.position - movePos;
                Vector3 finalPos = movePos + (targetMoveDIff * (targetMoveDIff.magnitude * .95f));
                finalPos.y = myPos.y = 0;
                Quaternion q = Quaternion.LookRotation(finalPos - myPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 3f);
            }
            else
            {
                float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
                if (boat == null)
                    transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
                else
                {
                    m_TurnAmount = Mathf.Clamp(m_TurnAmount, -.35f, .35f);
                    boat.transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
                }

            }
            if (m_IsGrounded)
            {
                Vector3 v = transform.forward * m_ForwardAmount * m_MoveSpeedMultiplier;
                v.y = 0;
                if (boat == null)
                    m_charController.Move(v * Time.deltaTime);
                else
                {
                    m_charController.Move(v * Time.deltaTime * 3.0f);
                }

                v = transform.right * moveDir.x * m_MoveSpeedMultiplier;
                v.y = 0;
                if(boat == null)
                    m_charController.Move(v * Time.deltaTime);
                else
                {
                    //m_charController.Move(v * Time.deltaTime);
                }

            }
        }

        Vector3 vel = m_charController.velocity;
        vel.y = 0;
        if((moveDir.x != 0 || moveDir.z != 0) && boat == null)
        {
            m_Animator.SetFloat("Speed", currentSpeed * 2);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0);
        }
        currStamina -= staminaRate * Mathf.Abs(m_ForwardAmount) * (currentSpeed - 1) * Time.deltaTime;
        if(currStamina < 0)
        {
            currStamina = 0;
            currentSpeed = 1;
            currButtonHighlight.GetComponent<Image>().color = new Color(1, 1, 1);
            currButtonHighlight = GameObject.Find("Walking Button");
            currButtonHighlight.GetComponent<Image>().color = new Color(1, .8f, 0);
            m_MoveSpeedMultiplier = m_WalkSpeedMultiplier;
        }
        if(m_ForwardAmount == 0 || currentSpeed == 1)
        {
            currStamina += staminaRate / 3 * Time.deltaTime;
        }

        if (currStamina > maxStamina)
        {
            currStamina = maxStamina;
        }
        if (staminaText)
        {
            staminaBar.transform.localScale = new Vector3(currStamina / maxStamina, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
            if (staminaText)
            {
                staminaText.text = (((int)currStamina).ToString() + " / " + maxStamina.ToString());
            }
        }
    }
    public void OnAnimatorMove()
    {
    }
    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Structure")
        {
            if (m_IsGrounded == false)
            {
                m_IsGrounded = true;
                m_jumping = false;
            }
        }
    }
    public void SetBoat(GameObject newBoat)
    {
        if(newBoat != null)
        {
            boat = newBoat;
            m_charController = boat.GetComponent<CharacterController>();
        }
        else
        {
            boat = null;
            m_charController = GetComponent<CharacterController>();
        }
    }
}
