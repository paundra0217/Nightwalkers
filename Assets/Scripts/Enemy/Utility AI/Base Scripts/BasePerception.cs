using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasePerception : MonoBehaviour
{
    public static BaseBehavior baseBehavior;
    public static AIInfo aiInfo;

    private static float fNormalFactor = 10f;
    private static float fChaseFactor = 15f;
    private float range;

    public bool bLineOfSight;

    public GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        baseBehavior = GetComponent<BaseBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Player == null) findPlayer();

        HasLineOfSight();
    }

    public GameObject findPlayer()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");

        return m_Player;
    }

    public bool HasLineOfSight()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, m_Player.transform.position - transform.position);
        bLineOfSight = ray.collider.CompareTag("Player");        
        
        
        return bLineOfSight;
    }

    public float enemyToPlayerRange()
    {
        range = Vector2.Distance(this.transform.position, m_Player.transform.position);


        return range;
    }

    private void OnDrawGizmos()
    {
        Vector2 dir = transform.TransformDirection(m_Player.transform.position - transform.position);
        Gizmos.DrawRay(transform.position, dir);
    }
}
