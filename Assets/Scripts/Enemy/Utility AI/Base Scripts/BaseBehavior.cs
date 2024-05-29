using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EAIStates
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

    private static int BEHAVIOR_STUNNED = 15;
    private static int BEHAVIOR_HIGH_ATTACK = 10;
    private static int BEHAVIOR_ATTACK = 8;
    private static int BEHAVIOR_CHASE = 5;
    private static int BEHAVIOR_PATROL = 3;
    private static int BEHAVIOR_IDLE = 1;

    #endregion

    #region Action Var

    private int ScoreAction, ScoreAction1, ScoreAction2, ScoreAction3, ScoreAction4, ScoreAction5, ScoreAction6;
    private bool bIsInteruptable = true;
    private bool bIsAttackable;

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

    private void ScoreProcessing(int NextAction)
    {
        if (!bIsInteruptable)
            return;

        if (ScoreAction < NextAction)
        {
            
        }
    }

    private int GetBehaviorScore()
    {
        return ScoreAction;
    }

    private void NormalBehavior()
    {
        ScoreAction1 = BEHAVIOR_IDLE;
        ScoreAction = ScoreAction1;
        bIsInteruptable = true;
    }

    private void AttackBehavior()
    {
        ScoreAction2 = BEHAVIOR_ATTACK;
        ScoreAction = ScoreAction2;
        bIsInteruptable = false;
    }

    private void ChaseBehavior() 
    {
        ScoreAction3 = BEHAVIOR_CHASE;
        ScoreAction = ScoreAction3;
        bIsInteruptable = false;

        if (bIsAttackable)
            bIsInteruptable = true;
    }

    private void StunBehavior()
    {
        ScoreAction4 = BEHAVIOR_STUNNED;
        ScoreAction = ScoreAction4;
        bIsInteruptable = false;
    }

}
