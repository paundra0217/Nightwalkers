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

    public static EnemyBaseProperties     m_BaseProperties;
    public static BasePerception          m_BasePerception;
    public static AIInfo                  m_AIInfo;

    #endregion

    #region Variables

    private int _currActionScore;
    private float _currHP;


    public List<ActionBehavior> m_behaviors;
    public Waypoint m_WayPoint;
    public ActionBehavior nowAction;
    public ActionBehavior nextAction;
    // public ActionBehavior previousAction;

    #endregion

    private void Start()
    {
        m_BasePerception = GetComponent<BasePerception>();
        m_BaseProperties = GetComponent<EnemyBaseProperties>();
        m_AIInfo = GetComponent<AIInfo>();
        m_WayPoint = GetComponent<Waypoint>();
    }


    private void Update()
    {
        if (m_BaseProperties == null) { return ; }
        _currHP = m_AIInfo.getHitPointLeft();

        onSimulation();
    }

    private void ScoreProcessing(ActionBehavior nextActions)
    {
        var bIsInteruptable = nowAction.getIntruption();
        var iActionScore = nowAction.getScore();

        if (!bIsInteruptable)
            return;
        
        if (GetBehaviorScoreNow() < iActionScore)
        {
            if (m_behaviors[1] != null)
            {
                if (m_behaviors[0] = m_behaviors[1])
                    m_behaviors.Remove(m_behaviors[1]);
            }
            

            //addAction(nextActions);
            for (int i = m_behaviors.Count; i == 0; i--)
            {
                for (int j = i-1; j <= i; j--)
                {
                    var score = m_behaviors[i].getScore();
                    var nextScore = m_behaviors[j].getScore();

                    if (score > nextScore)
                    {
                        var now = m_behaviors[i];
                        var next = m_behaviors[j];

                        if (j < 0) return;

                        swap<ActionBehavior>(m_behaviors, i, j);
                    }
                }
            }

            
            //m_behaviors.RemoveAt(0);
        }
    }


    private void onSimulation()
    {
        //nowAction.GetComponent<ActionBehavior>();
        if (m_behaviors[0] == null) return;

        nowAction = m_behaviors[0].GetComponent<ActionBehavior>();

        if (nowAction.bIsSelected == true)
        {
            nowAction.OnSelected();
            nowAction.simulate();
        }
            
    }

    private int GetBehaviorScoreNow()
    {
        _currActionScore = nowAction.getScore();

        return _currActionScore;
    }

    public void addAction(ActionBehavior actions)
    {
        m_behaviors.Add(actions);
        ScoreProcessing(actions);
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
    public BaseBehavior baseBehavior;

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
        bIsSelected = false;
    }

    public virtual void simulate() { }
}