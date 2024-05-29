using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI Base Attack Handler")]
public class AIAttackHandlers : ScriptableObject
{
    [Tooltip("Animation of the Attack")] public Animation AttackNormal;
    [Tooltip("Damage of the Attack")] public float AttackDamage;
    [Tooltip("Range of the Attack")] public float attackRange;
    [Tooltip("Speed of the Attack")] public float attackSpeed;

    [CanBeNull][Tooltip("Projectile of the attack, if there are none then leave it empty")] public GameObject projectile;
}
