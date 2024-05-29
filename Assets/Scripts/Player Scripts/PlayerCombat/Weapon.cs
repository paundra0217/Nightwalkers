using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage;
    Collider2D triggerBox;

    private void Start()
    {
        triggerBox = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AIInfo>())
        {
            AIInfo aIInfo = collision.GetComponent<AIInfo>();
            aIInfo.TakeDamage(Damage);


        }
    }

    public void TriggerON()
    {
        triggerBox.enabled = true;
    }

    public void TriggerOff()
    {
        triggerBox.enabled = false;
    }



}
