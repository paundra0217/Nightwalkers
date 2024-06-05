using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAreaPlatform : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject platfrom1;
    public GameObject platfrom2;
    private bool doesTagExist = true;
    [SerializeField]private string enemyTag;
    void Update()
    {
        doesTagExist = GameObject.FindWithTag(enemyTag);
        if(doesTagExist == false)
        {
            Destroy(door1);
            Destroy(door2);
            platfrom1.SetActive(true);
            platfrom2.SetActive(true);
        }
    }
}
