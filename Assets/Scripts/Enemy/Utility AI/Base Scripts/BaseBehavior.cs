using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EAIStates
{
    Neutral,
    Attack,
    Chase,
    Stun
}


public class BaseBehavior : MonoBehaviour
{
    #region Module

    public static EnemyBaseProperties     m_BaseProperties;
    public static BasePerception          m_BasePerception;
    public static AIInfo                  m_AIInfo;

    #endregion

    #region Action Score

    private static int BEHAVIOR_HIGH_ATTACK;
    private static int BEHAVIOR_ATTACK;
    private static int BEHAVIOR_CHASE;
    private static int BEHAVIOR_PATROL;

    #endregion

    #region

    private int ScoreAction;

    #endregion

    private void Start()
    {
        m_BasePerception = GetComponent<BasePerception>();
        m_BaseProperties = GetComponent<EnemyBaseProperties>();
        m_AIInfo = GetComponent<AIInfo>();
    }


    private void Update()
    {
        if (m_BaseProperties == null) { return ; }

        m_AIInfo.getHitPointLeft();
    }

    private void ScoreCounting()
    {
        
    }

    private int GetBehaviorScore()
    {
        return ScoreAction;
    }
}
