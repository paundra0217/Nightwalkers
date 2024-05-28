using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/AI Base Prop")]
public class EnemyBaseProperties : ScriptableObject
{
    [Header("General Properties")]
    [Tooltip("Name of the enemy")] public  string _sName;
    [Tooltip("Brief Description")][TextAreaAttribute(3,5)] public string _sDescription;

    [Header("Attribute Stats")]
    [Tooltip("HP of the enemy")] public  float _fHitpoints;
    [Tooltip("Damage of the enemy")] public  float _fDamage;
    [Tooltip("Move Speed of the enemy")] public  float _fMoveSpeed;
    [Tooltip("Attack Speed of the enemy")] public  float _fAttSpeed;
    [Tooltip("Stunned Duration")] public  float _fStunnedDuration;
    [Tooltip("Attack Range Melle")] public float _fAttackRangeM;
    [Tooltip("Attack Range Ranged")] public float _fAttackRangeR;

    [Header("Attack")]
    [Tooltip("Attack Anim")] public AnimatorController[] _aAttackHandler;

    [Header("Perception Factor")]
    [Tooltip("Default enemy Factor")] public float _fperceptionBase;

}
