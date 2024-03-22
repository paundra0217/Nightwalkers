using RDCT;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private float moveSpeed = 2f;
    private float maxDistance = 5f;
    private Transform currentPoint;
    private float random;
    public float time;
    private float cooldown;
    public LayerMask playerMask;
    [SerializeField] GameObject leftBound;
    [SerializeField] GameObject rightBound;
    [SerializeField] GameObject player;
    private Rigidbody2D rb;

    PlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = rightBound.transform;
        time = 1f * Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        random = Random.Range(0, maxDistance);
        CheckPlayer(transform.position,transform.localScale.x);
        if(cooldown >= 0){
            cooldown -= Time.fixedDeltaTime;
        }
    }

    void CheckPlayer(Vector2 pos,float direction){
        Vector2 origin = new Vector2(pos.x + direction * 1f, pos.y + 1f);
        RaycastHit2D eye = Physics2D.Raycast(origin, new Vector2(direction, 0), 6f, playerMask);
        if(eye.rigidbody == player.GetComponent<Rigidbody2D>()){
            Chase(origin, direction);
        }
        else{
            Patrol();
        }

    }
    void Patrol(){
        //Random di boundsnya
        //Random ditambahin di boundsnya
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == rightBound.transform){
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        else{
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
        if(Vector2.Distance(transform.position, currentPoint.position) <= random && currentPoint == rightBound.transform){
            currentPoint = leftBound.transform;
            Debug.Log(currentPoint.position.x);
        }
        if(Vector2.Distance(transform.position, currentPoint.position) <= random && currentPoint == leftBound.transform){
            currentPoint = rightBound.transform;
            Debug.Log(currentPoint.position.x);
        }

    }

    void Chase(Vector2 origin, float direction){
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        RaycastHit2D attackRange = Physics2D.Raycast(origin, new Vector2(direction, 0), 3f, playerMask);
        if(attackRange.rigidbody == player.GetComponent<Rigidbody2D>()){
            Attack();
        }
        if(transform.position.x < leftBound.transform.position.x - 5f){
            rb.velocity = new Vector2(moveSpeed, 0);
        }
        if(transform.position.x > rightBound.transform.position.x + 5f){
            rb.velocity = new Vector2(-moveSpeed, 0);
        }
    }

    void Attack(){
        if(cooldown <= 0){
            Debug.Log("Launch Attack");
            cooldown = time;
        }
    }
}
