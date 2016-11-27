using UnityEngine;
using System.Collections;

public class PlayerAttackingController : MonoBehaviour 
{

    CharacterController m_charController;
    Animator m_Animator;

    public GameObject weaponPlaceholderL;
    public GameObject weaponPlaceholderR;
    private Weapon weaponL = null;
    private Weapon weaponR = null;
    int currAttack = 0;
    public float attackAnimationCooldown;
    public float attackCooldown;
    public GameObject Target;
	void Start ()
    {
        m_Animator = GetComponent<Animator>();
        m_charController = GetComponent<CharacterController>();
	}


    public void FixedUpdate()
    {
        attackAnimationCooldown -= Time.deltaTime;
        attackCooldown -= Time.deltaTime;
        m_Animator.SetBool("Attacking", (attackAnimationCooldown > 0));
        if (attackAnimationCooldown < 0)
        {
            m_Animator.SetInteger("AttackStance", 0);
        }
        if (weaponL == null || weaponR == null)
            WeaponsChanged();
    }
    public void WeaponsChanged()
    {
        Weapon[] temps = weaponPlaceholderL.GetComponentsInChildren<Weapon>();
        if (temps.Length != 0)
            weaponL = temps[0];
        temps = weaponPlaceholderR.GetComponentsInChildren<Weapon>();
        if (temps.Length != 0)
            weaponR = temps[0];
    }
    public void Attack(float attack)
    {
        if (weaponR == null && weaponL == null)
            return;
        bool check = attack > 0 && attackAnimationCooldown < 0 && attackCooldown < 0 && !m_Animator.GetBool("Attacking");
            
        if (check)
        {
            //0 = no wep 1 = left hand 2 = right hand 3 = both hands
            int wepStatus = 0;

            if (weaponL)
            {
                if (weaponL.gameObject.activeSelf == true)
                {
                    wepStatus += 1;
                }
            }
            if (weaponR.gameObject.activeSelf == true)
            {
                wepStatus += 2;
            }
            if (wepStatus == 3)
            {
                currAttack++;
                if (currAttack >= 3)
                {
                    currAttack = 0;
                }
            }
            else if (wepStatus == 0)
            {
                m_Animator.SetInteger("AttackStance", 0);
                if(weaponL != null)
                    weaponL.Attacking(false);
                if(weaponR != null)
                    weaponR.Attacking(false);
                return;
            }
            else
            {
                currAttack = wepStatus - 1; ;
            }
            if (currAttack == 0)
            {
                weaponL.Attacking(attack > 0);
            }
            else if (currAttack == 1)
            {
                if (weaponR.gameObject.GetComponent<RangedWeapon>() != null)
                {
                    weaponR.gameObject.GetComponent<RangedWeapon>().Attacking(attack > 0, Target);
                }
                else if (weaponR.type == 1)
                {
                    currAttack = 4;
                    weaponR.Attacking(attack > 0);
                }
                else
                {
                    weaponR.Attacking(attack > 0);
                }
            }
            else if (currAttack == 2)
            {
                weaponR.Attacking(attack > 0);
                weaponL.Attacking(attack > 0);
            }
            if (weaponR.gameObject.GetComponent<RangedWeapon>() != null)
            {
                Vector3 dir;
                if (Target != null)
                    dir = (Target.transform.position - transform.position).normalized;
                else
                    dir = Camera.main.transform.forward;
                dir.y = 0;
                float angle = Vector3.Angle(transform.forward, dir);
                if (Mathf.Abs(angle) > 25)
                {
                    transform.LookAt(transform.position + (dir));
                }
                if (weaponR.gameObject.GetComponent<ThrownWeapon>() != null)
                    currAttack = 5;
                else
                    currAttack = 3;
            }
            if (currAttack == 0)
            {
                attackAnimationCooldown = weaponL.attackAnimationDuration;
                m_Animator.SetInteger("WeaponSpeed", weaponL.weaponSpeed);
            }
            if (currAttack == 1)
            {
                attackAnimationCooldown = weaponR.attackAnimationDuration;
                m_Animator.SetInteger("WeaponSpeed", weaponR.weaponSpeed);

            }
            if (currAttack == 2)
            {
                if (weaponL.weaponSpeed < weaponR.weaponSpeed)
                {
                    attackAnimationCooldown = weaponR.attackAnimationDuration;
                    m_Animator.SetInteger("WeaponSpeed", weaponR.weaponSpeed);
                }
                else
                {
                    attackAnimationCooldown = weaponL.attackAnimationDuration;
                    m_Animator.SetInteger("WeaponSpeed", weaponL.weaponSpeed);
                }

            }
            if (currAttack == 3 || currAttack == 4 || currAttack == 5)
            {
                attackAnimationCooldown = weaponR.attackAnimationDuration;
                m_Animator.SetInteger("WeaponSpeed", weaponR.weaponSpeed);
            }
            m_Animator.SetInteger("AttackStance", ((currAttack + 1)));
            attackCooldown = attackAnimationCooldown + .2f;
        }
        else
        {
            if (attackAnimationCooldown < 0)
            {
                m_Animator.SetInteger("AttackStance", 0);
                if(weaponR != null)
                {
                    if (weaponR.gameObject.GetComponent<RangedWeapon>() != null)
                    {
                        weaponR.gameObject.GetComponent<RangedWeapon>().Attacking(false, Target);   
                    }
                    else
                    {
                        if(weaponL != null)
                            weaponL.Attacking(false);
                        weaponR.Attacking(false);
                    }
                }
            }
        }
    }
}
