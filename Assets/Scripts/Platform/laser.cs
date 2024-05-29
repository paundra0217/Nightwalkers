using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class laser : MonoBehaviour
{
    private BoxCollider2D laserCollider;
    private SpriteRenderer laserSprite;
    [SerializeField]private float laserTimeActive;
    [SerializeField]private float laserTimeStart;
    private bool laserIsOn;

    void Awake()
    {
        laserCollider = GetComponent<BoxCollider2D>();
        laserSprite = GetComponent<SpriteRenderer>();
        laserIsOn = false;
    }
    
    void Update()
    {
        if(laserIsOn == true)
        {
            laserCollider.enabled = true;
            laserSprite.enabled = true;
        }
        else if(laserIsOn == false){
            laserCollider.enabled = false;
            laserSprite.enabled = false;
            StartCoroutine(StartLaser());
        }
    }

    IEnumerator StartLaser()
    {
        yield return new WaitForSeconds(laserTimeStart);
        laserIsOn = true;
        yield return new WaitForSeconds(laserTimeActive);
        laserIsOn = false;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            takeDamage();
        }
    }
    
    void takeDamage()
    {
        Debug.Log("hit");
    }

}
