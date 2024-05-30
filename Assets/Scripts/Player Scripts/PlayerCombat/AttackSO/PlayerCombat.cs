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
    float Charging = 0;
    public bool IsDashing = false;
    //Variable buat attack/detik
    float AttackRate = 3.5f;
    float NextAttackTime;
    
    //dkk
    Animator anim;
    TrailRenderer trail;
    float SpeedTemp;
    PlayerController playerController;
    Rigidbody2D rb;
     [SerializeField] Weapon weapon;
    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        playerController = GetComponent<PlayerController>();
        SpeedTemp = StatsPlayer.MaxSpeed;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDashing)
        {
            return;
        }
        //Buat batesin Press Player per detik
        if(Time.time >= NextAttackTime)
        {
            //Player Input Attack
            if (Input.GetKey(KeyCode.J))
            {
                //Ngesimpen waktu Press;
                PressTime += Time.deltaTime;
                
                //kalo neken lebih lama dari TimeBuatCharge
                if(PressTime >= TimeToChargeAttack)
                {
                    Debug.Log("Charge");
                    ChargeAttack();
                    
                }
            }
            //Buat Input Teken cepet
            else if(Input.GetKeyUp(KeyCode.J) && PressTime < TimeToChargeAttack)
            {
                
                Attack();
                NextAttackTime = Time.time + 1f / AttackRate;
            }
            else
            {
                //reset PressTime
                PressTime = 0;
            }
            //buat reset AttackCombo kalau Player Tidak Lanjut
            ExitAttack();
        }

    }

    public void Attack()
    {
        if(Time.time - LastComboEnd > 0.5f && ComboCounter <= combo.Count)
        {
            //buat Bisa Jalanin combo, yang end dimatiin
            CancelInvoke("EndCombo");

            if(Time.time - LastClickedTime >= 0.2f)
            {
                //weapon.TriggerON();

                //Buat PLayer gk gerak pas attack
                playerController.enabled = false;
                StatsPlayer.MaxSpeed = 0;

                //Jalanin Animasi dalam List
                anim.runtimeAnimatorController = combo[ComboCounter].AnimatorOV;
                anim.Play("Attack", 0, 0);

                //Update Damage Weapon pada combo
                weapon.Damage = combo[ComboCounter].Damage;

                //Tambahin ComboCounter buat Index list
                ComboCounter++;

                //Update LastClicked buat ngecek terakhir ditekan
                LastClickedTime = Time.time;


                //Buat balikin gerakin terakhir balik ke awal
                if(ComboCounter >= combo.Count)
                {
                    ComboCounter = 0;
                }


            }


        }

    }


    void ExitAttack()
    {
        //buat gk lanjutin combo
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //Debug.Log("Bekantan");
            Invoke("EndCombo", 0);
            
        }
    }

    void EndCombo()
    {
        //Debug.Log("Monyet");
        
        //Player dibkin jalan lagi
        playerController.enabled = true;
        StatsPlayer.MaxSpeed = SpeedTemp;

        //Reset Index Combo dalam list
        ComboCounter = 0;

        //Simpen waktu terakhir kali ngecombo
        LastComboEnd = Time.time;
    }

    //Charge Attack
    void ChargeAttack()
    {
        
        if (Input.GetKeyUp(KeyCode.J))
        {
            Debug.Log("Oke");
            Launch(PressTime, 1f);

        }

    }

    public IEnumerator Launch(float dashingPower, float dashingTime)
    {

        IsDashing = true;
        //float gravityscale = rb.gravityScale;
        //rb.gravityScale = 0f;
        Vector2 DashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = DashingDir.normalized * dashingPower;
        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trail.emitting = false;
        IsDashing = false;
    }

}
