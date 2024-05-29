using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInfo : MonoBehaviour
{
    public EnemyBaseProperties m_BaseProperties;
    public static BaseBehavior m_BaseBehavior;
    public AIAttackHandlers[] m_AttackHandler;
    public EAIStates m_State;

    private float MaxHitPoint, HitPoint;
    private float Damage;
    private int AttackSeq;
    public GameObject ParentObject;
    private bool bAttack;

    // Start is called before the first frame update
    void Start()
    {
        m_BaseProperties = GetComponent<EnemyBaseProperties>();      
        AttackSeq = 0;
        MaxHitPoint = m_BaseProperties._fHitpoints;
        HitPoint = MaxHitPoint;
        
    }

    // Update is called once per frame
    void Update()
    {
        Damage = m_AttackHandler[AttackSeq].AttackDamage;
    }

    public float TakeDamage(float damage)
    {
        HitPoint -= damage;

        if (HitPoint <= 0)
        {
            Destroy(ParentObject);
        }

        //var idles = new IdleAction();

        //m_BaseBehavior.addAction(idles);

        return damage;
    }

    public float getHitPointLeft()
    {
        return HitPoint;
    }

    public float getDamage()
    {
        return Damage;
    }

    private void comboAttack()
    {

            
        if (bAttack)
        {
            if (AttackSeq > 0 && AttackSeq >= m_AttackHandler.Length)
            {
                comboStop();
                return;
            }

            if (AttackSeq >= m_AttackHandler.Length)
                return;

            AttackSeq++;
        }

        else return;
    }

    private void comboStop()
    {
        AttackSeq = 0;

        return;
    }

    private void Projectile()
    {
        if (m_AttackHandler[AttackSeq].projectile = null) return;   
    }
}