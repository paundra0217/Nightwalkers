using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileSO projectileSO;



    private void FixedUpdate()
    {
        transform.Translate(Vector3.right * projectileSO.speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AIInfo>())
        {
            AIInfo enemy = collision.gameObject.GetComponent<AIInfo>();
            enemy.TakeDamage(projectileSO.damage);
        }

        Destroy(gameObject);
    }
}
