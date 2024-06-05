using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        door1.SetActive(true);
        door2.SetActive(true);
        door3.SetActive(true);
        Destroy(gameObject);
    }
}
