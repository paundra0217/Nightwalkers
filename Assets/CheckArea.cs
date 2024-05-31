using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    public GameObject door;
    private bool doesTagExist = true;
    void Update()
    {
        doesTagExist = GameObject.FindWithTag("Area1");
        if(doesTagExist == false)
        {
            Destroy(door);
        }
    }
}
