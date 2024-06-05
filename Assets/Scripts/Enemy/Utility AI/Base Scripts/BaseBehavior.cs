using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;
using Cinemachine.Utility;


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
    private float meleeRange = 1f;
    private float rangeDistance;
    private bool firstRunPatrol = true;
    private bool isHoundPatrol = false;
    private float xVal;
    private bool isAttacking = false;

    private Vector2 rightPatrol;
    private Vector2 leftPatrol;
    private Vector2 playerLastSeen;
    private Vector2 currPos;
    private Vector2 moveTowards;

    private int stuckCount;

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
    public GameObject orientationL;
    public EdgeCollider2D attackCollider;
    //public EdgeCollider2D front;
    private Animation anims;
    //public GameObject orientationR;

    #endregion

    private void Start()
    {
        currPos = (Vector2)transform.position;
        m_BasePerception = GetComponent<BasePerception>();
        m_AIInfo = GetComponent<AIInfo>();
        m_WayPoint = GetComponent<Waypoint>();
        playerRef = m_BasePerception.findPlayer();
        _currHP = m_AIInfo.getHitPointLeft();
        playerLastSeen = transform.position;
        anims = GetComponent<Animation>();
    }

    private void Update()
    {
        if (m_behaviors.Count == 0)
        {
            m_behaviors.Add(defaultBehavior);
        } else if (m_behaviors.Count > 5)
        {
            m_behaviors.Clear();
        }

        if (m_BasePerception.bLineOfSight == true)
        {
            addAction(chaseBehavior);
        }

        if(m_behaviors.Contains(defaultBehavior))
        {
            Patrol();
            
        } else if (m_behaviors.Contains(chaseBehavior) && isAttacking == false)
        {
            chase();
        }
        if (m_BasePerception.enemyToPlayerRange() > rangeDistance)
        {
            chase();
        }
        if (m_BasePerception.enemyToPlayerRange() > meleeRange)
        {

        }
        if (m_BasePerception.enemyToPlayerRange() < meleeRange)
        {
            attacking();
            isAttacking = true;
        }
        else isAttacking = false;

        if (nowAction == null)
        {
            addAction(defaultBehavior);
        }

        onSimulation();
    }

    private void ScoreProcessing()
    {
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
        CompleteAction();
        ScoreProcessing();
    }

    public Waypoint getPatrolWaypoint()
    {
        Waypoint waypoint = m_WayPoint;

        return waypoint;
    }

    public void Patrol()
    {
        bool patrolingNow = false;

        xVal = transform.position.x;
        transform.position = Vector2.MoveTowards(transform.position, moveTowards, m_AIInfo.getMovespeed()/2.5f * Time.deltaTime);

        RaycastHit2D wallCheckL = Physics2D.Raycast(transform.position, orientationL.transform.position, 0.5f);
        //RaycastHit2D wallCheckR = Physics2D.Raycast(transform.position, orientationR.transform.position, 0.3f);

        var distanceToLeft = Vector2.Distance(transform.position, leftPatrol);
        var distanceToRight = Vector2.Distance(transform.position, rightPatrol);
        bool hitSomething = wallCheckL.collider;//|| wallCheckR.collider;

        if (isHoundPatrol == false && patrolingNow == false && firstRunPatrol == true)
        {
            rightPatrol = currPos + (Vector2.right) * 4;
            leftPatrol = currPos + (Vector2.left) * 4;

            patrolingNow = true;
            firstRunPatrol = true;
        }

        if (patrolingNow == true && firstRunPatrol == true)
        {
                moveTowards = leftPatrol;

                Debug.Log("patrol to Left 1");

                patrolingNow = false;
                firstRunPatrol = false;
        }

        if (xVal <= leftPatrol.x + 0.8)
        {
            Debug.Log("Switch R");
            transform.localScale = new Vector3(-1, 1, 1);
            moveTowards = rightPatrol;
        }

        if (xVal >= rightPatrol.x - 0.8)
        {
            Debug.Log("Switch L");
            transform.localScale = new Vector3(1, 1, 1);
            moveTowards = leftPatrol;
        }

        if (hitSomething)
        {
            Debug.Log("Nabrak");

            stuckCount++;

            if (distanceToLeft > distanceToRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
                moveTowards = leftPatrol;
            } 

            if (distanceToLeft < distanceToRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                moveTowards = rightPatrol;
            }
        }

        if (stuckCount >= 15)
        {
            if (distanceToLeft > distanceToRight)
            {
                playerLastSeen = transform.position + (Vector3.left) * 4;
            }

            if (distanceToLeft < distanceToRight)
            {
                playerLastSeen = transform.position + (Vector3.right) * 4;
            }
            rightPatrol = playerLastSeen + (Vector2.right) * 4;
            leftPatrol = playerLastSeen + (Vector2.left) * 4;

            stuckCount = 0;
        }
    }

    public void houndPatrol()
    {
        rightPatrol = playerLastSeen + (Vector2.right) * 4;
        leftPatrol = playerLastSeen + (Vector2.left) * 4;

        isHoundPatrol = true;

        Patrol();
    }

    public void chase()
    { 
        if (m_BasePerception.bLineOfSight)
        {
            playerLastSeen.x = playerRef.transform.position.x;
            playerLastSeen.y = transform.position.y;
            Debug.Log("Moving towards Player");
            
            if (transform.position.x > playerRef.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * m_AIInfo.getMovespeed() / 2 * Time.deltaTime;
            }
            if (transform.position.x < playerRef.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * m_AIInfo.getMovespeed() / 2 * Time.deltaTime;
            }

        } else if (!m_BasePerception.bLineOfSight)
        {
            chases = 0;
            CompleteAction();
            houndPatrol();
        }
    }

    public void attacking()
    {
        Debug.Log("Attacking");

        if (attackCollider.CompareTag("Player"))
        {
            Debug.Log("Player");
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
        Gizmos.DrawSphere(leftPatrol, 1);
        Gizmos.DrawSphere(rightPatrol, 1);
        Gizmos.DrawLine(transform.position, orientationL.transform.position);
        //Gizmos.DrawLine(transform.position, orientationR.transform.position);
        
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