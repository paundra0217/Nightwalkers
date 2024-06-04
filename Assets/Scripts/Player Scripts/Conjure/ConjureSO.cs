using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConjureSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    //public float BulletDamage;
    public GameObject Projectile;
    public float BulletCost;
    
    [Header("Instantiate Options")]
    [Tooltip("How Many Ammo Shoot only 1 click")]
    public int BurstAmount; 

}
