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
            Debug.Log("damage enemy");
            AIInfo aIInfo = collision.GetComponent<AIInfo>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            //Take Damage
            aIInfo.TakeDamage(Damage);

            //Enemy AddForce
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            rb.AddForce(direction * 5f, ForceMode2D.Impulse);
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
