using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    private bool doesTagExist = true;
    [SerializeField]private string enemyTag;
    void Update()
    {
        doesTagExist = GameObject.FindWithTag(enemyTag);
        if(doesTagExist == false)
        {
            Destroy(door1);
            Destroy(door2);
            Destroy(door3);
        }
    }
}
