using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMove : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;

    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; 
    public float timer; 
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool inRange; 
    private bool cooling; 
    private float intTimer;
    #endregion

    private void Awake()
    {
        intTimer = timer;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
        }

        if (hit.collider != null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;
        }
        if (inRange == false)
        {
            anim.SetBool("Attack", false);
        }

        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;

            }
            else if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }
        }

        else
        {

            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
                anim.SetBool("Attack", true);
            }

            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance * 1.2f) isChasing = false;


            if (patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    patrolDestination = 1;
                }
            }

            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    patrolDestination = 0;
                }
            }


            if (!isChasing) anim.SetBool("Attack", false);
        }

        void Attack()
        {
            timer = intTimer;
            attackMode = true;
            anim.SetBool("Attack", true);
        }

        void OnTriggerEnter2D(Collider2D trig)
        {
            if (trig.gameObject.tag == "Player")
            {
                target = trig.gameObject;
                inRange = true;
            }
        }

        void EnemyLogic()
        {
            distance = Vector2.Distance(transform.position, target.transform.position);

            if (attackDistance >= distance)
            {
                Attack();
            }
        }
    }
    public void Colldowntrigger()
    {
        cooling = true;
    }
}
