using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI Idle Action")]
public class IdleAction : ActionBehavior
{
    private void OnSelected()
    {
        ScoreAction = BEHAVIOR_IDLE;
        bIsInteruptable = true;
        bIsAttackable = false;
    }


    void Start()
    {
        OnSelected();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
