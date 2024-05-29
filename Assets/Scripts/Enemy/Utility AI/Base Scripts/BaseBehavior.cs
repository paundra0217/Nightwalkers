using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.Rendering;


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

    #region Variables

    private int _currActionScore;
    private float _currHP;


    public List<ActionBehavior> m_behaviors;
    public ActionBehavior nowAction;
    public ActionBehavior nextAction;

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
        _currHP = m_AIInfo.getHitPointLeft();
    }

    private void ScoreProcessing(ActionBehavior nextActions)
    {
        var bIsInteruptable = nowAction.getIntruption();
        var iActionScore = nowAction.getScore();

        if (!bIsInteruptable)
            return;
        
        if (GetBehaviorScoreNow() < iActionScore)
        {
            addAction(nextActions);
            for (int i = m_behaviors.Count; i == 0; i--)
            {
                var score = m_behaviors[i].getScore();
                var nextScore = m_behaviors[i-1].getScore();

                if (score > nextScore)
                {
                    var now = m_behaviors[i];
                    var next = m_behaviors[i-1];

                   // m_behaviors.TrySwap<ActionBehavior>(i, i--);
                }

            }

            m_behaviors.RemoveAt(0);
        }
    }

    private int GetBehaviorScoreNow()
    {
        _currActionScore = nowAction.getScore();

        return _currActionScore;
    }

    private void addAction(ActionBehavior actions)
    {
        nextAction = actions;
        m_behaviors.Add(actions);
    }
}


public class ActionBehavior : ScriptableObject
{
    #region Action Score

    public static int BEHAVIOR_STUNNED = 15;
    public static int BEHAVIOR_HIGH_ATTACK = 10;
    public static int BEHAVIOR_ATTACK = 8;
    public static int BEHAVIOR_CHASE = 5;
    public static int BEHAVIOR_PATROL = 3;
    public static int BEHAVIOR_IDLE = 1;

    #endregion

    [HideInInspector] public int ScoreAction;
    public bool bIsInteruptable = true;
    public bool bIsAttackable;

    public int getScore()
    {
        return ScoreAction;
    }

    public bool getIntruption()
    {
        return bIsInteruptable;
    }
}