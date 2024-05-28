using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPProjectile : ScriptableObject
{
    // Base Properties and Projectile Scripts for Stats

    [Header("Properties")]

    [Tooltip("Projectile Name")] public string _sProjectileName;

    [Tooltip("Projectile Speed")] public float _fProjectileSpeed;

    float getSpeed()
    {
        return _fProjectileSpeed;
    }
}
