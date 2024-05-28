using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePerception : MonoBehaviour
{
    public static EnemyBaseProperties m_BaseProp;

    private static float fNormalFactor = 10f;
    private static float fChaseFactor = 15f;

    private GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_BaseProp = GetComponent<EnemyBaseProperties>();


    }

    // Update is called once per frame
    void Update()
    {
        if (m_BaseProp == null) return; 

        findPlayer();
    }

    public GameObject findPlayer()
    {
        if (m_Player = null)
        {
            m_Player = GameObject.FindGameObjectWithTag("Player");
        }

        return m_Player;
    }


}
