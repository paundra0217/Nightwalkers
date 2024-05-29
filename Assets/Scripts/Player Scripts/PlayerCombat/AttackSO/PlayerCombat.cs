using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDCT;

public class PlayerCombat : MonoBehaviour
{
    //buat combo
    public List<AttackSO> combo;
    [SerializeField] ScriptableStats StatsPlayer;
    float LastClickedTime;
    float LastComboEnd;
    int ComboCounter;

    //buat charge attack
    float PressTime = 0;
    float TimeToChargeAttack = 0.2f;
    //Variable buat attack/detik
    float AttackRate = 3.5f;
    float NextAttackTime;

    //dkk
    Animator anim;
    float SpeedTemp;
    PlayerController playerController;
     [SerializeField] Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        SpeedTemp = StatsPlayer.MaxSpeed;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= NextAttackTime)
        {
            if (Input.GetKey(KeyCode.J))
            {
                PressTime += Time.deltaTime;
                
                if(PressTime >= TimeToChargeAttack)
                {
                    Debug.Log("Charge");

                }

                

            }
            else if(Input.GetKeyUp(KeyCode.J) && PressTime < TimeToChargeAttack)
            {
                Attack();
                NextAttackTime = Time.time + 1f / AttackRate;
            }
            else
            {
                PressTime = 0;
            }
            
            ExitAttack();
        }

    }

    public void Attack()
    {
        if(Time.time - LastComboEnd > 0.5f && ComboCounter <= combo.Count)
        {

            CancelInvoke("EndCombo");

            if(Time.time - LastClickedTime >= 0.2f)
            {
                //weapon.TriggerON();
                playerController.enabled = false;
                StatsPlayer.MaxSpeed = 0;
                anim.runtimeAnimatorController = combo[ComboCounter].AnimatorOV;
                anim.Play("Attack", 0, 0);
                weapon.Damage = combo[ComboCounter].Damage;
                ComboCounter++;
                LastClickedTime = Time.time;

                if(ComboCounter >= combo.Count)
                {
                    ComboCounter = 0;
                }


            }


        }

    }


    void ExitAttack()
    {

        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Debug.Log("Bekantan");
            Invoke("EndCombo", 0);
            
        }
    }

    void EndCombo()
    {
        //Debug.Log("Monyet");
        playerController.enabled = true;
        //weapon.TriggerOff();
        StatsPlayer.MaxSpeed = SpeedTemp;
        ComboCounter = 0;
        LastComboEnd = Time.time;
    }

    void ChargeAttack()
    {


    }

}
