using UnityEngine;


[CreateAssetMenu(menuName = "AI/AI Base Prop")]
public class EnemyBaseProperties : ScriptableObject
{
    [Header("General Properties")]
    [Tooltip("Name of the enemy")] public  string _sName;
    [Tooltip("Brief Description")][TextAreaAttribute(3,5)] public string _sDescription;

    [Header("Attribute Stats")]
    [Tooltip("HP of the enemy")] public  float _fHitpoints;
    [Tooltip("Move Speed of the enemy")] public  float _fMoveSpeed;
    [Tooltip("Stunned Duration")] public  float _fStunnedDuration;
    [Tooltip("Knockback Resistant of the Enemy")] public float _fKnockbackRes;

    [Header("Perception Factor")]
    [Tooltip("Default enemy Factor")] public float _fperceptionBase;
    
}
