using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player/Flow Data")]
public class PlayerCombatStat : ScriptableObject
{

    [Header("Stats")]
    [Tooltip("Luna MaxHp")]
    public float Maxhp = 5;

    [Header("Flow")]
    [Tooltip("MaxFlowGauge")]
    public float MaxFlow = 100;




}
