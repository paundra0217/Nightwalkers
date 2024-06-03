using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform2 : MonoBehaviour
{
    [SerializeField] private Transform posA, posB;
    [SerializeField] private float speed;
    [SerializeField] private float waitDuration;
    [SerializeField] private Rigidbody2D playerRB;
    private Vector2 targetPos;

    private void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        targetPos = posB.position;
    }

    private void Update()
    {
        Moving();    
    }

    private void Moving()
    {
        if(Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
        }
        if(Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            coll.transform.parent = this.transform;
            playerRB.gravityScale = playerRB.gravityScale * 50;
        }
    }

    private void OnTriggerExit2D (Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            coll.transform.parent = null;
            playerRB.gravityScale = playerRB.gravityScale / 50;
        }
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        Moving();
    }
}
