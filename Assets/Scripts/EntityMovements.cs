/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovements : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    private new Rigidbody2D rigidBody;
    private Vector2 velocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidBody.WakeUp();
    }

    private void OnDisable()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.Sleep();
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);

        if (rigidBody.Raycast(direction))
        {
            direction = -direction;
        }

        if (rigidBody.Raycast(Vector2.down))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }
}*/
