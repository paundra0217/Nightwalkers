using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject platform;
    public BoxCollider2D coll;
    void Update()
    {
        coll = platform.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        coll.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        coll.enabled = true;
    }
}
