using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;


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
    public int actionQ;
    private int chases;
    private int rand;
    private int ChangePoint;
    private float meleeRange;
    private float rangeDistance;
    private bool firstRunPatrol = true;

    private Vector2 rightPatrol;
    private Vector2 leftPatrol;
    private Vector2 playerLastSeen;
    

    public List<Transform> patrolPoint;

    [Header("Dont Change")]
    [CanBeNull] public List<ActionBehavior> m_behaviors;
    [CanBeNull] public Waypoint m_WayPoint;
    public GameObject waypointPref;
    public ActionBehavior nowAction;
    public ActionBehavior nextAction;
    public ActionBehavior previousAction;

    [Header("Behavior Handler")]
    public ActionBehavior defaultBehavior;

    public ActionBehavior idleBehavior;
    public ActionBehavior chaseBehavior;
    public ActionBehavior attackBehavior;

    [Header("Refrences")]
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
        if (m_behaviors.Count == 0)
        {
            m_behaviors.Add(defaultBehavior);
        }

        if (m_BasePerception.bLineOfSight == true)
        {
            addAction(chaseBehavior);
            CompleteAction();
            
            chase();
        }

        if(m_behaviors.Contains(defaultBehavior))
        {

        }

        onSimulation();
    }

    private void ScoreProcessing()
    {
        if (!nowAction.getIntruption())
            return;

        if (nextAction.IsUnityNull())
            return;

        if (GetBehaviorScoreNow() < nextAction.getScore())
        {
            if (m_behaviors[1] != null)
            {
                if (m_behaviors[0] = m_behaviors[1])
                    m_behaviors.Remove(m_behaviors[1]);
            }
            else return;

            for (int i = m_behaviors.Count; i == 0; i--)
            {
                for (int j = i - 1; j == 1; j--)
                {
                    var score = m_behaviors[i].getScore();
                    var nextScore = m_behaviors[j].getScore();

                    if (score > nextScore)
                        swap<ActionBehavior>(m_behaviors, j, i);
                    else return;
                }
            }
        }
    }


    private void onSimulation()
    {
        if (m_behaviors.Count == 0) return;

        nowAction = m_behaviors[0];
        if (m_behaviors.Count > 1)
        {
            if (m_behaviors[1] != null)
                nextAction = m_behaviors[1];
        }
        nowAction.OnSelected();
        nowAction.simulate();
    }

    public void CompleteAction()
    {
        previousAction = nowAction;
        m_behaviors.RemoveAt(0);
        ScoreProcessing();
    }

    public void removeAction(ActionBehavior remove)
    {
        m_behaviors.Remove(remove);
        nowAction = nextAction;
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
        var currPos = transform.position;

        rightPatrol = currPos.normalized + transform.right;
        leftPatrol = currPos.normalized - transform.right;
        
        
    }

    public void houndPatrol()
    {
        
    }

    public void chase()
    { 
        m_WayPoint = null;

        if (m_BasePerception.bLineOfSight)
        {
            playerLastSeen = playerRef.transform.position;
            Debug.Log("Moving towards Player");
            transform.position = Vector2.MoveTowards(transform.position, playerRef.transform.position, m_AIInfo.getMovespeed() * Time.deltaTime);
        } else if (!m_BasePerception.bLineOfSight)
        {
            chases = 0;
            CompleteAction();
            houndPatrol();
        }
    }

    public void attacking()
    {
        if (m_BasePerception.enemyToPlayerRange() > rangeDistance)
        {

        } else if (m_BasePerception.enemyToPlayerRange() > meleeRange)
        {

        } else
        {

        }
    }

    public static IList<T> swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
        return list;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(leftPatrol, 2);
        Gizmos.DrawSphere(rightPatrol, 2);  
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
    public bool bIsSelected = false;
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
        
        baseBehavior.m_behaviors.Remove(this);
        baseBehavior.removeAction(this);
    }

    public virtual void onComplete()
    {
        Debug.Log("Simulation Complete");
        OnStopSimulate();
        baseBehavior.m_behaviors.Remove(this);
        baseBehavior.CompleteAction();
    }
}