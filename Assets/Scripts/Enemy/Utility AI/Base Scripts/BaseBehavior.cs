using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static BasePerception          m_BasePerception;
    public static AIInfo                  m_AIInfo;

    #endregion

    #region Variables

    private int _currActionScore;
    private float _currHP;

    public List<Transform> patrolPoint;

    [CanBeNull] public List<ActionBehavior> m_behaviors;
    [CanBeNull] public Waypoint m_WayPoint;
    public ActionBehavior nowAction;
    public ActionBehavior nextAction;
    public ActionBehavior previousAction;

    public ActionBehavior defaultBehavior;

    [CanBeNull] public GameObject playerRef;
    [NotNull] public GameObject chaseEndPatrolPrefab;

    #endregion

    private void Start()
    {
        m_BasePerception = GetComponent<BasePerception>();
        m_AIInfo = GetComponent<AIInfo>();
        m_WayPoint = GetComponent<Waypoint>();
        playerRef = m_BasePerception.findPlayer();

        _currHP = m_AIInfo.getHitPointLeft();
    }


    private void Update()
    {
        if (m_behaviors.IsUnityNull())
        {
            m_behaviors.Add(defaultBehavior);
        }

        onSimulation();
    }

    private void ScoreProcessing()
    {
        var bIsInteruptable = nowAction.getIntruption();
        var iActionScore = nowAction.getScore();

        if (!bIsInteruptable)
            return;
        
        if (GetBehaviorScoreNow() < iActionScore)
        {
            if (m_behaviors[2] != null)
            {
                if (m_behaviors[1] = m_behaviors[2])
                    m_behaviors.Remove(m_behaviors[2]);
            }

            for (int i = m_behaviors.Count; i == 0; i--)
            {
                for (int j = i - 1; j == 1; j--)
                {
                    var score = m_behaviors[i].getScore();
                    var nextScore = m_behaviors[j].getScore();

                    if (score > nextScore)
                        swap<ActionBehavior>(m_behaviors, i, j);
                    else return;
                }
            }
        }

        if (m_behaviors.Count == 10)
        {
            int iRange = m_behaviors.Count;
        }
    }


    private void onSimulation()
    {
        //nowAction.GetComponent<ActionBehavior>();
        if (m_behaviors.Count == 0) return;

        nowAction = m_behaviors[1].GetComponent<ActionBehavior>();

        if (nowAction.bIsSelected == true)
        {
            nowAction.OnSelected();
            nowAction.simulate();
        } else if (nowAction.bIsSelected == false)
        {
            ScoreProcessing();
        }
    }

    public void CompleteAction()
    {
        previousAction = nowAction;
        m_behaviors.RemoveAt(0);
        ScoreProcessing();
    }

    private int GetBehaviorScoreNow()
    {
        _currActionScore = nowAction.getScore();

        return _currActionScore;
    }

    public void addAction(ActionBehavior actions)
    {
        m_behaviors.Add(actions);
        ScoreProcessing();
    }

    public Waypoint getPatrolWaypoint()
    {
        Waypoint waypoint = m_WayPoint;

        return waypoint;
    }

    public void Patrol()
    {
        getPatrolWaypoint();

        if (m_WayPoint.waypointAvail() == false)
            return;

        for (int i = 0; i > m_WayPoint.wayPointCount(); i++)
        {
            patrolPoint.Add(m_WayPoint.getPatrolPos(i));
        }


    }

    public void houndPatrol()
    {
        
    }

    public void chase()
    {
        Destroy(m_WayPoint.gameObject);
        m_WayPoint = null;

        transform.position = Vector2.MoveTowards(transform.position, m_BasePerception.m_Player.transform.position, m_AIInfo.getMovespeed() * Time.deltaTime);
        
    }

    public static IList<T> swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
        return list;
    }
}


public class ActionBehavior : ScriptableObject
{
    #region Action Score Value

    [HideInInspector] public static int BEHAVIOR_STUNNED = 15;
    [HideInInspector] public static int BEHAVIOR_HIGH_ATTACK = 10;
    [HideInInspector] public static int BEHAVIOR_ATTACK = 8;
    [HideInInspector] public static int BEHAVIOR_CHASE = 5;
    [HideInInspector] public static int BEHAVIOR_PATROL = 3;
    [HideInInspector] public static int BEHAVIOR_IDLE = 1;

    #endregion

    [HideInInspector] public int ScoreAction;
    public bool bIsInteruptable = true;
    [HideInInspector] public bool bIsAttackable;
    [HideInInspector] public bool bIsSelected = false;
    [HideInInspector] public BaseBehavior baseBehavior;

    public int getScore()
    {
        return ScoreAction;
    }

    public bool getIntruption()
    {
        return bIsInteruptable;
    }

    public virtual void OnSelected()
    {
        baseBehavior.GetComponent<BaseBehavior>();
        bIsSelected = true;
        simulate();
    }

    public virtual void simulate() { }

    public virtual void OnStopSimulate()
    {
        if (bIsInteruptable == false)
        {
            return;
        } else if (bIsInteruptable == true)
        {
            bIsSelected = false;
        }
    }

    public virtual void onComplete()
    {
        Debug.Log("Simulation Complete");
        OnStopSimulate();
        baseBehavior.m_behaviors.Remove(this);
        baseBehavior.CompleteAction();
    }
}