using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;
using Cinemachine.Utility;
using RDCT;


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
    Animator animators;
    //public GameObject orientationR;

    #endregion

    bool ReadyAttack = true;
    private void Start()
    {
        currPos = (Vector2)transform.position;
        m_BasePerception = GetComponent<BasePerception>();
        m_AIInfo = GetComponent<AIInfo>();
        m_WayPoint = GetComponent<Waypoint>();
        playerRef = m_BasePerception.findPlayer();
        _currHP = m_AIInfo.getHitPointLeft();
        playerLastSeen = transform.position;
        animators = GetComponent<Animator>();
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
            if (ReadyAttack)
            {
                StartCoroutine(attacking());
            }
                isAttacking = true;
        }
        else
        {
            isAttacking = false;
            animators.SetBool("Attack", false);
        }
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

        bool moveToLeft = false, moveToRight = false;

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
            moveToRight = true;
            moveToLeft = false;
        }

        if (xVal >= rightPatrol.x - 0.8)
        {
            Debug.Log("Switch L");
            transform.localScale = new Vector3(1, 1, 1);
            moveTowards = leftPatrol;
            moveToRight = false;
            moveToLeft = true;
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

        if (stuckCount >= 10)
        {
            if (moveToRight)
            {
                playerLastSeen = transform.position + (Vector3.left) * 15;
            }

            if (moveToLeft)
            {
                playerLastSeen = transform.position + (Vector3.right) * 15;
            }

            rightPatrol = playerLastSeen + (Vector2.right) * 4;
            leftPatrol = playerLastSeen + (Vector2.left) * 4;

            stuckCount = 0;
        }
    }

    public void houndPatrol()
    {
        rightPatrol = playerLastSeen + (Vector2.right) * 5;
        leftPatrol = playerLastSeen + (Vector2.left) * 5;

        isHoundPatrol = true;

        Patrol();
    }

    public void chase()
    {
        if (isAttacking)
            return;

        if (m_BasePerception.bLineOfSight)
        {
            playerLastSeen.x = playerRef.transform.position.x;
            playerLastSeen.y = transform.position.y;
            Debug.Log("Moving towards Player");
            
            if (transform.position.x > playerRef.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * m_AIInfo.getMovespeed() / 4 * Time.deltaTime;
            }
            if (transform.position.x < playerRef.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * m_AIInfo.getMovespeed() / 4 * Time.deltaTime;
            }

        } else if (!m_BasePerception.bLineOfSight)
        {
                chases = 0;
                CompleteAction();
                houndPatrol();
        }    
    }

    public IEnumerator attacking()
    {
        Debug.Log("Attacking");
        //animators.SetBool("Attack", true);
        ReadyAttack = false;
        animators.SetTrigger("Attacking");
        yield return new WaitForSeconds(2f);
        ReadyAttack = true;
       
    }

    [SerializeField] private LayerMask PlayerLayer;

    public void AttackBeneran()
    {
        
        Collider2D Player = Physics2D.OverlapCircle(transform.position, 1f, PlayerLayer);

        Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
        PlayerCombat pcon = Player.GetComponent<PlayerCombat>();
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        if (pcon.IsParry)
        {
            pcon.Parried();
            Debug.Log("berhasil");
            
        }
        else
        {
            Debug.Log("gagal");
            StartCoroutine(pcon.TakeDamage(m_AIInfo.getDamage()));
            
            rb.AddForce(direction * 10f, ForceMode2D.Impulse);
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