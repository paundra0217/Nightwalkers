using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDCT;

public class PlayerCombat : MonoBehaviour
{
    [Header("Stats")]
    //Player Stats
    private float Hp = 0;

    [Header("Combo")]
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
    bool IsCharging = false;
    [Header("Charge")]
    public bool IsDashing = false;
    public bool IsLaunching = false;
    //Variable buat attack/detik
    float AttackRate = 3.5f;
    float NextAttackTime;

    [Header("Parry")]
    //Variable buat Parry
    public bool IsParry = false;
    float ParryTime = 0.5f;
    float ParryTakeDamageTime = 0f;
    [Header("Flow")]
    //Flow
    [SerializeField] private PlayerCombatStat combatStat;
    float CurrFlowGauge = 0;
    //float MaxFlowGauge;
    [Header("Conjure")]
    [SerializeField] private ConjureSO[] ConjureSO_Group;
    [SerializeField] private Transform ProjectilePos;
    public int ConjureIndex = 0;
    public bool IsConjure = false;
    //dkk
    public bool IstakeDamage = false;
    Animator anim;
    TrailRenderer trail;
    float SpeedTemp;
    float DashSpeed = 10000f;
    float ChargeSpeed;
    float ResetTime;
    bool TandaReset = true;
    PlayerController playerController;
    Rigidbody2D rb;
    Collider2D collider2D;
    [SerializeField] Weapon[] weapon;

    // Start is called before the first frame update
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        playerController = GetComponent<PlayerController>();
        SpeedTemp = StatsPlayer.MaxSpeed;
        ChargeSpeed = SpeedTemp / 2;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Awake()
    {
        //MaxFlowGauge = combatStat.MaxFlow;

        Hp = combatStat.Maxhp;
    }
    // Update is called once per frame
    void Update()
    {
        if (IstakeDamage && TandaReset == true)
        {
            ResetTime = Time.time + 2f;
            TandaReset = false;
        }

        if (ResetTime < Time.time && TandaReset == false)
        {
            IstakeDamage = false;
            TandaReset = true;
        }

        if (IsDashing)
        {
            return;
        }

        if (IsDashing || Hp <= 0)
        {
            //fuck you
            Destroy(gameObject);
            return;
        }

        //catat waktu parry
        if (IsParry)
        {
            ParryTakeDamageTime += Time.deltaTime;
        }

        //Buat batesin Press Player per detik
        if (Time.time >= NextAttackTime)
        {
            //Flow input
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //flow Heal
                if (Input.GetKeyDown(KeyCode.O))
                {
                    FlowHeal();
                }

                //Gyakuten pls gw gk tau apaan cuman ada kata disainer
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    Gyakuten();
                }

            }

            //Conjure
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                //Select Zodiac left
                if (Input.GetKeyDown(KeyCode.O))
                {
                    if (ConjureIndex == 0)
                    {
                        ConjureIndex = ConjureSO_Group.Length - 1;
                    }
                    else
                    {
                        ConjureIndex--;
                    }

                }

                //Select Zodiac right
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    if (ConjureIndex == ConjureSO_Group.Length - 1)
                    {
                        ConjureIndex = 0;
                    }
                    else
                    {
                        ConjureIndex++;
                    }

                }
                //Choose Zodiac
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    ConjureWeapon();
                    //IsConjure = true;

                }

            }

            //Player Input Attack
            else if (Input.GetKey(KeyCode.O))
            {
                //Ngesimpen waktu Press;
                PressTime += Time.deltaTime;

                //kalo neken lebih lama dari TimeBuatCharge
                if (PressTime >= TimeToChargeAttack)
                {
                    Charging += Time.deltaTime;
                    StatsPlayer.MaxSpeed = ChargeSpeed;
                    IsCharging = true;

                }


            }

            //Buat Input Attack Teken cepet
            else if (Input.GetKeyUp(KeyCode.O) && PressTime < TimeToChargeAttack)
            {

                Attack();
                NextAttackTime = Time.time + 1f / AttackRate;
            }

            //Input Parry
            else if (Input.GetKeyDown(KeyCode.P))
            {
                ParryTakeDamageTime = 0;
                StartCoroutine(Parry());

            }

            //Key Up
            if (Input.GetKeyUp(KeyCode.O) && IsCharging)
            {
                StartCoroutine(Launch(PressTime, 0.1f));
                PressTime = 0;
            }

            else if (Input.GetKeyUp(KeyCode.O) && IsCharging == false)
            {
                //reset PressTime
                PressTime = 0;
            }
            //buat reset AttackCombo kalau Player Tidak Lanjut
            ExitAttack();
        }



    }



    #region ComboAttack
    public void Attack()
    {
        if (Time.time - LastComboEnd > 0.5f && ComboCounter <= combo.Count)
        {
            //buat Bisa Jalanin combo, yang end dimatiin
            CancelInvoke("EndCombo");

            if (Time.time - LastClickedTime >= 0.2f)
            {
                //weapon.TriggerON();

                //Buat PLayer gk gerak pas attack
                playerController.enabled = false;

                //Jalanin Animasi dalam List
                anim.runtimeAnimatorController = combo[ComboCounter].AnimatorOV;
                anim.Play("Attack", 0, 0);

                //Update Damage Weapon pada combo
                weapon[ComboCounter].Damage = combo[ComboCounter].Damage;

                //Update Flow Gauge
                CurrFlowGauge += combo[ComboCounter].Flow;
                CurrFlowGauge = Mathf.Clamp(CurrFlowGauge, 0, combatStat.MaxFlow);

                //Tambahin ComboCounter buat Index list
                ComboCounter++;

                //Update LastClicked buat ngecek terakhir ditekan
                LastClickedTime = Time.time;


                //Buat balikin gerakin terakhir balik ke awal
                if (ComboCounter >= combo.Count)
                {
                    ComboCounter = 0;
                }


            }


        }

    }


    void ExitAttack()
    {
        //buat gk lanjutin combo
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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


        //Reset Index Combo dalam list
        ComboCounter = 0;

        //Simpen waktu terakhir kali ngecombo
        LastComboEnd = Time.time;
    }

    #endregion

    #region ChargeAttack
    //Charge Attack
    public IEnumerator Launch(float dashingPower, float dashingTime)
    {
        StatsPlayer.MaxSpeed = DashSpeed;
        IsDashing = true;

        //ambil Input Arah
        Vector2 DashingDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //biar gk lari kenceng
        dashingPower = Mathf.Clamp(dashingPower, 0f, 3f);

        //biar cuman 3 arah
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            DashingDir.x = 0;
        }

        //ngedash
        IsLaunching = true;
        rb.velocity = DashingDir.normalized * dashingPower * 10f;
        //collider2D.isTrigger = true;

        trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        //setelah dash
        //collider2D.isTrigger = false;
        IsLaunching = false;
        trail.emitting = false;
        IsDashing = false;
        StatsPlayer.MaxSpeed = SpeedTemp;
    }

    #endregion

    #region Parry

    //Parry
    public IEnumerator Parry()
    {
        IsParry = true;
        anim.SetBool("Parry_Fail", false);
        anim.SetTrigger("ParryTrigger");
        yield return new WaitForSeconds(1f);
        IsParry = false;
        anim.SetBool("Parry_Fail", true);
    }

    public void Parried()
    {
        StopCoroutine(Parry());
        anim.SetTrigger("Parry_Success");
        //Debug.Log(ParryTakeDamageTime);
        if (ParryTakeDamageTime < 0.3f)
        {
            //Perfect Parry
            CurrFlowGauge += 30;

        }
        else
        {
            //Normal Parry
            CurrFlowGauge += 15;


        }
        CurrFlowGauge = Mathf.Clamp(CurrFlowGauge, 0, combatStat.MaxFlow);

    }

    #endregion

    #region PlayerCombat
    public bool PlayerDeath()
    {
        if (Hp <= 0)
        {
            return true;
        }
        return false;
    }

    public IEnumerator TakeDamage(float damage)
    {
        Hp -= damage;
        Hp = Mathf.Clamp(Hp, 0, combatStat.Maxhp);
        IstakeDamage = true;
        yield return new WaitForSeconds(1f);
        IstakeDamage = false;
    }

    public void TakeHeal(float heal)
    {
        Hp += heal;
        Hp = Mathf.Clamp(Hp, 0, combatStat.Maxhp);
    }

    #endregion

    #region Flow

    public void FlowHeal()
    {
        if (CurrFlowGauge < 40f)
        {
            Debug.Log("GK cukup");
            return;
        }
        TakeHeal(1f);
        CurrFlowGauge -= 40f;

    }

    public void Gyakuten()
    {
        Debug.Log("gk tau ngapain");
        if (CurrFlowGauge < 60f)
        {
            return;
        }
        CurrFlowGauge -= 60f;
    }


    #endregion

    #region Conjure

    public void ConjureWeapon()
    {
        if (CurrFlowGauge < ConjureSO_Group[ConjureIndex].BulletCost)
        {
            Debug.Log("Flow gk cukup");
            return;
        }
        CurrFlowGauge -= ConjureSO_Group[ConjureIndex].BulletCost;

        for (int i = 0; i < ConjureSO_Group[ConjureIndex].BurstAmount; i++)
        {
            GameObject projectile = Instantiate(ConjureSO_Group[ConjureIndex].Projectile, ProjectilePos);
            projectile.transform.parent = null;
        }

    }

    #endregion



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AIInfo>() && IsLaunching)
        {
            AIInfo enemy = collision.gameObject.GetComponent<AIInfo>();
            Rigidbody2D Enemyrb = collision.gameObject.GetComponent<Rigidbody2D>();
            //enemy AddForce
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Enemyrb.AddForce(direction * 5f, ForceMode2D.Impulse);

            enemy.TakeDamage(weapon[ComboCounter].Damage);
        }
    }


}
