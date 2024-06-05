using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Conjure Data")]
public class ConjureSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    public GameObject Projectile;
    public float BulletCost;
    
    [Header("Instantiate Options")]
    [Tooltip("How Many Ammo Shoot only 1 click")]
    public int BurstAmount; 

}
