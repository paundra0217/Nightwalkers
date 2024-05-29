using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Attacks/NormalAttack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    public float Damage;


}
