using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInfo : MonoBehaviour
{
    public static EnemyBaseProperties m_BaseProperties;

    private float MaxHitPoint, HitPoint;
    private float Damage;

    // Start is called before the first frame update
    void Start()
    {
        m_BaseProperties = GetComponent<EnemyBaseProperties>();

        MaxHitPoint = m_BaseProperties._fHitpoints;
        HitPoint = MaxHitPoint;
        Damage = m_BaseProperties._fDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float TakeDamage(float damage)
    {
        HitPoint -= damage;

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
}
